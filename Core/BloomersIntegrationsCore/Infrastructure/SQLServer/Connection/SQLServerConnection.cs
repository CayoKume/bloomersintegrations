using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;

public class SQLServerConnection : ISQLServerConnection
{
    private readonly string _connectionString;
    private SqlConnection _conn;
    private IDbConnection _connection;

    public SQLServerConnection(IDbConnection connection, SqlConnection conn)
        => (_connection, _conn) = (connection, conn);

    public SQLServerConnection(IConfiguration configuration)
        => _connectionString = configuration.GetConnectionString("Connection");

    public IDbConnection GetIDbConnection()
    {
        _connection = new SqlConnection(_connectionString);
        _connection.Open();
        return _connection;
    }
    
    public SqlConnection GetDbConnection()
    {
        _conn = new SqlConnection(_connectionString);
        _conn.Open();
        return _conn;
    }

    public void Dispose() => _connection?.Dispose();
}
