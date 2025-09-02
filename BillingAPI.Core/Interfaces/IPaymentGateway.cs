using BillingAPI.Core.Entities;
using BillingAPI.Core.Payments;

namespace BillingAPI.Core.Interfaces
{
    public interface IPaymentGateway
    {
        string Id { get; }
        Task<PaymentResult> ProcessAsync(Order order);
    }
}
