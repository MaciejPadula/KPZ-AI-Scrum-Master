using Artificial.Scrum.Master.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Artificial.Scrum.Master.Infrastructure;

internal class SqlDbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqlDbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection GetOpenConnection()
    {
        var connection = new SqlConnection(_connectionString);
        connection.Open();
        return connection;
    }

    public async Task<IDbConnection> GetOpenConnectionAsync()
    {
        var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}
