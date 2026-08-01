using E_commerce.Application.Common;
using E_commerce.Application.DTOS.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Contracts
{
    public interface IPaymentService
    {
        Task<Result<BasketDto>>CreateOrUpdatePaymentIntentAsync(string basketId,CancellationToken ct=default);
        Task PaymentSucceeded(string paymentIntentId);
        Task PaymentFailed(string paymentIntentId);
        
        
    }
}
