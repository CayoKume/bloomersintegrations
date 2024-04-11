using BloomersIntegrationsCore.Domain.Entities;
using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using BloomersWorkersCore.Domain.Entities;
using BloomersWorkersCore.Infrastructure.Repositorys;
using Dapper;
using Order = BloomersWorkers.ChangingOrder.Domain.Entities.Order;
using Product = BloomersWorkers.ChangingOrder.Domain.Entities.Product;

namespace BloomersWorkers.ChangingOrder.Infrastructure.Repositorys
{
    public class ChangingOrderRepository : IChangingOrderRepository
    {
        private readonly IBloomersWorkersCoreRepository _bloomersWorkersCoreRepository;
        private readonly ISQLServerConnection _conn;

        public ChangingOrderRepository(ISQLServerConnection conn, IBloomersWorkersCoreRepository bloomersWorkersCoreRepository) =>
            (_conn, _bloomersWorkersCoreRepository) = (conn, bloomersWorkersCoreRepository);

        public async Task<MicrovixUser> GetMicrovixUser(string gabot)
        {
            try
            {
                return await _bloomersWorkersCoreRepository.GetMicrovixUser(gabot);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Order>> GetOrdersFromIT4()
        {
            var sql = @$"SELECT DISTINCT
                             A.IDCONTROLE AS IDCONTROL,
                             TRIM(A.DOCUMENTO) AS NUMBER,
                             A.NB_DOC_REMETENTE AS DOC_COMPANY,
                             B.CODIGO_BARRA as COD_PRODUCT_MICROVIX,
                             B.CODIGO_BARRA as COD_PRODUCT_VOLO,
                             B.QTDE as QTDE_MICROVIX,
                             B.QTDERETORNO as QTDE_VOLO                             
                              
                             FROM [GENERAL].[dbo].IT4_WMS_DOCUMENTO A (NOLOCK)
                             JOIN [GENERAL].[dbo].IT4_WMS_DOCUMENTO_ITEM B (NOLOCK) ON A.IDCONTROLE = B.IDCONTROLE 
                             WHERE
                             --A.DOCUMENTO IN ('')
                             (A.DOCUMENTO LIKE '%-VD%' OR A.DOCUMENTO LIKE '%-LJ%')
                             AND B.QTDE != B.QTDERETORNO
                             AND A.RETORNO IS NOT NULL
                             AND A.CHAVE_NFE IS NULL
                             AND A.XML_FATURAMENTO IS NULL
                             AND A.NF_SAIDA IS NULL
                             AND A.ORIGEM = 'P'
                             AND A.SERIE != 'MX-'
                             AND A.CANCELADO IS NULL
							 AND A.CANCELAMENTO IS NULL";

            try
            {
                var result = await _conn.GetDbConnection().QueryAsync<Order, Company, Product, Order>(sql, (order, company, item) =>
                {
                    order.itens.Add(item);
                    order.company = company;

                    return order;
                }, splitOn: "doc_company, cod_product_microvix");

                var order = result.GroupBy(p => p.number).Select(g =>
                {
                    var groupedOrder = g.First();
                    groupedOrder.itens = g.Select(p => p.itens.Single()).ToList();
                    return groupedOrder;
                });

                return order.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($@"GetOrdersFromIT4 - Erro ao obter pedidos a serem cortados da tabela IT4_WMS_DOCUMENTO - {ex.Message}");
            }
        }

        public async Task UpdateReturnIT4ITEM(string nr_pedido, string idControle)
        {
            var sql = $@"UPDATE GENERAL..IT4_WMS_DOCUMENTO_ITEM SET 
	                        Qtde = QtdeRetorno
                            WHERE idControle = '{idControle}'";
            try
            {
                _conn.GetIDbConnection().ExecuteAsync(sql);
            }
            catch (Exception ex)
            {
                throw new Exception($"{nr_pedido} - UpdateIT4_WMS_DOCUMENTO_ITEM - Erro ao atualizar status do registro na tabela UpdateIT4_WMS_DOCUMENTO_ITEM - {ex.Message}");
            }
        }
    }
}
