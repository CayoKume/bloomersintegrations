using System.Data;
using System.Data.SqlClient;

namespace BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;

public interface ISQLServerConnection : IDisposable
{
    public IDbConnection GetIDbConnection();
}
