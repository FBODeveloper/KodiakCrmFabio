using System.Data;
using KodiakCrm.Core.Interfaces;
using Npgsql;

namespace KodiakCrm.Infrastructure.Data;

public class DatabaseConnection : IDatabaseConnection
{
    private readonly string _connectionString;

    public DatabaseConnection(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
