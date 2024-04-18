using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxEcommerce;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxCommerce;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Extensions;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Apis;
using System.Globalization;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxCommerce
{
    public class B2CConsultaPedidosStatusService<TEntity> : IB2CConsultaPedidosStatusService<TEntity> where TEntity : B2CConsultaPedidosStatus, new()
    {
        private string PARAMETERS = String.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveB2C.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationB2C.ToName();
        private readonly IAPICall _apiCall;
        private readonly IB2CConsultaPedidosStatusRepository _b2CConsultaPedidosStatusRepository;

        public B2CConsultaPedidosStatusService(IB2CConsultaPedidosStatusRepository b2CConsultaPedidosStatusRepository, IAPICall apiCall)
            => (_b2CConsultaPedidosStatusRepository, _apiCall) = (b2CConsultaPedidosStatusRepository, apiCall);

        public List<TEntity?> DeserializeResponse(List<Dictionary<string, string>> registros)
        {
            Int64 timestamp, id;
            Int32 id_status, id_pedido, portal;
            DateTime data_hora;

            var list = new List<TEntity>();

            for (int i = 0; i < registros.Count(); i++)
            {
                try
                {
                    if (Int64.TryParse(registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First(), out Int64 result))
                        timestamp = result;
                    else
                        timestamp = 0;

                    if (Int64.TryParse(registros[i].Where(pair => pair.Key == "id").Select(pair => pair.Value).First(), out Int64 result_0))
                        id = result;
                    else
                        id = 0;

                    if (Int32.TryParse(registros[i].Where(pair => pair.Key == "id_status").Select(pair => pair.Value).First(), out Int32 result_1))
                        id_status = result_1;
                    else
                        id_status = 0;

                    if (Int32.TryParse(registros[i].Where(pair => pair.Key == "id_pedido").Select(pair => pair.Value).First(), out Int32 result_2))
                        id_pedido = result_2;
                    else
                        id_pedido = 0;

                    if (Int32.TryParse(registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First(), out Int32 result_3))
                        portal = result_3;
                    else
                        portal = 0;

                    if (DateTime.TryParse(registros[i].Where(pair => pair.Key == "data_hora").Select(pair => pair.Value).First(), out DateTime result_7))
                        data_hora = result_7;
                    else
                        data_hora = new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar);

                    list.Add(new TEntity
                    {
                        lastupdateon = DateTime.Now,
                        id = id,
                        id_status = id_status,
                        id_pedido = id_pedido,
                        data_hora = data_hora,
                        anotacao = registros[i].Where(pair => pair.Key == "anotacao").Select(pair => pair.Value).First(),
                        timestamp = timestamp,
                        portal = portal
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = registros[i].Where(pair => pair.Key == "id").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id").Select(pair => pair.Value).First();
                    throw new Exception($"B2CConsultaPedidosStatus - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
                }
            }
            return list;
        }

        public async Task IntegraRegistrosAsync(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _b2CConsultaPedidosStatusRepository.GetParametersAsync(tableName, database, "parameters_lastday");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    var _listResults = listResults.ConvertAll(new Converter<TEntity, B2CConsultaPedidosStatus>(TEntityToObject));
                    var __listResults = await _b2CConsultaPedidosStatusRepository.GetRegistersExistsAsync(_listResults, tableName, database);

                    for (int i = 0; i < __listResults.Count; i++)
                    {
                        _listResults.Remove(_listResults.Where(r => r.id == Convert.ToInt32(__listResults[i].id) && r.timestamp == __listResults[i].timestamp).FirstOrDefault());
                    }

                    if (_listResults.Count() > 0)
                        _b2CConsultaPedidosStatusRepository.BulkInsertIntoTableRaw(_listResults, tableName, database);
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
                PARAMETERS = _b2CConsultaPedidosStatusRepository.GetParametersNotAsync(tableName, database, "parameters_lastday");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    if (listResults.Count() > 0)
                    {
                        var list = listResults.ConvertAll(new Converter<TEntity, B2CConsultaPedidosStatus>(TEntityToObject));
                        _b2CConsultaPedidosStatusRepository.BulkInsertIntoTableRaw(list, tableName, database);
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
                PARAMETERS = await _b2CConsultaPedidosStatusRepository.GetParametersAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[id_pedido]", $"{identificador}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro is not null)
                {
                    await _b2CConsultaPedidosStatusRepository.InsereRegistroIndividualAsync(registro[0], tableName, database);
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
                PARAMETERS = _b2CConsultaPedidosStatusRepository.GetParametersNotAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[id_pedido]", $"{identificador}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                string response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro is not null)
                {
                    _b2CConsultaPedidosStatusRepository.InsereRegistroIndividualNotAsync(registro[0], tableName, database);
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
                    id = t1.id,
                    id_status = t1.id_status,
                    id_pedido = t1.id_pedido,
                    data_hora = t1.data_hora,
                    anotacao = t1.anotacao,
                    timestamp = t1.timestamp,
                    portal = t1.portal
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"B2CConsultaPedidosStatus - TEntityToObject - Erro ao converter registro: {t1.id} para objeto - {ex.Message}");
            }
        }
    }
}
