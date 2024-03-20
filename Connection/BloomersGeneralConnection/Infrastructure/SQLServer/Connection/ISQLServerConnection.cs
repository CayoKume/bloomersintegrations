using System.Data;
using System.Data.SqlClient;

namespace BloomersGeneralConnection.Infrastructure.SQLServer.Connection;

public interface ISQLServerConnection : IDisposable
{
    public IDbConnection GetIDbConnection();
    public SqlConnection GetSqlConnection();
    public void CloseSqlConnection();
}
