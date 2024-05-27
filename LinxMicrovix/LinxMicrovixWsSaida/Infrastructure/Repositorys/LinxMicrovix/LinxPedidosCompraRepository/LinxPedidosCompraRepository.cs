using BloomersIntegrationsCore.Domain.Entities;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.Base;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix
{
    public class LinxPedidosCompraRepository : ILinxPedidosCompraRepository
    {
        private readonly ILinxMicrovixRepositoryBase<LinxPedidosCompra> _linxMicrovixRepositoryBase;

        public LinxPedidosCompraRepository(ILinxMicrovixRepositoryBase<LinxPedidosCompra> linxMicrovixRepositoryBase) =>
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;

        public void BulkInsertIntoTableRaw(List<LinxPedidosCompra> registros, string tableName, string database)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateDataTable(tableName, new LinxPedidosCompra().GetType().GetProperties());

                for (int i = 0; i < registros.Count(); i++)
                {
                    table.Rows.Add(registros[i].lastupdateon, registros[i].portal, registros[i].cnpj_emp, registros[i].cod_pedido, registros[i].data_pedido,
                                   registros[i].transacao, registros[i].usuario, registros[i].codigo_fornecedor, registros[i].cod_produto, registros[i].quantidade,
                                   registros[i].valor_unitario, registros[i].cod_comprador, registros[i].valor_frete, registros[i].valor_total, registros[i].cod_plano_pagamento,
                                   registros[i].plano_pagamento, registros[i].obs, registros[i].aprovado, registros[i].cancelado, registros[i].encerrado, registros[i].data_aprovacao,
                                   registros[i].numero_ped_fornec, registros[i].tipo_frete, registros[i].natureza_operacao, registros[i].previsao_entrega, registros[i].numero_projeto_officina,
                                   registros[i].status_pedido, registros[i].qtde_entregue, registros[i].descricao_frete, registros[i].integrado_linx, registros[i].nf_gerada, registros[i].timestamp,
                                   registros[i].empresa);
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
            string sql = $@"SELECT empresa as cod_company, nome_emp as name_company, cnpj_emp as doc_company FROM BLOOMERS_LINX..LinxLojas_trusted WHERE nome_emp LIKE '%MISHA%' or nome_emp LIKE '%OPEN%'";

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
            string sql = $@"SELECT empresa as cod_company, nome_emp as name_company, cnpj_emp as doc_company FROM BLOOMERS_LINX..LinxLojas_trusted WHERE nome_emp LIKE '%MISHA%' or nome_emp LIKE '%OPEN%'";

            try
            {
                return _linxMicrovixRepositoryBase.GetCompanysNotAsync(tableName, sql);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<LinxPedidosCompra>> GetRegistersExistsAsync(List<LinxPedidosCompra> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].cnpj_emp}'";
                else
                    identificadores += $"'{registros[i].cnpj_emp}', ";
            }
            string query = $"SELECT cnpj_emp, cod_produto, cod_pedido, TIMESTAMP FROM {database}.[dbo].{tableName} WHERE cod_pedido IN ({identificadores})";

            try
            {
                return await _linxMicrovixRepositoryBase.GetRegistersExistsAsync(tableName, query);
            }
            catch
            {
                throw;
            }
        }

        public List<LinxPedidosCompra> GetRegistersExistsNotAsync(List<LinxPedidosCompra> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].cnpj_emp}'";
                else
                    identificadores += $"'{registros[i].cnpj_emp}', ";
            }
            string query = $"SELECT cnpj_emp, cod_produto, cod_pedido, TIMESTAMP FROM {database}.[dbo].{tableName} WHERE cod_pedido IN ({identificadores})";

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
