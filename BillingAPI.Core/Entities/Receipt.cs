namespace BillingAPI.Core.Entities
{
    public class Receipt
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; }
        public string UserId { get; set; } = default!;
        public decimal Amount { get; set; }
        public string GatewayId { get; set; } = default!;
        public string TransactionId { get; set; } = default!;
        public bool Success { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
