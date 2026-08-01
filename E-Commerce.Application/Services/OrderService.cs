using AutoMapper;
using E_commerce.Application.Common;
using E_commerce.Application.Contracts;
using E_commerce.Application.DTOS.Order;
using E_commerce.Application.Specifications;
using E_commerce.Domain.Contracts;
using E_commerce.Domain.Entities.Orders;
using E_commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Services
{
    public class OrderService(IMapper mapper, IUnitOfWork unitOfWork, IBasketRepository basketRepository) : IOrderService
    {
        public async Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto orderDto, string email, CancellationToken ct = default)
        {
           var basket=await basketRepository.GetBasketAsync(orderDto.BasketId,ct);
            if (basket == null)
                return Result<OrderToReturnDto>.Fail(Error.NotFound("Basket Not Found", $"Basket With Id{orderDto.BasketId}Is Not Found"));
            if (basket.Items.Count == 0)
                return Result<OrderToReturnDto>.Fail(Error.Validation("Basket Is Empty", $"Can Not Create Order With Basket With Id{orderDto.BasketId}"));

            var orderRepo = unitOfWork.GetRepository<Order, Guid>();
            var productRepo=unitOfWork.GetRepository<Product,int>();

            var productIds = basket.Items.Select(i => i.Id).ToHashSet();
            var products = (await productRepo.GetAllAsync(new ProductWithIdsSpecifications(productIds), ct)).ToDictionary(x=>x.Id);
            var orderItems = new List<OrderItem>(basket.Items.Count);
            foreach(var item in basket.Items)
            {
                if (!products.TryGetValue(item.Id, out var product))
                    return Result<OrderToReturnDto>.Fail(Error.NotFound("product not found", $"product with id{item.Id}not foind"));

                orderItems.Add(new OrderItem
                {
                    Price = product.Price,
                    Quantity = item.Quantity,
                    product=new ProductItemOrdered
                    {
                        ProductId=product.Id,
                        ProductName=product.Name,
                        PictureUrl=product.PictureUrl,

                    }
                });


            }

            var orderAddress=mapper.Map<OrderAddress>(orderDto.ShipToAddress);
            var deliveryRepo=unitOfWork.GetRepository<DeliveryMethod, int>();
            var deliveryMethod = await deliveryRepo.GetByIdAsync(orderDto.DeliveryMethodId,ct);
            if (deliveryMethod == null)
                return Result<OrderToReturnDto>.Fail(Error.NotFound("Delivery Method Not Found", $"Delivery method with id{orderDto.DeliveryMethodId}is not found"));
            var subTotal = orderItems.Sum(x => x.Quantity * x.Price);
            var order=new Order(email,orderItems,orderAddress,deliveryMethod,subTotal);
            orderRepo.Add(order);
            var result = await unitOfWork.SaveChangesAsync(ct);

            if(result<=0)
            {
                return Result<OrderToReturnDto>.Fail(Error.Failure("Order Save Failed", "Canot Create order"));

            }
            await basketRepository.DeleteBasketAsync(orderDto.BasketId, ct);
            return mapper.Map<OrderToReturnDto>(order);



        }

        public async Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetAllDeliveryMethodsAsync(CancellationToken ct = default)
        {
            var deliveryMethods = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync(ct);
            return Result<IReadOnlyList<DeliveryMethodDto>>.Ok(mapper.Map<IReadOnlyList<DeliveryMethodDto>>(deliveryMethods));
        }

        public async Task<Result<IReadOnlyList<OrderToReturnDto>>> GetAllOrdersAsync(string email, CancellationToken ct = default)
        {
            var orders = await unitOfWork.GetRepository<Order, Guid>()
                .GetAllAsync(new OrderSpecifications(email), ct);
            return Result<IReadOnlyList<OrderToReturnDto>>.Ok(mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders));
        }

        public async Task<Result<OrderToReturnDto>> GetOrderByIdAndEmailAsync(Guid Id, string email, CancellationToken ct = default)
        {
            var order = await unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(new OrderSpecifications(Id, email), ct);
            if (order == null)
                return Result<OrderToReturnDto>.Fail(Error.NotFound("Order Not Found", $"Order With Id{Id}not found"));
            return mapper.Map<OrderToReturnDto>(order);
        }
    }
}
