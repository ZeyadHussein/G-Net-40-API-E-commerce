using AutoMapper;
using E_commerce.Application.Common;
using E_commerce.Application.Contracts;
using E_commerce.Application.DTOS.Baskets;
using E_commerce.Application.Specifications;
using E_commerce.Domain.Contracts;
using E_commerce.Domain.Entities.Orders;
using E_commerce.Domain.Entities.Products;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentGetway _paymentGetway;
        private readonly PaymentGetwaySettings _stripeSettings;
        private readonly IMapper _mapper;

        public PaymentService(IBasketRepository basketRepository,IUnitOfWork unitOfWork,IPaymentGetway paymentGetway,
            IOptions<PaymentGetwaySettings> stripeSettings,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentGetway = paymentGetway;
            _stripeSettings = stripeSettings.Value;
            _mapper = mapper;
        }
        public async Task<Result<BasketDto>> CreateOrUpdatePaymentIntentAsync(string basketId, CancellationToken ct = default)
        {
            var basket=await _basketRepository.GetBasketAsync(basketId,ct);
            if (basket == null)
                return Result<BasketDto>.Fail(Error.NotFound("Basket Not Found", $"Basket with Id{basketId}is not found"));
            if(basket.Items.Count == 0)
                return Result<BasketDto>.Fail(Error.Validation("Basket Is Empity", $"Canot create order Basket with Id{basketId}is Empity"));
            var productRepo = _unitOfWork.GetRepository<Product, int>();
            var productIds = basket.Items.Select(i => i.Id).ToHashSet();
            var products = (await productRepo.GetAllAsync(new ProductWithIdsSpecifications(productIds), ct)).ToDictionary(x => x.Id);
            foreach(var item in basket.Items)
            {
                if (!products.TryGetValue(item.Id, out var product))
                    return Result<BasketDto>.Fail(Error.NotFound("Product Not Found", $"Product with Id{item.Id}is not found"));
                item.Price= product.Price;

            }
            var deliveryRepo = _unitOfWork.GetRepository<DeliveryMethod, int>();
            if (!basket.DeliveryMethodId.HasValue)
            {
                return Result<BasketDto>.Fail(
                    Error.Validation(
                        "Delivery Method",
                        "Please select a delivery method for the basket."));
            }
            var deliveryMethod = await deliveryRepo.GetByIdAsync(basket.DeliveryMethodId.Value, ct);
            if (deliveryMethod == null)
                return Result<BasketDto>.Fail(Error.NotFound("Delivery Not Found", $"Delivery with id{basket.DeliveryMethodId}is not found"));
            basket.ShippingPrice = deliveryMethod.Price;
            var subtotal = basket.Items.Sum(i => i.Quantity * i.Price);
            var amount = (long)Math.Round((subtotal + deliveryMethod.Price) * 100m);
            if(!string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                await _paymentGetway.UpdatePaymentIntentAsync(basket.PaymentIntentId, amount, ct);

            }
            else
            {
                var result = await _paymentGetway.CreatePaymentIntentAsync(amount, _stripeSettings.DefaultCurrency, ct);
                basket.PaymentIntentId = result.PaymentIntentId;
                basket.ClientSecret= result.ClientSecret;
            }
            await _basketRepository.CreateOrUpdateBasketAsync(basket, ct:ct);
            return _mapper.Map<BasketDto>(basket);


        }

        public async Task PaymentFailed(string paymentIntentId)
        {
            var OrderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var order = await OrderRepo.GetByIdAsync(new PaymentIntentSpec(paymentIntentId));
            if (order == null)
                return;
            order.Status = OrderStatus.PaymentFailed;
            await _unitOfWork.SaveChangesAsync();

        }

        public async Task PaymentSucceeded(string paymentIntentId)
        {
            var OrderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var order=await OrderRepo.GetByIdAsync(new PaymentIntentSpec(paymentIntentId));
            if (order == null)
                return;
            order.Status = OrderStatus.PaymentReceived;
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
