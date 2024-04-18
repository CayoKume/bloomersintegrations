using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxEcommerce;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.Base;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxCommerce
{
    public class B2CConsultaPedidosRepository : IB2CConsultaPedidosRepository
    {
        private readonly ILinxMicrovixRepositoryBase<B2CConsultaPedidos> _linxMicrovixRepositoryBase;

        public B2CConsultaPedidosRepository(ILinxMicrovixRepositoryBase<B2CConsultaPedidos> linxMicrovixRepositoryBase) =>
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;

        public void BulkInsertIntoTableRaw(List<B2CConsultaPedidos> registros, string tableName, string database)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateDataTable(tableName, new B2CConsultaPedidos().GetType().GetProperties());

                for (int i = 0; i < registros.Count(); i++)
                {
                    table.Rows.Add(registros[i].lastupdateon, registros[i].id_pedido, registros[i].dt_pedido.ToString("yyyy-MM-dd HH:mm:ss"), registros[i].cod_cliente_erp, registros[i].cod_cliente_b2c, registros[i].vl_frete, registros[i].forma_pgto, registros[i].plano_pagamento,
                        registros[i].anotacao, registros[i].taxa_impressao, registros[i].finalizado, registros[i].valor_frete_gratis, registros[i].tipo_frete, registros[i].id_status, registros[i].cod_transportador,
                        registros[i].tipo_cobranca_frete, registros[i].ativo, registros[i].empresa, registros[i].id_tabela_preco, registros[i].valor_credito, registros[i].cod_vendedor, registros[i].timestamp, registros[i].dt_insert.ToString("yyyy-MM-dd HH:mm:ss"),
                        registros[i].dt_disponivel_faturamento.ToString("yyyy-MM-dd HH:mm:ss"), registros[i].portal, registros[i].mensagem_falha_faturamento, registros[i].id_tipo_b2c, registros[i].ecommerce_origem, registros[i].order_id, registros[i].fulfillment_id);
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

        public async Task<List<B2CConsultaPedidos>> GetRegistersExistsAsync(List<B2CConsultaPedidos> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].order_id}'";
                else
                    identificadores += $"'{registros[i].order_id}', ";
            }
            string sql = $"SELECT order_id, timestamp FROM [{database}].[dbo].[{tableName}_TRUSTED] WHERE order_id IN ({identificadores})";

            try
            {
                return await _linxMicrovixRepositoryBase.GetRegistersExistsAsync(tableName, sql);
            }
            catch
            {
                throw;
            }
        }

        public List<B2CConsultaPedidos> GetRegistersExistsNotAsync(List<B2CConsultaPedidos> registros, string tableName, string database)
        {
            var identificadores = String.Empty;
            for (int i = 0; i < registros.Count(); i++)
            {
                if (i == registros.Count() - 1)
                    identificadores += $"'{registros[i].order_id}'";
                else
                    identificadores += $"'{registros[i].order_id}', ";
            }
            string sql = $"SELECT order_id, timestamp FROM [{database}].[dbo].[{tableName}_TRUSTED] WHERE order_id IN ({identificadores})";

            try
            {
                return _linxMicrovixRepositoryBase.GetRegistersExistsNotAsync(tableName, sql);
            }
            catch
            {
                throw;
            }
        }

        public async Task InsereRegistroIndividualAsync(B2CConsultaPedidos registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [id_pedido], [dt_pedido], [cod_cliente_erp], [cod_cliente_b2c], [vl_frete], [forma_pgto], [plano_pagamento], [anotacao], [taxa_impressao], [finalizado], [valor_frete_gratis], 
                             [tipo_frete], [id_status], [cod_transportador], [tipo_cobranca_frete], [ativo], [empresa], [id_tabela_preco], [valor_credito], [cod_vendedor], [timestamp], [dt_insert], [dt_disponivel_faturamento], 
                             [portal], [mensagem_falha_faturamento], [id_tipo_b2c], [ecommerce_origem], [order_id], [fulfillment_id]) 
                            Values 
                            (@lastupdateon, @id_pedido, @dt_pedido, @cod_cliente_erp, @cod_cliente_b2c, @vl_frete, @forma_pgto, @plano_pagamento, @anotacao, @taxa_impressao, @finalizado, @valor_frete_gratis, 
                             @tipo_frete, @id_status, @cod_transportador, @tipo_cobranca_frete, @ativo, @empresa, @id_tabela_preco, @valor_credito, @cod_vendedor, @timestamp, @dt_insert, @dt_disponivel_faturamento, 
                             @portal, @mensagem_falha_faturamento, @id_tipo_b2c, @ecommerce_origem, @order_id, @fulfillment_id)";

            try
            {
                await _linxMicrovixRepositoryBase.InsereRegistroIndividualAsync(tableName, sql, registro);
            }
            catch
            {
                throw;
            }
        }

        public void InsereRegistroIndividualNotAsync(B2CConsultaPedidos registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [id_pedido], [dt_pedido], [cod_cliente_erp], [cod_cliente_b2c], [vl_frete], [forma_pgto], [plano_pagamento], [anotacao], [taxa_impressao], [finalizado], [valor_frete_gratis], 
                             [tipo_frete], [id_status], [cod_transportador], [tipo_cobranca_frete], [ativo], [empresa], [id_tabela_preco], [valor_credito], [cod_vendedor], [timestamp], [dt_insert], [dt_disponivel_faturamento], 
                             [portal], [mensagem_falha_faturamento], [id_tipo_b2c], [ecommerce_origem], [order_id], [fulfillment_id]) 
                            Values 
                            (@lastupdateon, @id_pedido, @dt_pedido, @cod_cliente_erp, @cod_cliente_b2c, @vl_frete, @forma_pgto, @plano_pagamento, @anotacao, @taxa_impressao, @finalizado, @valor_frete_gratis, 
                             @tipo_frete, @id_status, @cod_transportador, @tipo_cobranca_frete, @ativo, @empresa, @id_tabela_preco, @valor_credito, @cod_vendedor, @timestamp, @dt_insert, @dt_disponivel_faturamento, 
                             @portal, @mensagem_falha_faturamento, @id_tipo_b2c, @ecommerce_origem, @order_id, @fulfillment_id)";

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
