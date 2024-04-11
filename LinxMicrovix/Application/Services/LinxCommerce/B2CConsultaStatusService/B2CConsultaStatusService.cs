using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;
using BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxCommerce;
using BloomersMicrovixIntegrations.LinxMicrovix.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovix.Domain.Extensions;
using BloomersMicrovixIntegrations.LinxMicrovix.Infrastructure.Apis;

namespace BloomersMicrovixIntegrations.Application.Services.LinxCommerce
{
    public class B2CConsultaStatusService<TEntity> : IB2CConsultaStatusService<TEntity> where TEntity : B2CConsultaStatus, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveB2C.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationB2C.ToName();
        private readonly IAPICall _apiCall; 
        private readonly IB2CConsultaStatusRepository _b2CConsultaStatusRepository;

        public B2CConsultaStatusService(IB2CConsultaStatusRepository b2CConsultaStatusRepository, IAPICall apiCall) =>
            (_b2CConsultaStatusRepository, _apiCall) = (b2CConsultaStatusRepository, apiCall);
        
        public List<TEntity?> DeserializeResponse(List<Dictionary<string, string>> registros)
        {
            Int64 timestamp;
            Int32 id_status, portal;

            var list = new List<TEntity>();

            for (int i = 0; i < registros.Count(); i++)
            {
                try
                {
                    if (Int64.TryParse(registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First(), out long result))
                        timestamp = result;
                    else
                        timestamp = 0;

                    if (Int32.TryParse(registros[i].Where(pair => pair.Key == "id_status").Select(pair => pair.Value).First(), out Int32 result_0))
                        id_status = result_0;
                    else
                        id_status = 0;

                    if (Int32.TryParse(registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First(), out Int32 result_4))
                        portal = result_4;
                    else
                        portal = 0;

                    list.Add(new TEntity
                    {
                        lastupdateon = DateTime.Now,
                        id_status = id_status,
                        descricao_status = registros[i].Where(pair => pair.Key == "descricao_status").Select(pair => pair.Value).First(),
                        timestamp = timestamp,
                        portal = portal
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = registros[i].Where(pair => pair.Key == "id_status").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_status").Select(pair => pair.Value).First();
                    throw new Exception($"B2CConsultaStatus - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
                }
            }

            return list;
        }

        public async Task IntegraRegistrosAsync(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _b2CConsultaStatusRepository.GetParametersAsync(tableName, database, "parameters_lastday");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    var _listResults = listResults.ConvertAll(new Converter<TEntity, B2CConsultaStatus>(TEntityToObject));
                    var __listResults = await _b2CConsultaStatusRepository.GetRegistersExistsAsync(_listResults, tableName, database);

                    for (int i = 0; i < __listResults.Count; i++)
                    {
                        _listResults.Remove(_listResults.Where(r => r.id_status == Convert.ToInt32(__listResults[i].id_status) && r.timestamp == __listResults[i].timestamp).FirstOrDefault());
                    }

                    if (_listResults.Count() > 0)
                        _b2CConsultaStatusRepository.BulkInsertIntoTableRaw(_listResults, tableName, database);
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
                PARAMETERS = _b2CConsultaStatusRepository.GetParametersNotAsync(tableName, database, "parameters_lastday");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);

                    if (listResults.Count() > 0)
                    {
                        var list = listResults.ConvertAll(new Converter<TEntity, B2CConsultaStatus>(TEntityToObject));
                        _b2CConsultaStatusRepository.BulkInsertIntoTableRaw(list, tableName, database);
                    }
                }
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
                    id_status = t1.id_status,
                    descricao_status = t1.descricao_status,
                    timestamp = t1.timestamp,
                    portal = t1.portal
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"B2CConsultaStatus - TEntityToObject - Erro ao converter registro: {t1.id_status} para objeto - {ex.Message}");
            }
        }
    }
}
