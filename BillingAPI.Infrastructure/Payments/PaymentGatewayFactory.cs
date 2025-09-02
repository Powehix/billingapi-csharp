using BillingAPI.Core.Interfaces;

namespace BillingAPI.Infrastructure.Payments
{
    public class PaymentGatewayFactory : IPaymentGatewayFactory
    {
        private readonly IDictionary<string, IPaymentGateway> _gateways;

        public PaymentGatewayFactory(IEnumerable<IPaymentGateway> gateways)
        {
            _gateways = new Dictionary<string, IPaymentGateway>(StringComparer.OrdinalIgnoreCase);
            foreach (var g in gateways)
                _gateways[g.Id] = g;
        }

        public IPaymentGateway Get(string gatewayId)
        {
            if (!_gateways.TryGetValue(gatewayId, out var gateway))
                throw new KeyNotFoundException($"Unknown payment gateway: '{gatewayId}'.");
            return gateway;
        }
    }
}
