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
                        B.QTDERETORNO as picked_quantity,
						B.NB_SKU_PRODUTO as sku_product,
						B.DESCRICAO as description_product,
						TRIM(B.CODIGO_BARRA) as cod_ean_product,
						B.NB_VALOR_UNITARIO_PRODUTO as unitary_value_product,
						B.NB_VALOR_TOTAL_PRODUTO as amount_product,
						B.NB_VALOR_FRETE_PRODUTO as shipping_value_product,
                        IIF(C.IMG1 IS NULL, '', C.IMG1) as urlImg

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
                        B.QTDERETORNO as picked_quantity,
						B.NB_SKU_PRODUTO as sku_product,
						B.DESCRICAO as description_product,
						TRIM(B.CODIGO_BARRA) as cod_ean_product,
						B.NB_VALOR_UNITARIO_PRODUTO as unitary_value_product,
						B.NB_VALOR_TOTAL_PRODUTO as amount_product,
						B.NB_VALOR_FRETE_PRODUTO as shipping_value_product,
                        IIF(C.IMG1 IS NULL, '', C.IMG1) as urlImg

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

        public async Task<Order?> GetUnpickedOrderToPrint(string cnpj_emp, string serie, string nr_pedido)
        {
            string sql = $@"SELECT DISTINCT
                         A.DOCUMENTO AS NUMBER,
                         A.NB_CFOP_PEDIDO AS CFOP,
                         C.[1] AS OBS,
                         D.NOME_VENDEDOR AS SELLER,
                         A.NB_VALOR_PEDIDO AS AMOUNT,

                         A.NB_NOME_REMETENTE AS NAME_COMPANY,
                         A.NB_ENDERECO_REMETENTE AS ADDRESS_COMPANY,
                         A.NB_NUMERO_RUA_REMETENTE AS STREET_NUMBER_COMPANY,
                         A.NB_COMPLEMENTO_END_REMETENTE AS COMPLEMENT_ADDRESS_COMPANY,
                         A.NB_BAIRRO_REMETENTE AS NEIGHBORHOOD_COMPANY,
                         A.NB_CEP_REMETENTE AS ZIP_CODE_COMPANY,
                         A.NB_CIDADE_REMETENTE AS CITY_COMPANY,
                         A.NB_UF_REMETENTE AS UF_COMPANY,
                         A.NB_FONE_REMETENTE AS FONE_COMPANY,
                         A.NB_DOC_REMETENTE AS DOC_COMPANY,
                         A.NB_INSCRICAO_ESTADUAL_REMETENTE AS STATE_REGISTRATION_COMPANY,
                         
                         A.NB_RAZAO_CLIENTE AS REASON_CLIENT,
                         A.NB_CODIGO_CLIENTE AS COD_CLIENT,
                         A.NB_ENDERECO_CLIENTE AS ADDRESS_CLIENT,
                         A.NB_BAIRRO_CLIENTE AS NEIGHBORHOOD_CLIENT,
                         A.NB_NUMERO_RUA_CLIENTE AS STREET_NUMBER_CLIENT,
                         A.NB_COMPLEMENTO_END_CLIENTE AS COMPLEMENT_ADDRESS_CLIENT,
                         A.NB_CEP AS ZIP_CODE_CLIENT,
                         A.NB_EMAIL_CLIENTE AS EMAIL_CLIENT,
                         A.NB_DOC_CLIENTE AS DOC_CLIENT,
                         A.NB_INSCRICAO_ESTADUAL_CLIENTE AS STATE_REGISTRATION_CLIENT,
                         
                         A.NB_TRANSPORTADORA AS COD_SHIPPINGCOMPANY,
                         A.NB_RAZAO_TRANSPORTADORA AS REASON_SHIPPINGCOMPANY,

                         B.IDITEM AS IDITEM,
                         B.CODIGO_BARRA AS COD_PRODUCT,
                         B.DESCRICAO AS DESCRIPTION_PRODUCT,
                         B.NB_SKU_PRODUTO AS SKU_PRODUCT,
                         B.QTDE AS QUANTITY_PRODUCT,
                         B.NB_VALOR_UNITARIO_PRODUTO AS UNITARY_VALUE_PRODUCT,
                         B.NB_VALOR_TOTAL_PRODUTO AS AMOUNT_PRODUCT
                         
                         [0]";

            if (nr_pedido.Contains("-VD"))
            {
                sql = sql.Replace("[1]", "OBS").Replace("[0]", $@"FROM GENERAL..IT4_WMS_DOCUMENTO A (NOLOCK)
                                      JOIN GENERAL..IT4_WMS_DOCUMENTO_ITEM B (NOLOCK) ON A.IDCONTROLE = B.IDCONTROLE
                                      JOIN BLOOMERS_LINX..LINXPEDIDOSVENDA_TRUSTED C (NOLOCK) ON C.COD_PEDIDO = REPLACE(A.DOCUMENTO, 'MI-VD', '') AND C.CNPJ_EMP = A.NB_DOC_REMETENTE
                                      JOIN BLOOMERS_LINX..LINXVENDEDORES_TRUSTED D (NOLOCK) ON C.COD_VENDEDOR = D.COD_VENDEDOR
                                      WHERE 
                                      A.DOCUMENTO = '{nr_pedido}'
                                      AND A.NB_DOC_REMETENTE = '{cnpj_emp}'
                                      AND A.SERIE = '{serie}'");
            }
            else if (nr_pedido.Contains("-VF"))
            {
                sql = sql.Replace("[1]", "OBS").Replace("[0]", $@"FROM GENERAL..IT4_WMS_DOCUMENTO A (NOLOCK)
                                      JOIN GENERAL..IT4_WMS_DOCUMENTO_ITEM B (NOLOCK) ON A.IDCONTROLE = B.IDCONTROLE
                                      JOIN BLOOMERS_LINX..LINXMOVIMENTO_TRUSTED C (NOLOCK) ON C.DOCUMENTO = REPLACE(RIGHT(A.DOCUMENTO, 4), 'MI-VF', '') AND C.CNPJ_EMP = A.NB_DOC_REMETENTE
                                      JOIN BLOOMERS_LINX..LINXVENDEDORES_TRUSTED D (NOLOCK) ON C.COD_VENDEDOR = D.COD_VENDEDOR
                                      WHERE 
                                      (C.CODIGO_ROTINA_ORIGEM = 12 OR C.CODIGO_ROTINA_ORIGEM = 16) AND
						              A.DOCUMENTO = '{nr_pedido}'
                                      AND A.NB_DOC_REMETENTE = '{cnpj_emp}'
                                      AND A.SERIE = '{serie}'");
            }
            else
            {
                sql = sql.Replace("[1]", "ANOTACAO").Replace("[0]", $@"FROM GENERAL..IT4_WMS_DOCUMENTO A (NOLOCK)
                                      JOIN GENERAL..IT4_WMS_DOCUMENTO_ITEM B (NOLOCK) ON A.IDCONTROLE = B.IDCONTROLE
                                      JOIN BLOOMERS_LINX..B2CCONSULTAPEDIDOS_TRUSTED C (NOLOCK) ON C.ORDER_ID = A.DOCUMENTO AND C.EMPRESA = A.NB_COD_REMETENTE
                                      JOIN BLOOMERS_LINX..LINXVENDEDORES_TRUSTED D (NOLOCK) ON C.COD_VENDEDOR = D.COD_VENDEDOR
                                      WHERE 
                                      A.DOCUMENTO = '{nr_pedido}'
                                      AND A.NB_DOC_REMETENTE = '{cnpj_emp}'
                                      AND A.SERIE = '{serie}'");
            }

            try
            {
                var result = await _conn.GetDbConnection().QueryAsync<Order, Company, Client, ShippingCompany, Product, Order>(sql, (pedido, empresa, cliente, transportadora, produto) =>
                {
                    pedido.client = cliente;
                    pedido.company = empresa;
                    pedido.client = cliente;
                    pedido.shippingCompany = transportadora;
                    pedido.itens.Add(produto);
                    return pedido;
                }, splitOn: "name_company, reason_client, cod_shippingcompany, iditem");

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

        public async Task<List<Order>?> GetUnpickedOrdersToPrint(string cnpj_emp, string serie_pedido, string data_inicial, string data_final)
        {
            throw new NotImplementedException();
            //var sql = $@"";

            //try
            //{
            //    var result = await _conn.GetDbConnection().QueryAsync<Order, Client, ShippingCompany, Invoice, Product, Order>(sql, (pedido, cliente, transportadora, nota_fiscal, produto) =>
            //    {
            //        pedido.client = cliente;
            //        pedido.shippingCompany = transportadora;
            //        pedido.invoice = nota_fiscal;
            //        pedido.itens.Add(produto);
            //        return pedido;
            //    }, splitOn: "cod_client, cod_shippingCompany, amount_nf, cod_product");

            //    var pedidos = result.GroupBy(p => p.number).Select(g =>
            //    {
            //        var groupedOrder = g.First();
            //        groupedOrder.itens = g.Select(p => p.itens.Single()).ToList();
            //        return groupedOrder;
            //    });

            //    return pedidos.ToList();
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception($"MiniWms [Confere Pedido] - GetPedidosNaoConferidos - Erro ao obter pedidos na tabela IT4_WMS_DOCUMENTO  - {ex.Message}");
            //}
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
