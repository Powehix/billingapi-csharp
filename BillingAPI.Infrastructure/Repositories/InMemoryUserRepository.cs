using BillingAPI.Core.Interfaces;
using System.Collections.Concurrent;

namespace BillingAPI.Infrastructure.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly ConcurrentDictionary<string, string> _users = new(); // username -> password
        private readonly ConcurrentDictionary<string, string> _userIds = new(); // username -> userId

        public InMemoryUserRepository()
        {
            // Seed demo user
            _users["test"] = "1234";
            _userIds["test"] = "1";
        }

        public Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            return Task.FromResult(_users.TryGetValue(username, out var stored) && stored == password);
        }

        public Task<string?> GetUserIdByUsernameAsync(string username)
        {
            _userIds.TryGetValue(username, out var id);
            return Task.FromResult(id);
        }
    }
}
