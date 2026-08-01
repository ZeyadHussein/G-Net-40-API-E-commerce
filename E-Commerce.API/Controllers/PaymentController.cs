using E_commerce.Application.Common;
using E_commerce.Application.Contracts;
using E_commerce.Application.DTOS.Baskets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace E_Commerce.API.Controllers
{
    public class PaymentController : APIBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly PaymentGetwaySettings _stripeSettings;

        public PaymentController(
            IPaymentService paymentService,
            IOptions<PaymentGetwaySettings> options)
        {
            _paymentService = paymentService;
            _stripeSettings = options.Value;
        }

        [Authorize]
        [HttpPost("{basketId}")]
        [ProducesResponseType(typeof(BasketDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent(
            string basketId,
            CancellationToken ct = default)
        {
            return ToActionResult(
                await _paymentService.CreateOrUpdatePaymentIntentAsync(basketId, ct));
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();

            var stripeSignature = Request.Headers["Stripe-Signature"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(stripeSignature))
            {
                return BadRequest("Stripe-Signature header is missing.");
            }

            if (string.IsNullOrWhiteSpace(_stripeSettings.WebhookSecret))
            {
                return BadRequest("WebhookSecret is missing from appsettings.json.");
            }

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    stripeSignature,
                    _stripeSettings.WebhookSecret);

                switch (stripeEvent.Type)
                {
                    case EventTypes.PaymentIntentSucceeded:

                        var succeededIntent =
                            stripeEvent.Data.Object as PaymentIntent;

                        if (succeededIntent != null)
                        {
                            await _paymentService.PaymentSucceeded(succeededIntent.Id);
                        }

                        break;

                    case EventTypes.PaymentIntentPaymentFailed:

                        var failedIntent =
                            stripeEvent.Data.Object as PaymentIntent;

                        if (failedIntent != null)
                        {
                            await _paymentService.PaymentFailed(failedIntent.Id);
                        }

                        break;
                }

                return Ok();
            }
            catch (StripeException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}