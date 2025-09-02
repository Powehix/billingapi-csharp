using BillingAPI.Core.Entities;
using BillingAPI.Core.Interfaces;
using BillingAPI.Core.Payments;

namespace BillingAPI.Infrastructure.Payments
{
    public class PayPalGateway : IPaymentGateway
    {
        public string Id => "paypal";

        public async Task<PaymentResult> ProcessAsync(Order order)
        {
            await Task.Delay(100); // simulate network
            return new PaymentResult(true, $"PP-{Guid.NewGuid():N}", "Processed by PayPal (mock)");
        }
    }
}
