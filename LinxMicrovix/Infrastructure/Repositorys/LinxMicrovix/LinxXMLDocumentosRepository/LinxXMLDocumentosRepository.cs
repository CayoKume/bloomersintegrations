using BloomersMicrovixIntegrations.LinxMicrovix.Infrastructure.Repositorys.Base;
using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;
using BloomersIntegrationsCore.Domain.Entities;

namespace BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxMicrovix
{
    public class LinxXMLDocumentosRepository : ILinxXMLDocumentosRepository
    {
        private readonly ILinxMicrovixRepositoryBase<LinxXMLDocumentos> _linxMicrovixRepositoryBase;

        public LinxXMLDocumentosRepository(ILinxMicrovixRepositoryBase<LinxXMLDocumentos> linxMicrovixRepositoryBase) =>
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;

        public void BulkInsertIntoTableRaw(List<LinxXMLDocumentos> registros, string tableName, string database)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateDataTable(tableName, new LinxXMLDocumentos().GetType().GetProperties());

                for (int i = 0; i < registros.Count(); i++)
                {
                    table.Rows.Add(registros[i].lastupdateon, registros[i].portal, registros[i].cnpj_emp, registros[i].documento, registros[i].serie, registros[i].data_emissao, registros[i].chave_nfe, registros[i].situacao, registros[i].xml, registros[i].excluido, registros[i].identificador_microvix,
                                   registros[i].dt_insert, registros[i].timestamp, registros[i].nProtCanc, registros[i].nProtInut, registros[i].xmlDistribuicao, registros[i].nProtDeneg, registros[i].cStat, registros[i].id_nfe, registros[i].justificativa);
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

        public async Task InsereRegistroIndividualAsync(LinxXMLDocumentos registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [portal], [cnpj_emp], [documento], [serie], [data_emissao], [chave_nfe], [situacao], [xml], [excluido], [identificador_microvix],
                             [dt_insert], [timestamp], [nProtCanc], [nProtInut], [xmlDistribuicao], [nProtDeneg], [cStat], [id_nfe], [justificativa]
                            ) 
                            Values 
                            (@lastupdateon, @portal, @cnpj_emp, @documento, @serie, @data_emissao, @chave_nfe, @situacao, @xml, @excluido, @identificador_microvix,
                             @dt_insert, @timestamp, @nProtCanc, @nProtInut, @xmlDistribuicao, @nProtDeneg, @cStat, @id_nfe, @justificativa
                            )";

            try
            {
                await _linxMicrovixRepositoryBase.InsereRegistroIndividualAsync(tableName, sql, registro);
            }
            catch
            {
                throw;
            }
        }

        public void InsereRegistroIndividualNotAsync(LinxXMLDocumentos registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [portal], [cnpj_emp], [documento], [serie], [data_emissao], [chave_nfe], [situacao], [xml], [excluido], [identificador_microvix],
                             [dt_insert], [timestamp], [nProtCanc], [nProtInut], [xmlDistribuicao], [nProtDeneg], [cStat], [id_nfe], [justificativa]
                            ) 
                            Values 
                            (@lastupdateon, @portal, @cnpj_emp, @documento, @serie, @data_emissao, @chave_nfe, @situacao, @xml, @excluido, @identificador_microvix,
                             @dt_insert, @timestamp, @nProtCanc, @nProtInut, @xmlDistribuicao, @nProtDeneg, @cStat, @id_nfe, @justificativa
                            )";

            try
            {
                _linxMicrovixRepositoryBase.InsereRegistroIndividualNotAsync(tableName, sql, registro);
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

        public Task<List<LinxXMLDocumentos>> GetRegistersExistsAsync(List<LinxXMLDocumentos> registros, string tableName, string database)
        {
            throw new NotImplementedException();
        }

        public List<LinxXMLDocumentos> GetRegistersExistsNotAsync(List<LinxXMLDocumentos> registros, string tableName, string database)
        {
            throw new NotImplementedException();
        }
    }
}
