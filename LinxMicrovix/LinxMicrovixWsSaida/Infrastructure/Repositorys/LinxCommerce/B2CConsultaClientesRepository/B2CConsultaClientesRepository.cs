using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxEcommerce;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.Base;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxCommerce
{
    public class B2CConsultaClientesRepository : IB2CConsultaClientesRepository
    {
        private readonly ILinxMicrovixRepositoryBase<B2CConsultaClientes> _linxMicrovixRepositoryBase;

        public B2CConsultaClientesRepository(ILinxMicrovixRepositoryBase<B2CConsultaClientes> linxMicrovixRepositoryBase) =>
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;

        public void BulkInsertIntoTableRaw(List<B2CConsultaClientes> registros, string tableName, string database)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateDataTable(tableName, new B2CConsultaClientes().GetType().GetProperties());

                for (int i = 0; i < registros.Count(); i++)
                {
                    table.Rows.Add(registros[i].lastupdateon, registros[i].cod_cliente_b2c, registros[i].cod_cliente_erp, registros[i].doc_cliente, registros[i].nm_cliente, registros[i].nm_mae, registros[i].nm_pai, registros[i].nm_conjuge,
                        registros[i].dt_cadastro.Value.ToString("yyyy-MM-dd HH:mm:ss"), registros[i].dt_nasc_cliente.Value.ToString("yyyy-MM-dd HH:mm:ss"), registros[i].end_cliente, registros[i].complemento_end_cliente, registros[i].nr_rua_cliente, registros[i].bairro_cliente, registros[i].cep_cliente,
                        registros[i].cidade_cliente, registros[i].uf_cliente, registros[i].fone_cliente, registros[i].fone_comercial, registros[i].cel_cliente, registros[i].email_cliente, registros[i].rg_cliente, registros[i].rg_orgao_emissor,
                        registros[i].estado_civil_cliente, registros[i].empresa_cliente, registros[i].cargo_cliente, registros[i].sexo_cliente, registros[i].dt_update.Value.ToString("yyyy-MM-dd HH:mm:ss"), registros[i].ativo, registros[i].receber_email, registros[i].dt_expedicao_rg.Value.ToString("yyyy-MM-dd HH:mm:ss"), registros[i].naturalidade,
                        registros[i].tempo_residencia, registros[i].renda, registros[i].numero_compl_rua_cliente, registros[i].timestamp, registros[i].tipo_pessoa, registros[i].portal);
                }

                _linxMicrovixRepositoryBase.BulkInsertIntoTableRaw(table, database, tableName, table.Rows.Count);
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> GetTableLastTimestampAsync(string database, string tableName)
        {
            string sql = $@"SELECT MIN(TIMESTAMP) 
                            FROM [{database}].[dbo].[{tableName}_trusted] A (nolock) 
                            WHERE 
                            --DOC_CLIENTE IN ()
                            DATA_CADASTRO > '{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}T00:00:00' 
                            AND DATA_CADASTRO < '{DateTime.Today.ToString("yyyy-MM-dd")}T23:59:59'";

            try
            {
                return await _linxMicrovixRepositoryBase.GetTableLastTimestampAsync(tableName, sql);
            }
            catch
            {
                throw;
            }
        }

        public string GetTableLastTimestampNotAsync(string database, string tableName)
        {
            string sql = $@"SELECT MIN(TIMESTAMP) 
                            FROM [{database}].[dbo].[{tableName}_trusted] A (nolock) 
                            WHERE 
                            --DOC_CLIENTE IN ()
                            DATA_CADASTRO > '{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}T00:00:00' 
                            AND DATA_CADASTRO < '{DateTime.Today.ToString("yyyy-MM-dd")}T23:59:59'";

            try
            {
                return _linxMicrovixRepositoryBase.GetTableLastTimestampNotAsync(tableName, sql);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<B2CConsultaClientes>> GetRegistersExistsAsync(List<B2CConsultaClientes> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].doc_cliente}'";
                else
                    identificadores += $"'{registros[i].doc_cliente}', ";
            }

            string sql = $"SELECT DOC_CLIENTE, TIMESTAMP FROM BLOOMERS_LINX..B2CCONSULTACLIENTES_TRUSTED WHERE DOC_CLIENTE IN ({identificadores})";
            
            try
            {
                return await _linxMicrovixRepositoryBase.GetRegistersExistsAsync(tableName, sql);
            }
            catch
            {
                throw;
            }
        }

        public List<B2CConsultaClientes> GetRegistersExistsNotAsync(List<B2CConsultaClientes> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].doc_cliente}'";
                else
                    identificadores += $"'{registros[i].doc_cliente}', ";
            }

            string sql = $"SELECT DOC_CLIENTE, TIMESTAMP FROM BLOOMERS_LINX..B2CCONSULTACLIENTES_TRUSTED WHERE DOC_CLIENTE IN ({identificadores})";

            try
            {
                return _linxMicrovixRepositoryBase.GetRegistersExistsNotAsync(tableName, sql);
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

        public async Task InsereRegistroIndividualAsync(B2CConsultaClientes registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [cod_cliente_b2c], [cod_cliente_erp], [doc_cliente], [nm_cliente], [nm_mae], [nm_pai], [nm_conjuge], [dt_cadastro], [dt_nasc_cliente], [end_cliente],[complemento_end_cliente], 
                             [nr_rua_cliente], [bairro_cliente], [cep_cliente], [cidade_cliente], [uf_cliente], [fone_cliente], [fone_comercial], [cel_cliente],[email_cliente], [rg_cliente], [rg_orgao_emissor], [estado_civil_cliente], 
                             [empresa_cliente], [cargo_cliente], [sexo_cliente], [dt_update], [ativo], [receber_email],[dt_expedicao_rg], [naturalidade], [tempo_residencia], [renda],[numero_compl_rua_cliente], [timestamp], 
                             [tipo_pessoa], [portal]) 
                            Values 
                            (@lastupdateon, @cod_cliente_b2c, @cod_cliente_erp, @doc_cliente, @nm_cliente, @nm_mae, @nm_pai, @nm_conjuge, @dt_cadastro, @dt_nasc_cliente, @end_cliente,
                             @complemento_end_cliente, @nr_rua_cliente, @bairro_cliente, @cep_cliente, @cidade_cliente, @uf_cliente, @fone_cliente, @fone_comercial, @cel_cliente,
                             @email_cliente, @rg_cliente, @rg_orgao_emissor, @estado_civil_cliente, @empresa_cliente, @cargo_cliente, @sexo_cliente, @dt_update, @ativo, @receber_email,
                             @dt_expedicao_rg, @naturalidade, @tempo_residencia, @renda,@numero_compl_rua_cliente, @timestamp, @tipo_pessoa, @portal)";

            try
            {
                await _linxMicrovixRepositoryBase.InsereRegistroIndividualAsync(tableName, sql, registro);
            }
            catch
            {
                throw;
            }
        }

        public void InsereRegistroIndividualNotAsync(B2CConsultaClientes registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [cod_cliente_b2c], [cod_cliente_erp], [doc_cliente], [nm_cliente], [nm_mae], [nm_pai], [nm_conjuge], [dt_cadastro], [dt_nasc_cliente], [end_cliente],[complemento_end_cliente], 
                             [nr_rua_cliente], [bairro_cliente], [cep_cliente], [cidade_cliente], [uf_cliente], [fone_cliente], [fone_comercial], [cel_cliente],[email_cliente], [rg_cliente], [rg_orgao_emissor], [estado_civil_cliente], 
                             [empresa_cliente], [cargo_cliente], [sexo_cliente], [dt_update], [ativo], [receber_email],[dt_expedicao_rg], [naturalidade], [tempo_residencia], [renda],[numero_compl_rua_cliente], [timestamp], 
                             [tipo_pessoa], [portal]) 
                            Values 
                            (@lastupdateon, @cod_cliente_b2c, @cod_cliente_erp, @doc_cliente, @nm_cliente, @nm_mae, @nm_pai, @nm_conjuge, @dt_cadastro, @dt_nasc_cliente, @end_cliente,
                             @complemento_end_cliente, @nr_rua_cliente, @bairro_cliente, @cep_cliente, @cidade_cliente, @uf_cliente, @fone_cliente, @fone_comercial, @cel_cliente,
                             @email_cliente, @rg_cliente, @rg_orgao_emissor, @estado_civil_cliente, @empresa_cliente, @cargo_cliente, @sexo_cliente, @dt_update, @ativo, @receber_email,
                             @dt_expedicao_rg, @naturalidade, @tempo_residencia, @renda,@numero_compl_rua_cliente, @timestamp, @tipo_pessoa, @portal)";

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
