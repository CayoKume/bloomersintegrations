using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using BloomersWorkersCore.Domain.Entities;
using Dapper;

namespace BloomersWorkers.ChangingPassword.Infrastructure.Repositorys
{
    public class ChangingPasswordRepository : IChangingPasswordRepository
    {
        private readonly ISQLServerConnection _conn;

        public ChangingPasswordRepository(ISQLServerConnection conn) =>
            (_conn) = (conn);

        public async Task<List<MicrovixUser>> GetMicrovixUser()
        {
            var sql = @$"SELECT DISTINCT
                             USUARIO,
                             SENHA
                             FROM GENERAL.[dbo].VoloInvoiceUsuariosMicrovix
                             WHERE DATEDIFF(DAY, LASTUPDATEON, GETDATE()) > 20";
            try
            {
                var result = await _conn.GetIDbConnection().QueryAsync<MicrovixUser>(sql);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"GetUsuariosFromMicrovixUsers - {ex}");
            }
        }

        public async Task UpdateLastupdateonFromMicrovixUsers(MicrovixUser usuario)
        {
            var sql = @$"UPDATE GENERAL.[dbo].VoloInvoiceUsuariosMicrovix SET
                             lastupdateon = GETDATE(),
                             senha = '{usuario.senha}'
                             where
                             usuario = '{usuario.usuario}'";
            try
            {
                await _conn.GetIDbConnection().ExecuteAsync(sql);
            }
            catch (Exception ex)
            {
                throw new Exception($"GetUsuariosFromMicrovixUsers - {ex}");
            }

        }
    }
}
