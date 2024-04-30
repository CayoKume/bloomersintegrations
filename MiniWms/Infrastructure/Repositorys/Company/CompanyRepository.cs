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
                         empresa as cod_company,
                         cnpj_emp as doc_company,
                         razao_emp as reason_company,
                         nome_emp as name_company,
                         email_emp as email_company,
                         endereco_emp as address_company,
                         num_emp as street_number_company,
                         complement_emp as complement_address_company,
                         bairro_emp as neighborhood_company,
                         cidade_emp as city_company,
                         estado_emp as uf_company,
                         cep_emp as zip_code_company,
                         fone_emp as fone_company,
                         inscricao_emp as state_registration_company,
                         inscricao_municipal_emp as municipal_registration_company
                         FROM BLOOMERS_LINX..LinxLojas_trusted
                         WHERE empresa != 0
                         ORDER BY empresa asc";

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
