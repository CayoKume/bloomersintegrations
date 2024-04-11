using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;
using BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxCommerce;
using BloomersMicrovixIntegrations.LinxMicrovix.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovix.Domain.Extensions;
using BloomersMicrovixIntegrations.LinxMicrovix.Infrastructure.Apis;

namespace BloomersMicrovixIntegrations.Application.Services.LinxCommerce
{
    public class B2CConsultaNFeSituacaoService<TEntity> : IB2CConsultaNFeSituacaoService<TEntity> where TEntity : B2CConsultaNFeSituacao, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveB2C.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationB2C.ToName();
        private readonly IAPICall _apiCall;
        private readonly IB2CConsultaNFeSituacaoRepository _b2CConsultaNFeSituacaoRepository;

        public B2CConsultaNFeSituacaoService(IB2CConsultaNFeSituacaoRepository b2CConsultaNFeSituacaoRepository, IAPICall apiCall) =>
            (_b2CConsultaNFeSituacaoRepository, _apiCall) = (b2CConsultaNFeSituacaoRepository, apiCall);

        public List<TEntity> DeserializeResponse(List<Dictionary<string, string>> registros)
        {
            long timestamp;
            int id_nfe_situacao, portal;

            var list = new List<TEntity>();

            for (int i = 0; i < registros.Count(); i++)
            {
                try
                {
                    if (long.TryParse(registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First(), out long result))
                        timestamp = result;
                    else
                        timestamp = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "id_nfe_situacao").Select(pair => pair.Value).First(), out int result_0))
                        id_nfe_situacao = result_0;
                    else
                        id_nfe_situacao = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First(), out int result_4))
                        portal = result_4;
                    else
                        portal = 0;

                    list.Add(new TEntity
                    {
                        lastupdateon = DateTime.Now,
                        id_nfe_situacao = id_nfe_situacao,
                        descricao = registros[i].Where(pair => pair.Key == "descricao").Select(pair => pair.Value).First(),
                        timestamp = timestamp,
                        portal = portal
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = registros[i].Where(pair => pair.Key == "id_nfe_situacao").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_nfe_situacao").Select(pair => pair.Value).First();
                    throw new Exception($"B2CConsultaNFeSituacao - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
                }
            }

            return list;
        }

        public async Task IntegraRegistrosAsync(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _b2CConsultaNFeSituacaoRepository.GetParametersAsync(tableName, database, "parameters_lastday");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    var _listResults = listResults.ConvertAll(new Converter<TEntity, B2CConsultaNFeSituacao>(TEntityToObject));
                    var __listResults = await _b2CConsultaNFeSituacaoRepository.GetRegistersExistsAsync(_listResults, tableName, database);

                    for (int i = 0; i < __listResults.Count; i++)
                    {
                        _listResults.Remove(_listResults.Where(r => r.id_nfe_situacao == Convert.ToInt64(__listResults[i].id_nfe_situacao) && r.timestamp == __listResults[i].timestamp).FirstOrDefault());
                    }

                    if (_listResults.Count() > 0)
                        _b2CConsultaNFeSituacaoRepository.BulkInsertIntoTableRaw(_listResults, tableName, database);
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
                PARAMETERS = _b2CConsultaNFeSituacaoRepository.GetParametersNotAsync(tableName, database, "parameters_lastday");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);

                    if (listResults.Count() > 0)
                    {
                        var list = listResults.ConvertAll(new Converter<TEntity, B2CConsultaNFeSituacao>(TEntityToObject));
                        _b2CConsultaNFeSituacaoRepository.BulkInsertIntoTableRaw(list, tableName, database);
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
                    id_nfe_situacao = t1.id_nfe_situacao,
                    descricao = t1.descricao,
                    timestamp = t1.timestamp,
                    portal = t1.portal
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"B2CConsultaNFeSituacao - TEntityToObject - Erro ao converter registro: {t1.id_nfe_situacao} para objeto - {ex.Message}");
            }
        }
    }
}
