namespace BillingAPI.Core.Payments
{
    public record PaymentResult(bool Success, string TransactionId, string? Message = null);
}
