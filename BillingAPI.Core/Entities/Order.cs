namespace BillingAPI.Core.Entities
{
    public enum OrderStatus { Pending = 0, Paid = 1, Failed = 2 }

    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string OrderNumber { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public decimal Amount { get; set; }
        public string GatewayId { get; set; } = default!;
        public string? Description { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
