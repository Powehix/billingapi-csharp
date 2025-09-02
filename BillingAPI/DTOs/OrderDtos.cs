using System.ComponentModel.DataAnnotations;

namespace BillingAPI.DTOs
{
    public class CreateOrderRequest
    {
        [Required] public string OrderNumber { get; set; } = default!;
        [Required, Range(0.01, double.MaxValue)] public decimal Amount { get; set; }
        [Required] public string Gateway { get; set; } = default!;
        public string? Description { get; set; }
    }

    public class ReceiptDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public string GatewayId { get; set; } = default!;
        public string TransactionId { get; set; } = default!;
        public bool Success { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedUtc { get; set; }
    }
}
