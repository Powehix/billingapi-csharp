using BillingAPI.Core.Entities;
using BillingAPI.Infrastructure.Repositories;
using FluentAssertions;

namespace BillingAPI.Tests
{
    public class InMemoryOrderRepositoryTests
    {
        [Fact]
        public async Task AddAsync_WhenOrderIsAdded_ShouldBeRetrievableById()
        {
            // Arrange
            var repo = new InMemoryOrderRepository();
            var order = new Order { OrderNumber = "ORD-1001", UserId = "u1", Amount = 10, GatewayId = "paypal" };

            // Act
            await repo.AddAsync(order);
            var fetched = await repo.GetByIdAsync(order.Id);

            // Assert
            fetched.Should().NotBeNull();
            fetched!.OrderNumber.Should().Be("ORD-1001");
        }
    }
}
