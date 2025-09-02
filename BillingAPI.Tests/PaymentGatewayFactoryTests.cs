using BillingAPI.Core.Interfaces;
using BillingAPI.Infrastructure.Payments;
using FluentAssertions;
using Moq;

namespace BillingAPI.Tests
{
    public class PaymentGatewayFactoryTests
    {
        [Fact]
        public void Get_WithExistingGatewayId_ShouldReturnGateway()
        {
            // Arrange
            var paypal = new Mock<IPaymentGateway>();
            paypal.SetupGet(x => x.Id).Returns("paypal");
            var stripe = new Mock<IPaymentGateway>();
            stripe.SetupGet(x => x.Id).Returns("stripe");

            var factory = new PaymentGatewayFactory(new[] { paypal.Object, stripe.Object });

            // Act
            var result = factory.Get("paypal");

            // Assert
            result.Should().BeSameAs(paypal.Object);
        }

        [Fact]
        public void Get_WithUnknownGatewayId_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var factory = new PaymentGatewayFactory(Array.Empty<IPaymentGateway>());

            // Act
            Action act = () => factory.Get("unknown");

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
    }
}
