using BillingAPI.Core.Entities;
using BillingAPI.Infrastructure.Repositories;
using FluentAssertions;

namespace BillingAPI.Tests
{
    public class InMemoryReceiptRepositoryTests
    {
        [Fact]
        public async Task AddAsync_WhenReceiptIsAdded_ShouldBeRetrievableByUser()
        {
            // Arrange
            var repo = new InMemoryReceiptRepository();
            var receipt = new Receipt { OrderId = Guid.NewGuid(), UserId = "u1", Amount = 10, GatewayId = "paypal", TransactionId = "tx" };

            // Act
            await repo.AddAsync(receipt);
            var list = await repo.GetByUserAsync("u1");

            // Assert
            list.Should().ContainSingle(r => r.TransactionId == "tx");
        }
    }
}
