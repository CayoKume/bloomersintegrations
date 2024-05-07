using BloomersIntegrationsCore.Domain.Entities;
using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using Dapper;
using Newtonsoft.Json;
using Order = BloomersMiniWmsIntegrations.Domain.Entities.Picking.Order;
using Product = BloomersMiniWmsIntegrations.Domain.Entities.Picking.Product;

namespace BloomersMiniWmsIntegrations.Infrastructure.Repositorys
{
    public class PickingRepository : IPickingRepository
    {
        private readonly ISQLServerConnection _conn;

        public PickingRepository(ISQLServerConnection conn) =>
            (_conn) = (conn);

        public async Task<List<ShippingCompany>?> GetShippingCompanys()
        {
            var sql = $@"SELECT 
                         cod_cliente as cod_shippingCompany,
                         doc_cliente as doc_shippingCompany,
                         razao_cliente as reason_shippingCompany,
                         nome_cliente as name_shippingCompany,
                         email_cliente as email_shippingCompany,
                         endereco_cliente as address_shippingCompany,
                         numero_rua_cliente as street_number_shippingCompany,
                         complement_end_cli as complement_address_shippingCompany,
                         bairro_cliente as neighborhood_shippingCompany,
                         cidade_cliente as city_shippingCompany,
                         uf_cliente as uf_shippingCompany,
                         cep_cliente as zip_code_shippingCompany,
                         fone_cliente as fone_shippingCompany,
                         inscricao_estadual as state_registration_shippingCompany,
                         incricao_municipal as municipal_registration_shippingCompany
                         FROM BLOOMERS_LINX..LinxClientesFornec_trusted (NOLOCK)
                         WHERE tipo_cadastro = 'T'
                         ORDER BY cod_cliente";

            try
            {
                var result = await _conn.GetDbConnection().QueryAsync<ShippingCompany>(sql);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [Picking] - GetShippingCompanys - Erro ao obter transportadoras da tabela LinxClientesFornec_trusted  - {ex.Message}");
            }
        }

        public async Task<Order?> GetUnpickedOrder(string cnpj_emp, string serie, string nr_pedido)
        {
            var sql = $@"SELECT DISTINCT
                        TRIM(A.DOCUMENTO) as number,
                        A.RETORNO as retorno,
                        A.VOLUMES as volumes,

                        A.NB_CODIGO_CLIENTE as cod_client,
                        A.NB_RAZAO_CLIENTE as reason_client,
                        A.NB_DOC_CLIENTE as doc_client,
                        A.NB_ENDERECO_CLIENTE as address_client,
                        A.NB_NUMERO_RUA_CLIENTE as street_number_client,
                        A.NB_COMPLEMENTO_END_CLIENTE as complement_address_client,
                        A.NB_BAIRRO_CLIENTE as neighborhood_client,
                        A.NB_CIDADE as city_client,
                        A.NB_ESTADO as uf_client,
                        TRIM(REPLACE(A.NB_CEP, '-', '')) as zip_code_client,
                        A.NB_FONE_CLIENTE as fone_client,

                        A.NB_TRANSPORTADORA as cod_shippingCompany,
                        A.NB_METODO_TRANSPORTADORA as metodo_shippingCompany,
                        A.NB_RAZAO_TRANSPORTADORA as reason_shippingCompany,
                        A.NB_NOME_TRANSPORTADORA as name_shippingCompany,
                        A.NB_DOC_TRANSPORTADORA as doc_shippingCompany,
                        A.NB_EMAIL_TRANSPORTADORA as email_shippingCompany,
                        A.NB_ENDERECO_TRANSPORTADORA as address_shippingCompany,
                        A.NB_NUMERO_RUA_TRANSPORTADORA as street_number_shippingCompany,
                        A.NB_COMPLEMENTO_END_TRANSPORTADORA as complement_address_shippingCompany,
                        A.NB_BAIRRO_TRANSPORTADORA as neighborhood_shippingCompany,
                        A.NB_CIDADE_TRANSPORTADORA as city_shippingCompany,
                        A.NB_UF_TRANSPORTADORA as uf_shippingCompany,
                        A.NB_CEP_TRANSPORTADORA as zip_code_shippingCompany,
                        A.NB_FONE_TRANSPORTADORA as fone_shippingCompany,
                        A.NB_INSCRICAO_ESTADUAL_TRANSPORTADORA as state_registration_shippingCompany,
                              
                        A.NB_VALOR_PEDIDO as amount_nf,

                        TRIM(B.CODIGO_BARRA) as cod_product,
						B.QTDE as quantity_product,
						B.NB_SKU_PRODUTO as sku_product,
						B.DESCRICAO as description_product,
						TRIM(B.CODIGO_BARRA) as cod_ean_product,
						B.NB_VALOR_UNITARIO_PRODUTO as unitary_value_product,
						B.NB_VALOR_TOTAL_PRODUTO as amount_product,
						B.NB_VALOR_FRETE_PRODUTO as shipping_value_product,
                        C.IMG1 as urlImg

                        FROM GENERAL..IT4_WMS_DOCUMENTO A
                        JOIN GENERAL..IT4_WMS_DOCUMENTO_ITEM B ON A.IDCONTROLE = B.IDCONTROLE
						LEFT JOIN BLOOMERS_OPERACOES..CANAL_MOVIMENTACAO C ON B.CODIGO_BARRA = C.COD_PRODUTO AND C.PEDIDO = A.DOCUMENTO
                        WHERE
                        A.SERIE = '{serie}'
                        AND A.NB_DOC_REMETENTE = '{cnpj_emp}' 
						AND A.DOCUMENTO = '{nr_pedido}'
                        AND A.CANCELADO IS NULL";

            try
            {
                var result = await _conn.GetDbConnection().QueryAsync<Order, Client, ShippingCompany, Invoice, Product, Order>(sql, (pedido, cliente, transportadora, nota_fiscal, produto) =>
                {
                    pedido.client = cliente;
                    pedido.shippingCompany = transportadora;
                    pedido.invoice = nota_fiscal;
                    pedido.itens.Add(produto);
                    return pedido;
                }, splitOn: "cod_client, cod_shippingCompany, amount_nf, cod_product");

                var pedidos = result.GroupBy(p => p.number).Select(g =>
                {
                    var groupedOrder = g.First();
                    groupedOrder.itens = g.Select(p => p.itens.Single()).ToList();
                    return groupedOrder;
                });

                return pedidos.First();
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [Picking] - GetUnpickedOrder - Erro ao obter pedido: {nr_pedido} na tabela IT4_WMS_DOCUMENTO  - {ex.Message}");
            }
        }

