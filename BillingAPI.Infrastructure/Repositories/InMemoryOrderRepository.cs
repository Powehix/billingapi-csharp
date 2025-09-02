using BillingAPI.Core.Entities;
using BillingAPI.Core.Interfaces;
using System.Collections.Concurrent;

namespace BillingAPI.Infrastructure.Repositories
{
    public class InMemoryOrderRepository : IOrderRepository
    {
        private readonly ConcurrentDictionary<Guid, Order> _store = new();
        public Task AddAsync(Order order)
        {
            _store[order.Id] = order;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Order order)
        {
            _store[order.Id] = order;
            return Task.CompletedTask;
        }

        public Task<Order?> GetByIdAsync(Guid id)
        {
            _store.TryGetValue(id, out var order);
            return Task.FromResult(order);
        }

        public Task<IReadOnlyList<Order>> GetByUserAsync(string userId)
        {
            var list = _store.Values.Where(o => o.UserId == userId).OrderByDescending(o => o.CreatedUtc).ToList();
            return Task.FromResult((IReadOnlyList<Order>)list);
        }
    }
}
