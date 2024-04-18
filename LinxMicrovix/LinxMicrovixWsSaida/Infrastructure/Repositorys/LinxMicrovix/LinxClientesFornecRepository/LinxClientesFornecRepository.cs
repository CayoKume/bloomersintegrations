using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.Base;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix
{
    public class LinxClientesFornecRepository : ILinxClientesFornecRepository
    {
        private readonly ILinxMicrovixRepositoryBase<LinxClientesFornec> _linxMicrovixRepositoryBase;

        public LinxClientesFornecRepository(ILinxMicrovixRepositoryBase<LinxClientesFornec> linxMicrovixRepositoryBase) =>
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;

        public async void BulkInsertIntoTableRaw(List<LinxClientesFornec> registros, string tableName, string database)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateDataTable(tableName, new LinxClientesFornec().GetType().GetProperties());

                for (int i = 0; i < registros.Count(); i++)
                {
                    table.Rows.Add(registros[i].lastupdateon, registros[i].portal, registros[i].cod_cliente, registros[i].razao_cliente, registros[i].nome_cliente, registros[i].doc_cliente, registros[i].tipo_cliente, registros[i].endereco_cliente,
                        registros[i].numero_rua_cliente, registros[i].complement_end_cli, registros[i].bairro_cliente, registros[i].cep_cliente, registros[i].cidade_cliente, registros[i].uf_cliente, registros[i].pais,
                        registros[i].fone_cliente, registros[i].email_cliente, registros[i].sexo, registros[i].data_cadastro, registros[i].data_nascimento, registros[i].cel_cliente, registros[i].ativo, registros[i].dt_update,
                        registros[i].inscricao_estadual, registros[i].incricao_municipal, registros[i].identidade_cliente, registros[i].cartao_fidelidade, registros[i].cod_ibge_municipio, registros[i].classe_cliente, registros[i].matricula_conveniado, registros[i].tipo_cadastro, registros[i].empresa_cadastro,
                        registros[i].id_estado_civil, registros[i].fax_cliente, registros[i].site_cliente, registros[i].timestamp, registros[i].cliente_anonimo, registros[i].limite_compras, registros[i].codigo_ws, registros[i].limite_credito_compra, registros[i].id_classe_fiscal, registros[i].obs, registros[i].mae, registros[i].cliente_contribuinte);
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
            string sql = $@"SELECT {parameterCol} FROM [{database}].[dbo].[LinxAPIParam] (nolock) where method = '{tableName}'";

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
            string sql = $@"SELECT {parameterCol} FROM [{database}].[dbo].[LinxAPIParam] (nolock) where method = '{tableName}'";

            try
            {
                return _linxMicrovixRepositoryBase.GetParametersNotAsync(tableName, sql);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<LinxClientesFornec>> GetRegistersExistsAsync(List<LinxClientesFornec> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].doc_cliente}'";
                else
                    identificadores += $"'{registros[i].doc_cliente}', ";
            }
            string query = $"SELECT DOC_CLIENTE, TIMESTAMP FROM [{database}].[dbo].{tableName}_TRUSTED WHERE DOC_CLIENTE IN ({identificadores})";

            try
            {
                return await _linxMicrovixRepositoryBase.GetRegistersExistsAsync(tableName, query);
            }
            catch
            {
                throw;
            }
        }

        public List<LinxClientesFornec> GetRegistersExistsNotAsync(List<LinxClientesFornec> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].doc_cliente}'";
                else
                    identificadores += $"'{registros[i].doc_cliente}', ";
            }
            string query = $"SELECT DOC_CLIENTE, TIMESTAMP FROM [{database}].[dbo].{tableName}_TRUSTED WHERE DOC_CLIENTE IN ({identificadores})";

            try
            {
                return _linxMicrovixRepositoryBase.GetRegistersExistsNotAsync(tableName, query);
            }
            catch
            {
                throw;
            }
        }

        public async Task InsereRegistroIndividualAsync(LinxClientesFornec registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [portal], [cod_cliente], [razao_cliente], [nome_cliente], [doc_cliente], [tipo_cliente], [endereco_cliente],
                             [numero_rua_cliente], [complement_end_cli], [bairro_cliente], [cep_cliente], [cidade_cliente], [uf_cliente], [pais],
                             [fone_cliente], [email_cliente], [sexo], [data_cadastro], [data_nascimento], [cel_cliente], [ativo], [dt_update],
                             [inscricao_estadual], [incricao_municipal], [identidade_cliente], [cartao_fidelidade], [cod_ibge_municipio], [classe_cliente], 
                             [matricula_conveniado], [tipo_cadastro], [empresa_cadastro], [id_estado_civil], [fax_cliente], [site_cliente], [timestamp], 
                             [cliente_anonimo], [limite_compras], [codigo_ws], [limite_credito_compra], [id_classe_fiscal], [obs], [mae], [cliente_contribuinte]) 
                            Values 
                            (@lastupdateon, @portal, @cod_cliente, @razao_cliente, @nome_cliente, @doc_cliente, @tipo_cliente, @endereco_cliente,
                             @numero_rua_cliente, @complement_end_cli, @bairro_cliente, @cep_cliente, @cidade_cliente, @uf_cliente, @pais,
                             @fone_cliente, @email_cliente, @sexo, @data_cadastro, @data_nascimento, @cel_cliente, @ativo, @dt_update,
                             @inscricao_estadual, @incricao_municipal, @identidade_cliente, @cartao_fidelidade, @cod_ibge_municipio, @classe_cliente, 
                             @matricula_conveniado, @tipo_cadastro, @empresa_cadastro, @id_estado_civil, @fax_cliente, @site_cliente, @timestamp, 
                             @cliente_anonimo, @limite_compras, @codigo_ws, @limite_credito_compra, @id_classe_fiscal, @obs, @mae, @cliente_contribuinte)";

            try
            {
                await _linxMicrovixRepositoryBase.InsereRegistroIndividualAsync(tableName, sql, registro);
            }
            catch
            {
                throw;
            }
        }

        public void InsereRegistroIndividualNotAsync(LinxClientesFornec registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [portal], [cod_cliente], [razao_cliente], [nome_cliente], [doc_cliente], [tipo_cliente], [endereco_cliente],
                             [numero_rua_cliente], [complement_end_cli], [bairro_cliente], [cep_cliente], [cidade_cliente], [uf_cliente], [pais],
                             [fone_cliente], [email_cliente], [sexo], [data_cadastro], [data_nascimento], [cel_cliente], [ativo], [dt_update],
                             [inscricao_estadual], [incricao_municipal], [identidade_cliente], [cartao_fidelidade], [cod_ibge_municipio], [classe_cliente], 
                             [matricula_conveniado], [tipo_cadastro], [empresa_cadastro], [id_estado_civil], [fax_cliente], [site_cliente], [timestamp], 
                             [cliente_anonimo], [limite_compras], [codigo_ws], [limite_credito_compra], [id_classe_fiscal], [obs], [mae], [cliente_contribuinte]) 
                            Values 
                            (@lastupdateon, @portal, @cod_cliente, @razao_cliente, @nome_cliente, @doc_cliente, @tipo_cliente, @endereco_cliente,
                             @numero_rua_cliente, @complement_end_cli, @bairro_cliente, @cep_cliente, @cidade_cliente, @uf_cliente, @pais,
                             @fone_cliente, @email_cliente, @sexo, @data_cadastro, @data_nascimento, @cel_cliente, @ativo, @dt_update,
                             @inscricao_estadual, @incricao_municipal, @identidade_cliente, @cartao_fidelidade, @cod_ibge_municipio, @classe_cliente, 
                             @matricula_conveniado, @tipo_cadastro, @empresa_cadastro, @id_estado_civil, @fax_cliente, @site_cliente, @timestamp, 
                             @cliente_anonimo, @limite_compras, @codigo_ws, @limite_credito_compra, @id_classe_fiscal, @obs, @mae, @cliente_contribuinte)";

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
