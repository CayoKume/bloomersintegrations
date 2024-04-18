using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxEcommerce;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxCommerce;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Extensions;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Apis;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxCommerce
{
    public class B2CConsultaPedidosItensService<TEntity> : IB2CConsultaPedidosItensService<TEntity> where TEntity : B2CConsultaPedidosItens, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveB2C.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationB2C.ToName();
        private readonly IAPICall _apiCall;
        private readonly IB2CConsultaPedidosItensRepository _b2CConsultaPedidosItensRepository;

        public B2CConsultaPedidosItensService(IB2CConsultaPedidosItensRepository b2CConsultaPedidosItensRepository, IAPICall apiCall) =>
            (_b2CConsultaPedidosItensRepository, _apiCall ) = (b2CConsultaPedidosItensRepository, apiCall );

        public List<TEntity?> DeserializeResponse(List<Dictionary<string, string>> registros)
        {
            decimal vl_unitario;
            long timestamp, codigoproduto;
            int id_pedido_item, id_pedido, quantidade, portal;

            var list = new List<TEntity>();

            for (int i = 0; i < registros.Count(); i++)
            {
                try
                {
                    if (long.TryParse(registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First(), out long result))
                        timestamp = result;
                    else
                        timestamp = 0;

                    if (long.TryParse(registros[i].Where(pair => pair.Key == "codigoproduto").Select(pair => pair.Value).First(), out long result_0))
                        codigoproduto = result_0;
                    else
                        codigoproduto = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "id_pedido_item").Select(pair => pair.Value).First(), out int result_1))
                        id_pedido_item = result_1;
                    else
                        id_pedido_item = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "id_pedido").Select(pair => pair.Value).First(), out int result_2))
                        id_pedido = result_2;
                    else
                        id_pedido = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "quantidade").Select(pair => pair.Value).First(), out int result_3))
                        quantidade = result_3;
                    else
                        quantidade = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First(), out int result_4))
                        portal = result_4;
                    else
                        portal = 0;

                    if (decimal.TryParse(registros[i].Where(pair => pair.Key == "vl_unitario").Select(pair => pair.Value).First(), out decimal result_5))
                        vl_unitario = result_5;
                    else
                        vl_unitario = 0;

                    list.Add(new TEntity
                    {
                        lastupdateon = DateTime.Now,
                        id_pedido_item = id_pedido_item,
                        id_pedido = id_pedido,
                        codigoproduto = codigoproduto,
                        quantidade = quantidade,
                        vl_unitario = vl_unitario,
                        timestamp = timestamp,
                        portal = portal
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = registros[i].Where(pair => pair.Key == "id_pedido_item").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_pedido_item").Select(pair => pair.Value).First();
                    throw new Exception($"B2CConsultaPedidosItens - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
                }
            }

            return list;
        }

        public async Task IntegraRegistrosAsync(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _b2CConsultaPedidosItensRepository.GetParametersAsync(tableName, database, "parameters_lastday");

                var timestamp = await _b2CConsultaPedidosItensRepository.GetTableLastTimestampAsync(database, tableName);
                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", timestamp), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    var _listResults = listResults.ConvertAll(new Converter<TEntity, B2CConsultaPedidosItens>(TEntityToObject));
                    var __listResults = await _b2CConsultaPedidosItensRepository.GetRegistersExistsAsync(_listResults, tableName, database);

                    for (int i = 0; i < __listResults.Count; i++)
                    {
                        _listResults.Remove(_listResults.Where(r => r.id_pedido_item == Convert.ToInt64(__listResults[i].id_pedido_item) && r.timestamp == __listResults[i].timestamp).FirstOrDefault());
                    }

                    if (_listResults.Count() > 0)
                        _b2CConsultaPedidosItensRepository.BulkInsertIntoTableRaw(_listResults, tableName, database);
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
                PARAMETERS = _b2CConsultaPedidosItensRepository.GetParametersNotAsync(tableName, database, "parameters_lastday");

                var timestamp = _b2CConsultaPedidosItensRepository.GetTableLastTimestampNotAsync(database, tableName);
                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", timestamp), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    if (listResults.Count() > 0)
                    {
                        var list = listResults.ConvertAll(new Converter<TEntity, B2CConsultaPedidosItens>(TEntityToObject));
                        _b2CConsultaPedidosItensRepository.BulkInsertIntoTableRaw(list, tableName, database);
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
                PARAMETERS = await _b2CConsultaPedidosItensRepository.GetParametersAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[id_pedido]", $"{identificador}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    await _b2CConsultaPedidosItensRepository.InsereRegistroIndividualAsync(registro[0], tableName, database);
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
                PARAMETERS = _b2CConsultaPedidosItensRepository.GetParametersNotAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[id_pedido]", $"{identificador}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                string response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    _b2CConsultaPedidosItensRepository.InsereRegistroIndividualNotAsync(registro[0], tableName, database);
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
                    id_pedido_item = t1.id_pedido_item,
                    id_pedido = t1.id_pedido,
                    codigoproduto = t1.codigoproduto,
                    quantidade = t1.quantidade,
                    vl_unitario = t1.vl_unitario,
                    timestamp = t1.timestamp,
                    portal = t1.portal
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"B2CConsultaPedidosItens - TEntityToObject - Erro ao converter registro: {t1.id_pedido_item} para objeto - {ex.Message}");
            }
        }  
    }
}
