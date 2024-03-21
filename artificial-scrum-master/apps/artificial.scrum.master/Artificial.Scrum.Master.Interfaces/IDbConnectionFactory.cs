using System.Data;

namespace Artificial.Scrum.Master.Interfaces;

public interface IDbConnectionFactory
{
  IDbConnection GetOpenConnection();
  Task<IDbConnection> GetOpenConnectionAsync();
}
