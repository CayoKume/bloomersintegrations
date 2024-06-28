using BloomersIntegrationsCore.Domain.Entities;
using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using BloomersMiniWmsIntegrations.Domain.Entities.Picking;
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
                         COD_CLIENTE AS COD_SHIPPINGCOMPANY,
                         DOC_CLIENTE AS DOC_SHIPPINGCOMPANY,
                         RAZAO_CLIENTE AS REASON_SHIPPINGCOMPANY,
                         NOME_CLIENTE AS NAME_SHIPPINGCOMPANY,
                         EMAIL_CLIENTE AS EMAIL_SHIPPINGCOMPANY,
                         ENDERECO_CLIENTE AS ADDRESS_SHIPPINGCOMPANY,
                         NUMERO_RUA_CLIENTE AS STREET_NUMBER_SHIPPINGCOMPANY,
                         COMPLEMENT_END_CLI AS COMPLEMENT_ADDRESS_SHIPPINGCOMPANY,
                         BAIRRO_CLIENTE AS NEIGHBORHOOD_SHIPPINGCOMPANY,
                         CIDADE_CLIENTE AS CITY_SHIPPINGCOMPANY,
                         UF_CLIENTE AS UF_SHIPPINGCOMPANY,
                         CEP_CLIENTE AS ZIP_CODE_SHIPPINGCOMPANY,
                         FONE_CLIENTE AS FONE_SHIPPINGCOMPANY,
                         INSCRICAO_ESTADUAL AS STATE_REGISTRATION_SHIPPINGCOMPANY,
                         INCRICAO_MUNICIPAL AS MUNICIPAL_REGISTRATION_SHIPPINGCOMPANY
                         FROM BLOOMERS_LINX..LINXCLIENTESFORNEC_TRUSTED (NOLOCK)
                         WHERE TIPO_CADASTRO = 'T'
                         ORDER BY COD_CLIENTE";

            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    var result = await conn.QueryAsync<ShippingCompany>(sql);
                    return result.ToList(); 
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [Picking] - GetShippingCompanys - Erro ao obter transportadoras da tabela LinxClientesFornec_trusted  - {ex.Message}");
            }
        }

        public async Task<Order?> GetUnpickedOrder(string cnpj_emp, string serie, string nr_pedido)
        {
            var sql = $@"SELECT DISTINCT
                        TRIM(A.DOCUMENTO) AS NUMBER,
                        A.RETORNO AS RETORNO,
                        A.VOLUMES AS VOLUMES,

                        A.NB_CODIGO_CLIENTE AS COD_CLIENT,
                        A.NB_RAZAO_CLIENTE AS REASON_CLIENT,
                        A.NB_DOC_CLIENTE AS DOC_CLIENT,
                        A.NB_ENDERECO_CLIENTE AS ADDRESS_CLIENT,
                        A.NB_NUMERO_RUA_CLIENTE AS STREET_NUMBER_CLIENT,
                        A.NB_COMPLEMENTO_END_CLIENTE AS COMPLEMENT_ADDRESS_CLIENT,
                        A.NB_BAIRRO_CLIENTE AS NEIGHBORHOOD_CLIENT,
                        A.NB_CIDADE AS CITY_CLIENT,
                        A.NB_ESTADO AS UF_CLIENT,
                        TRIM(REPLACE(A.NB_CEP, '-', '')) AS ZIP_CODE_CLIENT,
                        A.NB_FONE_CLIENTE AS FONE_CLIENT,

                        A.NB_TRANSPORTADORA AS COD_SHIPPINGCOMPANY,
                        A.NB_METODO_TRANSPORTADORA AS METODO_SHIPPINGCOMPANY,
                        A.NB_RAZAO_TRANSPORTADORA AS REASON_SHIPPINGCOMPANY,
                        A.NB_NOME_TRANSPORTADORA AS NAME_SHIPPINGCOMPANY,
                        A.NB_DOC_TRANSPORTADORA AS DOC_SHIPPINGCOMPANY,
                        A.NB_EMAIL_TRANSPORTADORA AS EMAIL_SHIPPINGCOMPANY,
                        A.NB_ENDERECO_TRANSPORTADORA AS ADDRESS_SHIPPINGCOMPANY,
                        A.NB_NUMERO_RUA_TRANSPORTADORA AS STREET_NUMBER_SHIPPINGCOMPANY,
                        A.NB_COMPLEMENTO_END_TRANSPORTADORA AS COMPLEMENT_ADDRESS_SHIPPINGCOMPANY,
                        A.NB_BAIRRO_TRANSPORTADORA AS NEIGHBORHOOD_SHIPPINGCOMPANY,
                        A.NB_CIDADE_TRANSPORTADORA AS CITY_SHIPPINGCOMPANY,
                        A.NB_UF_TRANSPORTADORA AS UF_SHIPPINGCOMPANY,
                        A.NB_CEP_TRANSPORTADORA AS ZIP_CODE_SHIPPINGCOMPANY,
                        A.NB_FONE_TRANSPORTADORA AS FONE_SHIPPINGCOMPANY,
                        A.NB_INSCRICAO_ESTADUAL_TRANSPORTADORA AS STATE_REGISTRATION_SHIPPINGCOMPANY,
                              
                        A.NB_VALOR_PEDIDO AS AMOUNT_NF,

                        TRIM(B.CODIGO_BARRA) AS COD_PRODUCT,
						B.QTDE AS QUANTITY_PRODUCT,
                        B.QTDERETORNO AS PICKED_QUANTITY,
						B.NB_SKU_PRODUTO AS SKU_PRODUCT,
						B.DESCRICAO AS DESCRIPTION_PRODUCT,
						TRIM(B.CODIGO_BARRA) AS COD_EAN_PRODUCT,
						B.NB_VALOR_UNITARIO_PRODUTO AS UNITARY_VALUE_PRODUCT,
						B.NB_VALOR_TOTAL_PRODUTO AS AMOUNT_PRODUCT,
						B.NB_VALOR_FRETE_PRODUTO AS SHIPPING_VALUE_PRODUCT,
                        IIF(C.IMG1 IS NULL, '', C.IMG1) AS URLIMG

                        FROM GENERAL..IT4_WMS_DOCUMENTO A (NOLOCK)
                        JOIN GENERAL..IT4_WMS_DOCUMENTO_ITEM B (NOLOCK) ON A.IDCONTROLE = B.IDCONTROLE
						LEFT JOIN BLOOMERS_OPERACOES..CANAL_MOVIMENTACAO C (NOLOCK) ON B.CODIGO_BARRA = C.COD_PRODUTO AND C.PEDIDO = A.DOCUMENTO
                        WHERE
                        A.SERIE = '{serie}'
                        AND A.NB_DOC_REMETENTE = '{cnpj_emp}' 
						AND A.DOCUMENTO = '{nr_pedido}'
                        AND A.CANCELADO IS NULL";

            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    var result = await conn.QueryAsync<Order, Client, ShippingCompany, Invoice, Product, Order>(sql, (pedido, cliente, transportadora, nota_fiscal, produto) =>
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
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [Picking] - GetUnpickedOrder - Erro ao obter pedido: {nr_pedido} na tabela IT4_WMS_DOCUMENTO  - {ex.Message}");
            }
        }

        public async Task<List<Order>?> GetUnpickedOrders(string cnpj_emp, string serie_pedido, string data_inicial, string data_final)
        {
            var sql = $@"SELECT DISTINCT
                        TRIM(A.DOCUMENTO) AS NUMBER,
                        A.RETORNO AS RETORNO,
                        A.VOLUMES AS VOLUMES,

                        A.NB_CODIGO_CLIENTE AS COD_CLIENT,
                        A.NB_RAZAO_CLIENTE AS REASON_CLIENT,
                        A.NB_DOC_CLIENTE AS DOC_CLIENT,
                        A.NB_ENDERECO_CLIENTE AS ADDRESS_CLIENT,
                        A.NB_NUMERO_RUA_CLIENTE AS STREET_NUMBER_CLIENT,
                        A.NB_COMPLEMENTO_END_CLIENTE AS COMPLEMENT_ADDRESS_CLIENT,
                        A.NB_BAIRRO_CLIENTE AS NEIGHBORHOOD_CLIENT,
                        A.NB_CIDADE AS CITY_CLIENT,
                        A.NB_ESTADO AS UF_CLIENT,
                        TRIM(REPLACE(A.NB_CEP, '-', '')) AS ZIP_CODE_CLIENT,
                        A.NB_FONE_CLIENTE AS FONE_CLIENT,

                        A.NB_TRANSPORTADORA AS COD_SHIPPINGCOMPANY,
                        A.NB_METODO_TRANSPORTADORA AS METODO_SHIPPINGCOMPANY,
                        A.NB_RAZAO_TRANSPORTADORA AS REASON_SHIPPINGCOMPANY,
                        A.NB_NOME_TRANSPORTADORA AS NAME_SHIPPINGCOMPANY,
                        A.NB_DOC_TRANSPORTADORA AS DOC_SHIPPINGCOMPANY,
                        A.NB_EMAIL_TRANSPORTADORA AS EMAIL_SHIPPINGCOMPANY,
                        A.NB_ENDERECO_TRANSPORTADORA AS ADDRESS_SHIPPINGCOMPANY,
                        A.NB_NUMERO_RUA_TRANSPORTADORA AS STREET_NUMBER_SHIPPINGCOMPANY,
                        A.NB_COMPLEMENTO_END_TRANSPORTADORA AS COMPLEMENT_ADDRESS_SHIPPINGCOMPANY,
                        A.NB_BAIRRO_TRANSPORTADORA AS NEIGHBORHOOD_SHIPPINGCOMPANY,
                        A.NB_CIDADE_TRANSPORTADORA AS CITY_SHIPPINGCOMPANY,
                        A.NB_UF_TRANSPORTADORA AS UF_SHIPPINGCOMPANY,
                        A.NB_CEP_TRANSPORTADORA AS ZIP_CODE_SHIPPINGCOMPANY,
                        A.NB_FONE_TRANSPORTADORA AS FONE_SHIPPINGCOMPANY,
                        A.NB_INSCRICAO_ESTADUAL_TRANSPORTADORA AS STATE_REGISTRATION_SHIPPINGCOMPANY,
                              
                        A.NB_VALOR_PEDIDO AS AMOUNT_NF,

                        TRIM(B.CODIGO_BARRA) AS COD_PRODUCT,
						B.QTDE AS QUANTITY_PRODUCT,
                        B.QTDERETORNO AS PICKED_QUANTITY,
						B.NB_SKU_PRODUTO AS SKU_PRODUCT,
						B.DESCRICAO AS DESCRIPTION_PRODUCT,
						TRIM(B.CODIGO_BARRA) AS COD_EAN_PRODUCT,
						B.NB_VALOR_UNITARIO_PRODUTO AS UNITARY_VALUE_PRODUCT,
						B.NB_VALOR_TOTAL_PRODUTO AS AMOUNT_PRODUCT,
						B.NB_VALOR_FRETE_PRODUTO AS SHIPPING_VALUE_PRODUCT,
                        IIF(C.IMG1 IS NULL, '', C.IMG1) AS URLIMG

                        FROM GENERAL..IT4_WMS_DOCUMENTO A (NOLOCK)
						JOIN GENERAL..IT4_WMS_DOCUMENTO_ITEM B (NOLOCK) ON A.IDCONTROLE = B.IDCONTROLE
						LEFT JOIN BLOOMERS_OPERACOES..CANAL_MOVIMENTACAO C (NOLOCK) ON B.CODIGO_BARRA = C.COD_PRODUTO AND C.PEDIDO = A.DOCUMENTO
                        WHERE
                        --A.DOCUMENTO = ''
                        A.SERIE = '{serie_pedido}'
                        AND A.NB_DOC_REMETENTE = '{cnpj_emp}'
                        AND A.DATA >= '{data_inicial} 00:00:00'
                        AND A.DATA <= '{data_final} 23:59:59'
                        AND A.CANCELADO IS NULL";

            try
            {
                using (var conn = _conn.GetIDbConnection())
                {
                    var result = await conn.QueryAsync<Order, Client, ShippingCompany, Invoice, Product, Order>(sql, (pedido, cliente, transportadora, nota_fiscal, produto) =>
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
                         IIF(A.NB_PARA_PRESENTE = 'S', 'SIM', 'NÃO') AS PRESENT,
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

            if (nr_pedido.Contains("-VD") || nr_pedido.Contains("-LJ"))
            {
                sql = sql.Replace("[1]", "OBS").Replace("[0]", $@"FROM GENERAL..IT4_WMS_DOCUMENTO A (NOLOCK)
                                      JOIN GENERAL..IT4_WMS_DOCUMENTO_ITEM B (NOLOCK) ON A.IDCONTROLE = B.IDCONTROLE
                                      JOIN BLOOMERS_LINX..LINXPEDIDOSVENDA_TRUSTED C (NOLOCK) ON C.COD_PEDIDO = REPLACE(REPLACE(A.DOCUMENTO, 'MI-VD', ''), 'MI-LJ', '') AND C.CNPJ_EMP = A.NB_DOC_REMETENTE
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
                using (var conn = _conn.GetIDbConnection())
                {
                    var result = await conn.QueryAsync<Order, Company, Client, ShippingCompany, ProductToPrint, Order>(sql, (pedido, empresa, cliente, transportadora, produto) =>
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
                using (var conn = _conn.GetIDbConnection())
                {
                    return await conn.ExecuteAsync(sql); 
                }
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
                using (var conn = _conn.GetIDbConnection())
                {
                    return await conn.ExecuteAsync(sql); 
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [Picking] - UpdateShippingCompany - Erro ao atualizar informacoes da transportadora do pedido: {nr_pedido} na tabela IT4_WMS_DOCUMENTO  - {ex.Message}");
            }
        }
    }
}
