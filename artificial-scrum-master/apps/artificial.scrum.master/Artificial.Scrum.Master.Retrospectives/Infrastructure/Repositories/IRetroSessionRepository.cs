using Artificial.Scrum.Master.Retrospectives.Infrastructure.Models;

namespace Artificial.Scrum.Master.Retrospectives.Infrastructure.Repositories;

public interface IRetroSessionRepository
{
    Task CreateSession(RetroSession session);
    Task<RetroSession?> GetSession(int sprintId);
    Task<RetroSession?> GetSession(string sessionId);
    Task<bool> SessionExists(string sessionId);

    Task CreateSessionCard(SessionCard sessionCard);
    Task<List<SessionCard>> GetSessionCards(string sessionId);
}
