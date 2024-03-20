using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BloomersGeneralConnection.Infrastructure.SQLServer.Connection;

public class SQLServerConnection : ISQLServerConnection
{
    private readonly string _connectionString;
    private SqlConnection _sqlConnection;
    private IDbConnection _connection;

    public SQLServerConnection(IDbConnection connection, IConfiguration configuration) => 
        (_connection, _connectionString) = (connection, configuration.GetConnectionString("Connection"));

    public IDbConnection GetIDbConnection()
    {
        _connection = new SqlConnection(_connectionString);
        _connection.Open();
        return _connection;
    }

    public SqlConnection GetSqlConnection()
    {
        _sqlConnection = new SqlConnection(_connectionString);
        _sqlConnection.Open();
        return _sqlConnection;
    }
    
    public void Dispose() => _connection?.Dispose();

    public void CloseSqlConnection() => _sqlConnection?.Close();
}
