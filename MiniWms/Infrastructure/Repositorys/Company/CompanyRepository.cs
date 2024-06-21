using BloomersIntegrationsCore.Domain.Entities;
using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using Dapper;

namespace BloomersMiniWmsIntegrations.Infrastructure.Repositorys
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ISQLServerConnection _conn;

        public CompanyRepository(ISQLServerConnection conn) =>
            (_conn) = (conn);

        public async Task<IEnumerable<Company>?> GetCompanys()
        {
            var sql = $@"SELECT 
                         EMPRESA AS COD_COMPANY,
                         CNPJ_EMP AS DOC_COMPANY,
                         RAZAO_EMP AS REASON_COMPANY,
                         NOME_EMP AS NAME_COMPANY,
                         EMAIL_EMP AS EMAIL_COMPANY,
                         ENDERECO_EMP AS ADDRESS_COMPANY,
                         NUM_EMP AS STREET_NUMBER_COMPANY,
                         COMPLEMENT_EMP AS COMPLEMENT_ADDRESS_COMPANY,
                         BAIRRO_EMP AS NEIGHBORHOOD_COMPANY,
                         CIDADE_EMP AS CITY_COMPANY,
                         ESTADO_EMP AS UF_COMPANY,
                         CEP_EMP AS ZIP_CODE_COMPANY,
                         FONE_EMP AS FONE_COMPANY,
                         INSCRICAO_EMP AS STATE_REGISTRATION_COMPANY,
                         INSCRICAO_MUNICIPAL_EMP AS MUNICIPAL_REGISTRATION_COMPANY
                         FROM BLOOMERS_LINX..LINXLOJAS_TRUSTED (NOLOCK)
                         WHERE EMPRESA != 0
                         ORDER BY EMPRESA ASC";

            try
            {
                return await _conn.GetDbConnection().QueryAsync<Company>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [Companys] - GetCompanys - Erro ao obter empresa da tabela LinxLojas_trusted  - {ex.Message}");
            }
        }
    }
}