        public async Task<List<Order>?> GetUnpickedOrders(string cnpj_emp, string serie_pedido, string data_inicial, string data_final)
        {
            var sql = $@"SELECT DISTINCT
                        TRIM(A.DOCUMENTO) as number,
                        A.RETORNO as retorno,
                        A.VOLUMES as volumes,

                        A.NB_CODIGO_CLIENTE as cod_client,
                        A.NB_RAZAO_CLIENTE as reason_client,
                        A.NB_DOC_CLIENTE as doc_client,
                        A.NB_ENDERECO_CLIENTE as address_client,
                        A.NB_NUMERO_RUA_CLIENTE as street_number_client,
                        A.NB_COMPLEMENTO_END_CLIENTE as complement_address_client,
                        A.NB_BAIRRO_CLIENTE as neighborhood_client,
                        A.NB_CIDADE as city_client,
                        A.NB_ESTADO as uf_client,
                        TRIM(REPLACE(A.NB_CEP, '-', '')) as zip_code_client,
                        A.NB_FONE_CLIENTE as fone_client,
                        
                        A.NB_TRANSPORTADORA as cod_shippingCompany,
                        A.NB_METODO_TRANSPORTADORA as metodo_shippingCompany,
                        A.NB_RAZAO_TRANSPORTADORA as reason_shippingCompany,
                        A.NB_NOME_TRANSPORTADORA as name_shippingCompany,
                        A.NB_DOC_TRANSPORTADORA as doc_shippingCompany,
                        A.NB_EMAIL_TRANSPORTADORA as email_shippingCompany,
                        A.NB_ENDERECO_TRANSPORTADORA as address_shippingCompany,
                        A.NB_NUMERO_RUA_TRANSPORTADORA as street_number_shippingCompany,
                        A.NB_COMPLEMENTO_END_TRANSPORTADORA as complement_address_shippingCompany,
                        A.NB_BAIRRO_TRANSPORTADORA as neighborhood_shippingCompany,
                        A.NB_CIDADE_TRANSPORTADORA as city_shippingCompany,
                        A.NB_UF_TRANSPORTADORA as uf_shippingCompany,
                        A.NB_CEP_TRANSPORTADORA as zip_code_shippingCompany,
                        A.NB_FONE_TRANSPORTADORA as fone_shippingCompany,
                        A.NB_INSCRICAO_ESTADUAL_TRANSPORTADORA as state_registration_shippingCompany,

                        A.NB_VALOR_PEDIDO as amount_nf,

                        TRIM(B.CODIGO_BARRA) as cod_product,
						B.QTDE as quantity_product,
						B.NB_SKU_PRODUTO as sku_product,
						B.DESCRICAO as description_product,
						TRIM(B.CODIGO_BARRA) as cod_ean_product,
						B.NB_VALOR_UNITARIO_PRODUTO as unitary_value_product,
						B.NB_VALOR_TOTAL_PRODUTO as amount_product,
						B.NB_VALOR_FRETE_PRODUTO as shipping_value_product,
                        C.IMG1 as urlImg

                        FROM GENERAL..IT4_WMS_DOCUMENTO A
						JOIN GENERAL..IT4_WMS_DOCUMENTO_ITEM B ON A.IDCONTROLE = B.IDCONTROLE
						LEFT JOIN BLOOMERS_OPERACOES..CANAL_MOVIMENTACAO C ON B.CODIGO_BARRA = C.COD_PRODUTO AND C.PEDIDO = A.DOCUMENTO
                        WHERE
                        --A.DOCUMENTO = ''
                        A.SERIE = '{serie_pedido}'
                        AND A.NB_DOC_REMETENTE = '{cnpj_emp}'
                        AND A.DATA >= '{data_inicial} 00:00:00'
                        AND A.DATA <= '{data_final} 23:59:59'
                        AND A.CANCELADO IS NULL";

            try
            {
                var result = await _conn.GetDbConnection().QueryAsync<Order, Client, ShippingCompany, Invoice, Product, Order>(sql, (pedido, cliente, transportadora, nota_fiscal, produto) =>
                {
                    pedido.client = cliente;
                    pedido.shippingCompany = transportadora;
                    pedido.invoice = nota_fiscal;
                    pedido.itens.Add(produto);
                    return pedido;
                }, splitOn: "cod_client, cod_shippingCompany, amount_nf, cod_product");

                var pedidos = result.GroupBy(p => p.number).Select(g =>
                {
                    var groupedOrder = g.First();
                    groupedOrder.itens = g.Select(p => p.itens.Single()).ToList();
                    return groupedOrder;
                });

                return pedidos.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [Confere Pedido] - GetPedidosNaoConferidos - Erro ao obter pedidos na tabela IT4_WMS_DOCUMENTO  - {ex.Message}");
            }
        }

