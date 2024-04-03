using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using BloomersWorkersCore.Domain.Entities;
using Dapper;

namespace BloomersWorkersCore.Infrastructure.Repositorys
{
    public class BloomersWorkersCoreRepository : IBloomersWorkersCoreRepository
    {
        private readonly ISQLServerConnection _conn;

        public BloomersWorkersCoreRepository (ISQLServerConnection conn) =>
            (_conn) = (conn);

        public async Task<MicrovixUser> GetMicrovixUser(string gabot)
        {
            try
            {
                var sql = $@"SELECT 
                         usuario, senha FROM GENERAL..VoloInvoiceUsuariosMicrovix
                         WHERE gabot like '%{gabot}%'";

                using (var conn = _conn.GetIDbConnection())
                {
                    var result = await conn.QueryAsync<MicrovixUser>(sql);
                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"GetMicrovixUser - Erro ao obter usuario para o bot na tabela GENERAL..VoloInvoiceUsuariosMicrovix - {ex.Message}");
            }
        }
    }
}
