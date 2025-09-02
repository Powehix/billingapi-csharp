using BillingAPI.Core.Entities;

namespace BillingAPI.Core.Interfaces
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task<Order?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<Order>> GetByUserAsync(string userId);
    }

    public interface IReceiptRepository
    {
        Task AddAsync(Receipt receipt);
        Task<IReadOnlyList<Receipt>> GetByUserAsync(string userId);
        Task<IReadOnlyList<Receipt>> GetByOrdersAsync(IEnumerable<Guid> orderIds);
    }

    public interface IUserRepository
    {
        Task<bool> ValidateCredentialsAsync(string username, string password);
        Task<string?> GetUserIdByUsernameAsync(string username);
    }

    public interface IPaymentGatewayFactory
    {
        IPaymentGateway Get(string gatewayId);
    }
}
