using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;
using BloomersMicrovixIntegrations.LinxMicrovix.Infrastructure.Repositorys.Base;

namespace BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxCommerce
{
    public class B2CConsultaNFeRepository : IB2CConsultaNFeRepository
    {
        private readonly ILinxMicrovixRepositoryBase<B2CConsultaNFe> _linxMicrovixRepositoryBase;

        public B2CConsultaNFeRepository(ILinxMicrovixRepositoryBase<B2CConsultaNFe> linxMicrovixRepositoryBase) =>
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;

        public void BulkInsertIntoTableRaw(List<B2CConsultaNFe> notasFiscais, string tableName, string database)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateDataTable(tableName, new B2CConsultaNFe().GetType().GetProperties());

                for (int i = 0; i < notasFiscais.Count(); i++)
                {
                    table.Rows.Add(notasFiscais[i].lastupdateon, notasFiscais[i].id_nfe, notasFiscais[i].id_pedido, notasFiscais[i].documento, notasFiscais[i].data_emissao, notasFiscais[i].chave_nfe, notasFiscais[i].situacao,
                                   notasFiscais[i].xml, notasFiscais[i].excluido, notasFiscais[i].identificador_microvix, notasFiscais[i].dt_insert, notasFiscais[i].valor_nota, notasFiscais[i].serie, notasFiscais[i].frete,
                                   notasFiscais[i].timestamp, notasFiscais[i].portal, notasFiscais[i].nProt, notasFiscais[i].codigo_modelo_nf, notasFiscais[i].justificativa);
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

        public async Task<List<B2CConsultaNFe>> GetRegistersExistsAsync(List<B2CConsultaNFe> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].chave_nfe}'";
                else
                    identificadores += $"'{registros[i].chave_nfe}', ";
            }
            string query = $"SELECT CHAVE_NFE, TIMESTAMP FROM [{database}].[dbo].[{tableName}_TRUSTED] WHERE CHAVE_NFE IN ({identificadores})";

            try
            {
                return await _linxMicrovixRepositoryBase.GetRegistersExistsAsync(tableName, database);
            }
            catch
            {
                throw;
            }
        }

        public List<B2CConsultaNFe> GetRegistersExistsNotAsync(List<B2CConsultaNFe> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].chave_nfe}'";
                else
                    identificadores += $"'{registros[i].chave_nfe}', ";
            }
            string query = $"SELECT CHAVE_NFE, TIMESTAMP FROM [{database}].[dbo].[{tableName}_TRUSTED] WHERE CHAVE_NFE IN ({identificadores})";

            try
            {
                return _linxMicrovixRepositoryBase.GetRegistersExistsNotAsync(tableName, database);
            }
            catch
            {
                throw;
            }
        }

        public async Task InsereRegistroIndividualAsync(B2CConsultaNFe registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [id_nfe], [id_pedido], [documento], [data_emissao], [chave_nfe],
                             [situacao], [xml], [excluido], [identificador_microvix], [dt_insert], [valor_nota],
                             [serie], [frete], [timestamp], [portal], [nProt], [codigo_modelo_nf], [justificativa]) 
                            Values 
                            (@lastupdateon, @id_nfe, @id_pedido, @documento, @data_emissao, @chave_nfe,
                             @situacao, @xml, @excluido, @identificador_microvix, @dt_insert, @valor_nota,
                             @serie, @frete, @timestamp, @portal, @nProt, @codigo_modelo_nf, @justificativa)";

            try
            {
                await _linxMicrovixRepositoryBase.InsereRegistroIndividualAsync(tableName, sql, registro);
            }
            catch
            {
                throw;
            }
        }

        public void InsereRegistroIndividualNotAsync(B2CConsultaNFe registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [id_nfe], [id_pedido], [documento], [data_emissao], [chave_nfe],
                             [situacao], [xml], [excluido], [identificador_microvix], [dt_insert], [valor_nota],
                             [serie], [frete], [timestamp], [portal], [nProt], [codigo_modelo_nf], [justificativa]) 
                            Values 
                            (@lastupdateon, @id_nfe, @id_pedido, @documento, @data_emissao, @chave_nfe,
                             @situacao, @xml, @excluido, @identificador_microvix, @dt_insert, @valor_nota,
                             @serie, @frete, @timestamp, @portal, @nProt, @codigo_modelo_nf, @justificativa)";

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
