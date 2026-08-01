using E_commerce.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Contracts
{
    public interface IPaymentGetway
    {
        Task<PaymentIntentResult>CreatePaymentIntentAsync(decimal amount, string currency,CancellationToken ct=default);
        Task<PaymentIntentResult> UpdatePaymentIntentAsync(string paymentIntentId, decimal amount, CancellationToken ct = default);

    }
}
