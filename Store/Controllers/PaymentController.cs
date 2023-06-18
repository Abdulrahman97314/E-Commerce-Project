using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Store.APIs.Dtos;
using Store.APIs.Errors;
using Store.Core.Entities.Order;
using Store.Core.Services;
using Stripe;

namespace Store.APIs.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly IPaymentService paymentService;
        private readonly ILogger<PaymentController> logger;
        private readonly string WhSecret = "whsec_d9de67d530bbbe119fd557c4171bc06324ab28bef9c1ed6e8f70c9f9a1dd5bdd";

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            this.paymentService = paymentService;
            this.logger = logger;
            this.logger = logger;
        }
        [ProducesResponseType(typeof(CustomerBasketDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),400)]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string BasketId)
        {
            var basket =await paymentService.CreateOrUpdatePaymentIntent(BasketId);
            if (basket is null) return BadRequest(new ApiResponse(400,"Problem With Your Basket"));
            return Ok(basket);
        }
        [HttpPost("WebHook")]
        public async Task<IActionResult> StripWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], WhSecret);
                PaymentIntent paymentIntent;
                Order order;
                switch (stripeEvent.Type)
                {
                    case Events.PaymentIntentSucceeded:
                        paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                        logger.LogInformation("Payment Succeeded : ", paymentIntent.Id);
                        order = await paymentService.UpdateOrderPaymentSucceded(paymentIntent.Id);
                        logger.LogInformation("order updated to payment recived : ", order.Id);
                        break;
                    case Events.PaymentIntentPaymentFailed:
                        paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                        logger.LogInformation("Payment Failed : ", paymentIntent.Id);
                        order = await paymentService.UpdateOrderPaymentFailed(paymentIntent.Id);
                        logger.LogInformation("Payment Failed : ", order.Id);
                        break;
                }

            return Ok();
        }
    }
}
