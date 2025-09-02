using BillingAPI.DTOs;
using BillingAPI.Core.Entities;
using BillingAPI.Core.Interfaces;
using BillingAPI.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BillingAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("orders")]
    public class OrdersController : ControllerBase
    {
        private readonly BillingService _billing;
        private readonly IOrderRepository _orders;
        private readonly IReceiptRepository _receipts;

        public OrdersController(BillingService billing, IOrderRepository orders, IReceiptRepository receipts)
        { _billing = billing; _orders = orders; _receipts = receipts; }

        [HttpPost]
        public async Task<ActionResult<ReceiptDto>> Create([FromBody] CreateOrderRequest req)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var order = new Order
            {
                OrderNumber = req.OrderNumber,
                Amount = req.Amount,
                GatewayId = req.Gateway,
                UserId = userId,
                Description = req.Description
            };

            var receipt = await _billing.ProcessOrderAsync(order);

            return Ok(new ReceiptDto
            {
                Id = receipt.Id,
                OrderId = receipt.OrderId,
                Amount = receipt.Amount,
                GatewayId = receipt.GatewayId,
                TransactionId = receipt.TransactionId,
                Success = receipt.Success,
                Message = receipt.Message,
                CreatedUtc = receipt.CreatedUtc
            });
        }

        [HttpGet("history")]
        public async Task<ActionResult> History()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var orders = await _orders.GetByUserAsync(userId);
            var receipts = await _receipts.GetByUserAsync(userId);

            return Ok(new
            {
                orders = orders.Select(o => new
                {
                    o.Id,
                    o.OrderNumber,
                    o.Amount,
                    o.GatewayId,
                    o.Status,
                    o.CreatedUtc,
                    o.Description
                }),
                receipts = receipts.Select(r => new
                {
                    r.Id,
                    r.OrderId,
                    r.Amount,
                    r.GatewayId,
                    r.TransactionId,
                    r.Success,
                    r.Message,
                    r.CreatedUtc
                })
            });
        }
    }
}
