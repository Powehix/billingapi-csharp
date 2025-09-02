using BillingAPI.Core.Entities;
using BillingAPI.Core.Interfaces;
using System.Collections.Concurrent;

namespace BillingAPI.Infrastructure.Repositories
{
    public class InMemoryReceiptRepository : IReceiptRepository
    {
        private readonly ConcurrentDictionary<Guid, Receipt> _store = new();

        public Task AddAsync(Receipt receipt)
        {
            _store[receipt.Id] = receipt;
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<Receipt>> GetByUserAsync(string userId)
        {
            var list = _store.Values.Where(r => r.UserId == userId).OrderByDescending(r => r.CreatedUtc).ToList();
            return Task.FromResult((IReadOnlyList<Receipt>)list);
        }

        public Task<IReadOnlyList<Receipt>> GetByOrdersAsync(IEnumerable<Guid> orderIds)
        {
            var set = orderIds.ToHashSet();
            var list = _store.Values.Where(r => set.Contains(r.OrderId)).OrderByDescending(r => r.CreatedUtc).ToList();
            return Task.FromResult((IReadOnlyList<Receipt>)list);
        }
    }
}
