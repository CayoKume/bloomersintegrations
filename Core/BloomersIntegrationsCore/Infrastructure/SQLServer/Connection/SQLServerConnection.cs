using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;

public class SQLServerConnection : ISQLServerConnection
{
    private readonly string _connectionString;
    private IDbConnection _connection;

    public SQLServerConnection(IDbConnection connection)
        => _connection = connection;

    public SQLServerConnection(IConfiguration configuration)
        => _connectionString = configuration.GetConnectionString("Connection");

    public IDbConnection GetIDbConnection()
    {
        _connection = new SqlConnection(_connectionString);
        _connection.Open();
        return _connection;
    }
    
    public void Dispose() => _connection?.Dispose();
}
