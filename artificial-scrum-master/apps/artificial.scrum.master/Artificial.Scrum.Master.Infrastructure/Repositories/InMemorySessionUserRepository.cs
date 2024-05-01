using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;
using System.Collections.Concurrent;

namespace Artificial.Scrum.Master.Infrastructure.Repositories;

internal class InMemorySessionUserRepository : IActiveUserRepository
{
    private readonly ConcurrentDictionary<string, ConcurrentBag<string>> _users = [];

    public HashSet<string> GetUsers(string sessionId)
    {
        if (_users.TryGetValue(sessionId, out var users))
        {
            return users.ToHashSet();
        }

        return [];
    }

    public bool RegisterUser(string sessionId, string username)
    {
        if (!_users.TryGetValue(sessionId, out var users))
        {
            var newBag = new ConcurrentBag<string>();
            users = newBag;
            _users[sessionId] = users;
        }

        if (users.Any(x => x == username))
        {
            return false;
        }

        users.Add(username);
        return true;
    }
}
