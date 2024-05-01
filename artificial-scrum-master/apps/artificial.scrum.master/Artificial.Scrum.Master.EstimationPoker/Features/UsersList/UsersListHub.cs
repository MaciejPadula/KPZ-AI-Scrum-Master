using Artificial.Scrum.Master.EstimationPoker.Infrastructure.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace Artificial.Scrum.Master.EstimationPoker.Features.UsersList;

internal class UsersListHub : Hub
{
    private readonly IActiveUserRepository _activeUserRepository;

    public UsersListHub(IActiveUserRepository activeUserRepository)
    {
        _activeUserRepository = activeUserRepository;
    }

    public async Task RegisterUser(string sessionId, string name)
    {
        if (_activeUserRepository.RegisterUser(sessionId, name))
        {
            await Clients.Group(GetSessionAdmin(sessionId)).SendAsync("GetActiveUsers");
        }
    }

    public async Task JoinAdmins(string sessionId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, GetSessionAdmin(sessionId));
    }

    public HashSet<string> GetActiveUsers(string sessionId) =>
        _activeUserRepository.GetUsers(sessionId);

    private static string GetSessionAdmin(string sessionId) => $"Admin_{sessionId}";
}
