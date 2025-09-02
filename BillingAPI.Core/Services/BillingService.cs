using BillingAPI.Core.Entities;
using BillingAPI.Core.Interfaces;

namespace BillingAPI.Core.Services
{
    public class BillingService
    {
        private readonly IOrderRepository _orders;
        private readonly IReceiptRepository _receipts;
        private readonly IPaymentGatewayFactory _gatewayFactory;

        public BillingService(IOrderRepository orders, IReceiptRepository receipts, IPaymentGatewayFactory gatewayFactory)
        {
            _orders = orders;
            _receipts = receipts;
            _gatewayFactory = gatewayFactory;
        }

        public async Task<Receipt> ProcessOrderAsync(Order order)
        {
            if (order.Amount <= 0) throw new ArgumentException("Amount must be positive", nameof(order.Amount));
            if (string.IsNullOrWhiteSpace(order.OrderNumber)) throw new ArgumentException("OrderNumber is required", nameof(order.OrderNumber));
            if (string.IsNullOrWhiteSpace(order.UserId)) throw new ArgumentException("UserId is required", nameof(order.UserId));
            if (string.IsNullOrWhiteSpace(order.GatewayId)) throw new ArgumentException("GatewayId is required", nameof(order.GatewayId));

            await _orders.AddAsync(order);

            var gateway = _gatewayFactory.Get(order.GatewayId);
            var result = await gateway.ProcessAsync(order);

            order.Status = result.Success ? OrderStatus.Paid : OrderStatus.Failed;
            await _orders.UpdateAsync(order);

            var receipt = new Receipt
            {
                OrderId = order.Id,
                UserId = order.UserId,
                Amount = order.Amount,
                GatewayId = order.GatewayId,
                TransactionId = result.TransactionId,
                Success = result.Success,
                Message = result.Message
            };
            await _receipts.AddAsync(receipt);

            return receipt;
        }
    }
}
