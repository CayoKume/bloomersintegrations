using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;
using BloomersMicrovixIntegrations.LinxMicrovix.Infrastructure.Repositorys.Base;
using BloomersIntegrationsCore.Domain.Entities;

namespace BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxMicrovix
{
    public class LinxMovimentoCartoesRepository : ILinxMovimentoCartoesRepository
    {
        private readonly ILinxMicrovixRepositoryBase<LinxMovimentoCartoes> _linxMicrovixRepositoryBase;

        public LinxMovimentoCartoesRepository(ILinxMicrovixRepositoryBase<LinxMovimentoCartoes> linxMicrovixRepositoryBase) =>
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;

        public void BulkInsertIntoTableRaw(List<LinxMovimentoCartoes> registros, string tableName, string database)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateDataTable(tableName, new LinxMovimentoCartoes().GetType().GetProperties());

                for (int i = 0; i < registros.Count(); i++)
                {
                    table.Rows.Add(registros[i].lastupdateon, registros[i].portal, registros[i].cnpj_emp, registros[i].codlojasitef, registros[i].data_lancamento, registros[i].identificador, registros[i].cupomfiscal, registros[i].credito_debito, registros[i].id_cartao_bandeira, registros[i].descricao_bandeira, registros[i].valor,
                                   registros[i].ordem_cartao, registros[i].nsu_host, registros[i].nsu_sitef, registros[i].cod_autorizacao, registros[i].id_antecipacoes_financeiras, registros[i].transacao_servico_terceiro, registros[i].texto_comprovante, registros[i].id_maquineta_pos, registros[i].descricao_maquineta,
                                   registros[i].serie_maquineta, registros[i].timestamp);
                }

                _linxMicrovixRepositoryBase.BulkInsertIntoTableRaw(table, database, tableName, table.Rows.Count);
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> GetParametersAsync(string tableName, string database, string parameterCol)
        {
            string sql = $@"SELECT {parameterCol} FROM [BLOOMERS_LINX].[dbo].[LinxAPIParam] (nolock) where method = '{tableName}'";

            try
            {
                return await _linxMicrovixRepositoryBase.GetParametersAsync(tableName, sql);
            }
            catch
            {
                throw;
            }
        }

        public string GetParametersNotAsync(string tableName, string database, string parameterCol)
        {
            string sql = $@"SELECT {parameterCol} FROM [BLOOMERS_LINX].[dbo].[LinxAPIParam] (nolock) where method = '{tableName}'";

            try
            {
                return _linxMicrovixRepositoryBase.GetParametersNotAsync(tableName, sql);
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<Company>> GetCompanysAsync(string tableName, string database)
        {
            string sql = $@"SELECT empresa as numero_erp_empresa, nome_emp as nome_empresa, cnpj_emp as doc_empresa FROM BLOOMERS_LINX..LinxLojas_trusted WHERE nome_emp LIKE '%MISHA%' or nome_emp LIKE '%OPEN%'";

            try
            {
                return await _linxMicrovixRepositoryBase.GetCompanysAsync(tableName, sql);
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<Company> GetCompanysNotAsync(string tableName, string database)
        {
            string sql = $@"SELECT empresa as numero_erp_empresa, nome_emp as nome_empresa, cnpj_emp as doc_empresa FROM BLOOMERS_LINX..LinxLojas_trusted WHERE nome_emp LIKE '%MISHA%' or nome_emp LIKE '%OPEN%'";

            try
            {
                return _linxMicrovixRepositoryBase.GetCompanysNotAsync(tableName, sql);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<LinxMovimentoCartoes>> GetRegistersExistsAsync(List<LinxMovimentoCartoes> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].cnpj_emp}'";
                else
                    identificadores += $"'{registros[i].cnpj_emp}', ";
            }
            string query = $"SELECT cupomfiscal, TIMESTAMP FROM BLOOMERS_LINX..LinxMovimentoCartoes_trusted WHERE cupomfiscal IN ({identificadores})";

            try
            {
                return await _linxMicrovixRepositoryBase.GetRegistersExistsAsync(tableName, query);
            }
            catch
            {
                throw;
            }
        }

        public List<LinxMovimentoCartoes> GetRegistersExistsNotAsync(List<LinxMovimentoCartoes> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].cnpj_emp}'";
                else
                    identificadores += $"'{registros[i].cnpj_emp}', ";
            }
            string query = $"SELECT cupomfiscal, TIMESTAMP FROM BLOOMERS_LINX..LinxMovimentoCartoes_trusted WHERE cupomfiscal IN ({identificadores})";

            try
            {
                return _linxMicrovixRepositoryBase.GetRegistersExistsNotAsync(tableName, query);
            }
            catch
            {
                throw;
            }
        }
    }
}
