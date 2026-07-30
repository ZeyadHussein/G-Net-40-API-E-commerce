using AutoMapper;
using E_commerce.Application.Common;
using E_commerce.Application.Contracts;
using E_commerce.Application.DTOS.Baskets;
using E_commerce.Domain.Contracts;
using E_commerce.Domain.Entities.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Services
{
    public class BasketService(IBasketRepository basketRepository, IMapper mapper) : IBasketService
    {
        public async Task<Result<BasketDto>> CreateOrUpdateBasketAsync(BasketDto basket, CancellationToken ct = default)
        {
            var customerBaskert=mapper.Map<CustomerBasket>(basket);
            var basketResult = await basketRepository.CreateOrUpdateBasketAsync(customerBaskert, ct:ct);

            return basketResult!=null ? Result<BasketDto>.Ok(mapper.Map<BasketDto>(basketResult)) : Result<BasketDto>.Fail(Error.Failure("Can Not Create or Update Basket"));
        }

        public async Task<Result<bool>> DeleteBasketAsync(string Id, CancellationToken ct = default)
        {
            var result = await basketRepository.DeleteBasketAsync(Id, ct);
            return result ? Result<bool>.Ok(true) : Result<bool>.Fail(Error.Failure("Can Not Delete Basket"));
        }

        public async Task<Result<BasketDto>> GetBasketAsync(string Id, CancellationToken ct = default)
        {
           var basket=await basketRepository.GetBasketAsync(Id, ct);
            if(basket == null)
            {
                return Result<BasketDto>.Fail(Error.NotFound("Basket Not Found"));
            }
            return mapper.Map<BasketDto>(basket) ;
        }
    }
}
