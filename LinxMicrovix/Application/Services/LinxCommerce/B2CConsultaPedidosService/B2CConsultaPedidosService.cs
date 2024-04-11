using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;
using BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxCommerce;
using BloomersMicrovixIntegrations.LinxMicrovix.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovix.Domain.Extensions;
using BloomersMicrovixIntegrations.LinxMicrovix.Infrastructure.Apis;
using System.Globalization;

namespace BloomersMicrovixIntegrations.Application.Services.LinxCommerce
{
    public class B2CConsultaPedidosService<TEntity> : IB2CConsultaPedidosService<TEntity> where TEntity : B2CConsultaPedidos, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveB2C.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationB2C.ToName();
        private readonly IAPICall _apiCall;
        private readonly IB2CConsultaPedidosRepository _b2CConsultaPedidosRepository;

        public B2CConsultaPedidosService(IB2CConsultaPedidosRepository b2CConsultaPedidosRepository, IAPICall apiCall) =>
            (_b2CConsultaPedidosRepository, _apiCall) = (b2CConsultaPedidosRepository, apiCall);

        public List<TEntity?> DeserializeResponse(List<Dictionary<string, string>> registros)
        {
            long timestamp;
            int id_pedido, cod_cliente_erp, cod_cliente_b2c, forma_pgto, plano_pagamento, tipo_frete, id_status, cod_transportador, tipo_cobranca_frete, empresa, id_tabela_preco, cod_vendedor, portal, id_tipo_b2c;
            DateTime dt_pedido, dt_insert, dt_disponivel_faturamento;
            bool finalizado, ativo;
            decimal vl_frete, taxa_impressao, valor_frete_gratis, valor_credito;

            var list = new List<TEntity>();

            for (int i = 0; i < registros.Count(); i++)
            {
                try
                {
                    if (long.TryParse(registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First(), out long result))
                        timestamp = result;
                    else
                        timestamp = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "id_pedido").Select(pair => pair.Value).First(), out int result_0))
                        id_pedido = result_0;
                    else
                        id_pedido = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "cod_cliente_erp").Select(pair => pair.Value).First(), out int result_1))
                        cod_cliente_erp = result_1;
                    else
                        cod_cliente_erp = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "cod_cliente_b2c").Select(pair => pair.Value).First(), out int result_2))
                        cod_cliente_b2c = result_2;
                    else
                        cod_cliente_b2c = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "forma_pgto").Select(pair => pair.Value).First(), out int result_3))
                        forma_pgto = result_3;
                    else
                        forma_pgto = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "plano_pagamento").Select(pair => pair.Value).First(), out int result_4))
                        plano_pagamento = result_4;
                    else
                        plano_pagamento = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "tipo_frete").Select(pair => pair.Value).First(), out int result_5))
                        tipo_frete = result_5;
                    else
                        tipo_frete = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "id_status").Select(pair => pair.Value).First(), out int result_6))
                        id_status = result_6;
                    else
                        id_status = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "cod_transportador").Select(pair => pair.Value).First(), out int result_7))
                        cod_transportador = result_7;
                    else
                        cod_transportador = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "tipo_cobranca_frete").Select(pair => pair.Value).First(), out int result_8))
                        tipo_cobranca_frete = result_8;
                    else
                        tipo_cobranca_frete = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "empresa").Select(pair => pair.Value).First(), out int result_9))
                        empresa = result_9;
                    else
                        empresa = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "id_tabela_preco").Select(pair => pair.Value).First(), out int result_10))
                        id_tabela_preco = result_10;
                    else
                        id_tabela_preco = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "cod_vendedor").Select(pair => pair.Value).First(), out int result_11))
                        cod_vendedor = result_11;
                    else
                        cod_vendedor = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First(), out int result_12))
                        portal = result_12;
                    else
                        portal = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "id_tipo_b2c").Select(pair => pair.Value).First(), out int result_13))
                        id_tipo_b2c = result_13;
                    else
                        id_tipo_b2c = 0;

                    if (DateTime.TryParse(registros[i].Where(pair => pair.Key == "dt_pedido").Select(pair => pair.Value).First(), out DateTime result_14))
                        dt_pedido = result_14;
                    else
                        dt_pedido = new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar);

                    if (DateTime.TryParse(registros[i].Where(pair => pair.Key == "dt_insert").Select(pair => pair.Value).First(), out DateTime result_15))
                        dt_insert = result_15;
                    else
                        dt_insert = new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar);

                    if (DateTime.TryParse(registros[i].Where(pair => pair.Key == "dt_disponivel_faturamento").Select(pair => pair.Value).First(), out DateTime result_16))
                        dt_disponivel_faturamento = result_16;
                    else
                        dt_disponivel_faturamento = new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar);

                    if (bool.TryParse(registros[i].Where(pair => pair.Key == "finalizado").Select(pair => pair.Value).First(), out bool result_17))
                        finalizado = result_17;
                    else
                        finalizado = false;

                    if (bool.TryParse(registros[i].Where(pair => pair.Key == "ativo").Select(pair => pair.Value).First(), out bool result_18))
                        ativo = result_18;
                    else
                        ativo = false;

                    if (decimal.TryParse(registros[i].Where(pair => pair.Key == "vl_frete").Select(pair => pair.Value).First(), out decimal result_19))
                        vl_frete = result_19;
                    else
                        vl_frete = 0;

                    if (decimal.TryParse(registros[i].Where(pair => pair.Key == "taxa_impressao").Select(pair => pair.Value).First(), out decimal result_20))
                        taxa_impressao = result_20;
                    else
                        taxa_impressao = 0;

                    if (decimal.TryParse(registros[i].Where(pair => pair.Key == "valor_frete_gratis").Select(pair => pair.Value).First(), out decimal result_21))
                        valor_frete_gratis = result_21;
                    else
                        valor_frete_gratis = 0;

                    if (decimal.TryParse(registros[i].Where(pair => pair.Key == "valor_credito").Select(pair => pair.Value).First(), out decimal result_22))
                        valor_credito = result_22;
                    else
                        valor_credito = 0;

                    list.Add(new TEntity
                    {
                        lastupdateon = DateTime.Now,
                        id_pedido = id_pedido,
                        dt_pedido = dt_pedido,
                        cod_cliente_erp = cod_cliente_erp,
                        cod_cliente_b2c = cod_cliente_b2c,
                        vl_frete = vl_frete,
                        forma_pgto = forma_pgto,
                        plano_pagamento = plano_pagamento,
                        anotacao = registros[i].Where(pair => pair.Key == "anotacao").Select(pair => pair.Value).First(),
                        taxa_impressao = taxa_impressao,
                        finalizado = finalizado,
                        valor_frete_gratis = valor_frete_gratis,
                        tipo_frete = tipo_frete,
                        id_status = id_status,
                        cod_transportador = cod_transportador,
                        tipo_cobranca_frete = tipo_cobranca_frete,
                        ativo = ativo,
                        empresa = empresa,
                        id_tabela_preco = id_tabela_preco,
                        valor_credito = valor_credito,
                        cod_vendedor = cod_vendedor,
                        timestamp = timestamp,
                        dt_insert = dt_insert,
                        dt_disponivel_faturamento = dt_disponivel_faturamento,
                        portal = portal,
                        mensagem_falha_faturamento = registros[i].Where(pair => pair.Key == "mensagem_falha_faturamento").Select(pair => pair.Value).First(),
                        id_tipo_b2c = id_tipo_b2c,
                        ecommerce_origem = registros[i].Where(pair => pair.Key == "ecommerce_origem").Select(pair => pair.Value).First(),
                        order_id = registros[i].Where(pair => pair.Key == "order_id").Select(pair => pair.Value).First(),
                        fulfillment_id = registros[i].Where(pair => pair.Key == "fulfillment_id").Select(pair => pair.Value).First()
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = registros[i].Where(pair => pair.Key == "id_pedido").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_pedido").Select(pair => pair.Value).First();
                    throw new Exception($"B2CConsultaPedidos - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
                }
            }

            return list;
        }

        public async Task IntegraRegistrosAsync(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _b2CConsultaPedidosRepository.GetParametersAsync(tableName, database, "parameters_lastday");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    var _listResults = listResults.ConvertAll(new Converter<TEntity, B2CConsultaPedidos>(TEntityToObject));
                    var __listResults = await _b2CConsultaPedidosRepository.GetRegistersExistsAsync(_listResults, tableName, database);

                    for (int i = 0; i < __listResults.Count; i++)
                    {
                        _listResults.Remove(_listResults.Where(r => r.order_id == __listResults[i].order_id && r.timestamp == __listResults[i].timestamp).FirstOrDefault());
                    }

                    if (_listResults.Count() > 0)
                        _b2CConsultaPedidosRepository.BulkInsertIntoTableRaw(_listResults, tableName, database);
                }
            }
            catch
            {
                throw;
            }
        }

        public void IntegraRegistrosNotAsync(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = _b2CConsultaPedidosRepository.GetParametersNotAsync(tableName, database, "parameters_lastday");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);

                    if (listResults.Count() > 0)
                    {
                        var listPedidos = listResults.ConvertAll(new Converter<TEntity, B2CConsultaPedidos>(TEntityToObject));
                        _b2CConsultaPedidosRepository.BulkInsertIntoTableRaw(listPedidos, tableName, database);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> IntegraRegistrosIndividualAsync(string tableName, string procName, string database, string identificador)
        {
            try
            {
                PARAMETERS = await _b2CConsultaPedidosRepository.GetParametersAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[id_pedido]", $"{identificador}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    await _b2CConsultaPedidosRepository.InsereRegistroIndividualAsync(registro[0], tableName, database);
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                throw;
            }
        }

        public bool IntegraRegistrosIndividualNotAsync(string tableName, string procName, string database, string identificador)
        {
            try
            {
                PARAMETERS = _b2CConsultaPedidosRepository.GetParametersNotAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[id_pedido]", $"{identificador}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    _b2CConsultaPedidosRepository.InsereRegistroIndividualNotAsync(registro[0], tableName, database);
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                throw;
            }
        }

        public TEntity? TEntityToObject(TEntity t1)
        {
            try
            {
                return new TEntity
                {
                    lastupdateon = t1.lastupdateon,
                    id_pedido = t1.id_pedido,
                    dt_pedido = t1.dt_pedido,
                    cod_cliente_erp = t1.cod_cliente_erp,
                    cod_cliente_b2c = t1.cod_cliente_b2c,
                    vl_frete = t1.vl_frete,
                    forma_pgto = t1.forma_pgto,
                    plano_pagamento = t1.plano_pagamento,
                    anotacao = t1.anotacao,
                    taxa_impressao = t1.taxa_impressao,
                    finalizado = t1.finalizado,
                    valor_frete_gratis = t1.valor_frete_gratis,
                    tipo_frete = t1.tipo_frete,
                    id_status = t1.id_status,
                    cod_transportador = t1.cod_transportador,
                    tipo_cobranca_frete = t1.tipo_cobranca_frete,
                    ativo = t1.ativo,
                    empresa = t1.empresa,
                    id_tabela_preco = t1.id_tabela_preco,
                    valor_credito = t1.valor_credito,
                    cod_vendedor = t1.cod_vendedor,
                    timestamp = t1.timestamp,
                    dt_insert = t1.dt_insert,
                    dt_disponivel_faturamento = t1.dt_disponivel_faturamento,
                    portal = t1.portal,
                    mensagem_falha_faturamento = t1.mensagem_falha_faturamento,
                    id_tipo_b2c = t1.id_tipo_b2c,
                    ecommerce_origem = t1.ecommerce_origem,
                    order_id = t1.order_id,
                    fulfillment_id = t1.fulfillment_id,
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"B2CConsultaPedidos - TEntityToObject - Erro ao converter registro: {t1.id_pedido} para objeto - {ex.Message}");
            }
        }
    }
}
