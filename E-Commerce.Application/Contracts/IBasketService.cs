using E_commerce.Application.Common;
using E_commerce.Application.DTOS.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Contracts
{
    public interface IBasketService
    {
        Task<Result<BasketDto>> GetBasketAsync(string Id,CancellationToken ct=default);
        Task<Result<BasketDto>> CreateOrUpdateBasketAsync(BasketDto basket,CancellationToken ct=default);
        Task<Result<bool>> DeleteBasketAsync(string Id,CancellationToken ct=default);
    }
}