        public async Task<int> UpdateRetorno(string nr_pedido, int volumes, string listProdutos)
        {
            var listaDeProdutos = JsonConvert.DeserializeObject<List<Product>>(listProdutos);

            var sql = $@"UPDATE GENERAL..IT4_WMS_DOCUMENTO SET 
                         VOLUMES = {volumes},
                         RETORNO = GETDATE()
                         WHERE
                         DOCUMENTO = '{nr_pedido}'";

            foreach (var produto in listaDeProdutos)
            {
                sql += $@"UPDATE A SET
                          A.QTDERETORNO = {produto.picked_quantity}
                          FROM GENERAL..IT4_WMS_DOCUMENTO_ITEM A
                          JOIN GENERAL..IT4_WMS_DOCUMENTO B ON A.IDCONTROLE = B.IDCONTROLE
                          WHERE B.DOCUMENTO = '{nr_pedido}' AND A.CODIGO_BARRA = {produto.cod_product}";
            }

            try
            {
                return await _conn.GetDbConnection().ExecuteAsync(sql);
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [Confere Pedido] - UpdateReturnIT4_WMS_DOCUMENTO - Erro ao atualizar retorno do pedido: {nr_pedido} na tabela IT4_WMS_DOCUMENTO  - {ex.Message}");
            }
        }

        public async Task<int> UpdateShippingCompany(string nr_pedido, int cod_transportador)
        {
            var sql = $@"UPDATE A SET
                        NB_TRANSPORTADORA = {cod_transportador},
                        NB_METODO_TRANSPORTADORA = 'ESTD',
                        NB_RAZAO_TRANSPORTADORA = B.RAZAO_CLIENTE,
                        NB_NOME_TRANSPORTADORA = B.NOME_CLIENTE,
                        NB_DOC_TRANSPORTADORA = B.DOC_CLIENTE,
                        NB_EMAIL_TRANSPORTADORA = B.EMAIL_CLIENTE,
                        NB_ENDERECO_TRANSPORTADORA = B.ENDERECO_CLIENTE,
                        NB_NUMERO_RUA_TRANSPORTADORA = B.NUMERO_RUA_CLIENTE,
                        NB_COMPLEMENTO_END_TRANSPORTADORA = B.COMPLEMENT_END_CLI,
                        NB_BAIRRO_TRANSPORTADORA = B.BAIRRO_CLIENTE,
                        NB_CIDADE_TRANSPORTADORA = B.CIDADE_CLIENTE,
                        NB_CEP_TRANSPORTADORA = B.CEP_CLIENTE,
                        NB_UF_TRANSPORTADORA = B.UF_CLIENTE,
                        NB_FONE_TRANSPORTADORA = B.FONE_CLIENTE,
                        NB_INSCRICAO_ESTADUAL_TRANSPORTADORA = B.INSCRICAO_ESTADUAL
                        FROM 
                             GENERAL..IT4_WMS_DOCUMENTO A
                        INNER JOIN 
                            BLOOMERS_LINX..LINXCLIENTESFORNEC_TRUSTED B ON B.COD_CLIENTE = {cod_transportador}
                        WHERE 
                            UPPER(TRIM(A.DOCUMENTO)) = ('{nr_pedido.ToUpper()}')";
            try
            {
                return await _conn.GetDbConnection().ExecuteAsync(sql);
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [Picking] - UpdateShippingCompany - Erro ao atualizar informacoes da transportadora do pedido: {nr_pedido} na tabela IT4_WMS_DOCUMENTO  - {ex.Message}");
            }
        }
    }
}
