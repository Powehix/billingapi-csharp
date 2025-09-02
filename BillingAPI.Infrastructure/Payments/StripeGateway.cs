using BillingAPI.Core.Entities;
using BillingAPI.Core.Interfaces;
using BillingAPI.Core.Payments;

namespace BillingAPI.Infrastructure.Payments
{
    public class StripeGateway : IPaymentGateway
    {
        public string Id => "stripe";

        public async Task<PaymentResult> ProcessAsync(Order order)
        {
            await Task.Delay(100); // simulate network
            return new PaymentResult(false, $"ST-{Guid.NewGuid():N}", "Payment declined by Stripe (mock)");
        }
    }
}
