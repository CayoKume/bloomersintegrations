using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.Base;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix
{
    public class LinxLojasRepository : ILinxLojasRepository
    {
        private readonly ILinxMicrovixRepositoryBase<LinxLojas> _linxMicrovixRepositoryBase;

        public LinxLojasRepository(ILinxMicrovixRepositoryBase<LinxLojas> linxMicrovixRepositoryBase) =>
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;

        public void BulkInsertIntoTableRaw(List<LinxLojas> registros, string tableName, string database)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateDataTable(tableName, new LinxLojas().GetType().GetProperties());

                for (int i = 0; i < registros.Count(); i++)
                {
                    table.Rows.Add(registros[i].lastupdateon, registros[i].portal, registros[i].empresa, registros[i].nome_emp, registros[i].razao_emp, registros[i].cnpj_emp, registros[i].inscricao_emp, registros[i].endereco_emp,
                        registros[i].num_emp, registros[i].complement_emp, registros[i].bairro_emp, registros[i].cep_emp, registros[i].cidade_emp, registros[i].estado_emp, registros[i].fone_emp,
                        registros[i].email_emp, registros[i].cod_ibge_municipio, registros[i].data_criacao_emp, registros[i].data_criacao_portal, registros[i].sistema_tributacao, registros[i].regime_tributario, registros[i].area_empresa, registros[i].timestamp,
                        registros[i].sigla_empresa, registros[i].id_classe_fiscal, registros[i].centro_distribuicao, registros[i].cnae_emp, registros[i].cod_cliente_linx);
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

        public async Task<List<LinxLojas>> GetRegistersExistsAsync(List<LinxLojas> registros, string tableName, string? db)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].cnpj_emp}'";
                else
                    identificadores += $"'{registros[i].cnpj_emp}', ";
            }
            string query = $"SELECT cnpj_emp, TIMESTAMP FROM BLOOMERS_LINX..LinxLojas_trusted WHERE cnpj_emp IN ({identificadores})";

            try
            {
                return await _linxMicrovixRepositoryBase.GetRegistersExistsAsync(tableName, query);
            }
            catch
            {
                throw;
            }
        }

        public List<LinxLojas> GetRegistersExistsNotAsync(List<LinxLojas> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].cnpj_emp}'";
                else
                    identificadores += $"'{registros[i].cnpj_emp}', ";
            }
            string query = $"SELECT cnpj_emp, TIMESTAMP FROM BLOOMERS_LINX..LinxLojas_trusted WHERE cnpj_emp IN ({identificadores})";

            try
            {
                return _linxMicrovixRepositoryBase.GetRegistersExistsNotAsync(tableName, query);
            }
            catch
            {
                throw;
            }
        }

        public async Task InsereRegistroIndividualAsync(LinxLojas registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [portal], [empresa], [nome_emp], [razao_emp], [cnpj_emp], [inscricao_emp], [endereco_emp],
                             [num_emp], [complement_emp], [bairro_emp], [cep_emp], [cidade_emp], [estado_emp], [fone_emp],
                             [email_emp], [cod_ibge_municipio], [data_criacao_emp], [data_criacao_portal], [sistema_tributacao], [regime_tributario], [area_empresa], [timestamp],
                             [sigla_empresa], [id_classe_fiscal], [centro_distribuicao], [inscricao_municipal_emp], [cnae_emp], [cod_cliente_linx]) 
                            Values 
                            (@lastupdateon, @portal, @empresa, @nome_emp, @razao_emp, @cnpj_emp, @inscricao_emp, @endereco_emp,
                             @num_emp, @complement_emp, @bairro_emp, @cep_emp, @cidade_emp, @estado_emp, @fone_emp,
                             @email_emp, @cod_ibge_municipio, @data_criacao_emp, @data_criacao_portal, @sistema_tributacao, @regime_tributario, @area_empresa, @timestamp,
                             @sigla_empresa, @id_classe_fiscal, @centro_distribuicao, @inscricao_municipal_emp, @cnae_emp, @cod_cliente_linx)";

            try
            {
                await _linxMicrovixRepositoryBase.InsereRegistroIndividualAsync(tableName, sql, registro);
            }
            catch
            {
                throw;
            }
        }

        public void InsereRegistroIndividualNotAsync(LinxLojas registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [portal], [empresa], [nome_emp], [razao_emp], [cnpj_emp], [inscricao_emp], [endereco_emp],
                             [num_emp], [complement_emp], [bairro_emp], [cep_emp], [cidade_emp], [estado_emp], [fone_emp],
                             [email_emp], [cod_ibge_municipio], [data_criacao_emp], [data_criacao_portal], [sistema_tributacao], [regime_tributario], [area_empresa], [timestamp],
                             [sigla_empresa], [id_classe_fiscal], [centro_distribuicao], [inscricao_municipal_emp], [cnae_emp], [cod_cliente_linx]) 
                            Values 
                            (@lastupdateon, @portal, @empresa, @nome_emp, @razao_emp, @cnpj_emp, @inscricao_emp, @endereco_emp,
                             @num_emp, @complement_emp, @bairro_emp, @cep_emp, @cidade_emp, @estado_emp, @fone_emp,
                             @email_emp, @cod_ibge_municipio, @data_criacao_emp, @data_criacao_portal, @sistema_tributacao, @regime_tributario, @area_empresa, @timestamp,
                             @sigla_empresa, @id_classe_fiscal, @centro_distribuicao, @inscricao_municipal_emp, @cnae_emp, @cod_cliente_linx)";

            try
            {
                _linxMicrovixRepositoryBase.InsereRegistroIndividualNotAsync(tableName, sql, registro);
            }
            catch
            {
                throw;
            }
        }
    }
}
