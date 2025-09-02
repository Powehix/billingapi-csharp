using BillingAPI.Core.Entities;
using BillingAPI.Core.Interfaces;
using BillingAPI.Core.Payments;
using BillingAPI.Core.Services;
using FluentAssertions;
using Moq;

namespace BillingAPI.Tests
{
    public class BillingServiceTests
    {
        [Fact]
        public async Task ProcessOrder_WithSuccessfulGateway_ShouldMarkOrderAsPaidAndReturnSuccessReceipt()
        {
            // Arrange
            var orderRepo = new Mock<IOrderRepository>();
            orderRepo.Setup(r => r.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);
            orderRepo.Setup(r => r.UpdateAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

            var receiptRepo = new Mock<IReceiptRepository>();
            receiptRepo.Setup(r => r.AddAsync(It.IsAny<Receipt>())).Returns(Task.CompletedTask);

            var gateway = new Mock<IPaymentGateway>();
            gateway.SetupGet(g => g.Id).Returns("paypal");
            gateway.Setup(g => g.ProcessAsync(It.IsAny<Order>()))
                .ReturnsAsync(new PaymentResult(true, "tx-123"));

            var factory = new Mock<IPaymentGatewayFactory>();
            factory.Setup(f => f.Get("paypal")).Returns(gateway.Object);

            var svc = new BillingService(orderRepo.Object, receiptRepo.Object, factory.Object);
            var order = new Order { OrderNumber = "1", UserId = "u1", Amount = 9.99m, GatewayId = "paypal" };

            // Act
            var receipt = await svc.ProcessOrderAsync(order);

            // Assert
            receipt.Success.Should().BeTrue();
            receipt.TransactionId.Should().Be("tx-123");
            order.Status.Should().Be(OrderStatus.Paid);
        }

        [Fact]
        public async Task ProcessOrder_WithFailingGateway_ShouldMarkOrderAsFailedAndReturnFailureReceipt()
        {
            // Arrange
            var orderRepo = new Mock<IOrderRepository>();
            orderRepo.Setup(r => r.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);
            orderRepo.Setup(r => r.UpdateAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

            var receiptRepo = new Mock<IReceiptRepository>();
            receiptRepo.Setup(r => r.AddAsync(It.IsAny<Receipt>())).Returns(Task.CompletedTask);

            var gateway = new Mock<IPaymentGateway>();
            gateway.SetupGet(g => g.Id).Returns("stripe");
            gateway.Setup(g => g.ProcessAsync(It.IsAny<Order>()))
                .ReturnsAsync(new PaymentResult(false, "tx-fail", "declined"));

            var factory = new Mock<IPaymentGatewayFactory>();
            factory.Setup(f => f.Get("stripe")).Returns(gateway.Object);

            var svc = new BillingService(orderRepo.Object, receiptRepo.Object, factory.Object);
            var order = new Order { OrderNumber = "1", UserId = "u1", Amount = 9.99m, GatewayId = "stripe" };

            // Act
            var receipt = await svc.ProcessOrderAsync(order);

            // Assert
            receipt.Success.Should().BeFalse();
            receipt.Message.Should().Be("declined");
            order.Status.Should().Be(OrderStatus.Failed);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public async Task ProcessOrder_WithInvalidAmount_ShouldThrowArgumentException(decimal amount)
        {
            // Arrange
            var svc = new BillingService(Mock.Of<IOrderRepository>(), Mock.Of<IReceiptRepository>(), Mock.Of<IPaymentGatewayFactory>());
            var order = new Order { OrderNumber = "1", UserId = "u1", Amount = amount, GatewayId = "paypal" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => svc.ProcessOrderAsync(order));
        }

        [Fact]
        public async Task ProcessOrder_WithMissingOrderNumber_ShouldThrowArgumentException()
        {
            // Arrange
            var svc = new BillingService(Mock.Of<IOrderRepository>(), Mock.Of<IReceiptRepository>(), Mock.Of<IPaymentGatewayFactory>());
            var order = new Order { OrderNumber = "", UserId = "u1", Amount = 5, GatewayId = "paypal" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => svc.ProcessOrderAsync(order));
        }

        [Fact]
        public async Task ProcessOrder_WithMissingUserId_ShouldThrowArgumentException()
        {
            // Arrange
            var svc = new BillingService(Mock.Of<IOrderRepository>(), Mock.Of<IReceiptRepository>(), Mock.Of<IPaymentGatewayFactory>());
            var order = new Order { OrderNumber = "1", UserId = "", Amount = 5, GatewayId = "paypal" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => svc.ProcessOrderAsync(order));
        }

        [Fact]
        public async Task ProcessOrder_WithMissingGatewayId_ShouldThrowArgumentException()
        {
            // Arrange
            var svc = new BillingService(Mock.Of<IOrderRepository>(), Mock.Of<IReceiptRepository>(), Mock.Of<IPaymentGatewayFactory>());
            var order = new Order { OrderNumber = "1", UserId = "u1", Amount = 5, GatewayId = "" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => svc.ProcessOrderAsync(order));
        }
    }
}
