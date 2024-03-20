using BloomersMicrovixIntegrations.Repositorys.Ecommerce;
using BloomersMicrovixIntegrations.Saida.Core.Biz;
using BloomersMicrovixIntegrations.Saida.Ecommerce.Models.Ecommerce;
using BloomersMicrovixIntegrations.Saida.Ecommerce.Repositorys.Interfaces;
using BloomersMicrovixIntegrations.Saida.Ecommerce.Services.Interfaces;

namespace BloomersMicrovixIntegrations.Saida.Ecommerce.Services
{
    public class B2CConsultaStatusService<T1> : IB2CConsultaStatusService<T1> where T1 : B2CConsultaStatus, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveB2C.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationB2C.ToName();
        private readonly IB2CConsultaStatusRepository<B2CConsultaStatus> _b2CConsultaStatusRepository;

        public B2CConsultaStatusService(IB2CConsultaStatusRepository<B2CConsultaStatus> b2CConsultaStatusRepository) =>
            (_b2CConsultaStatusRepository) = (b2CConsultaStatusRepository);
        
        public List<T1?> DeserializeResponse(List<Dictionary<string, string>> registros)
        {
            Int64 timestamp;
            Int32 id_status, portal;

            var list = new List<T1>();

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

                    list.Add(new T1
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

        public async Task IntegraRegistros(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _b2CConsultaStatusRepository.GetParameters(tableName, "parameters_lastday");

                var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var registros = APICaller.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    var _listResults = listResults.ConvertAll(new Converter<T1, B2CConsultaStatus>(T1ToObject));
                    var __listResults = await _b2CConsultaStatusRepository.GetRegistersExists(_listResults, tableName, database);

                    for (int i = 0; i < __listResults.Count; i++)
                    {
                        _listResults.Remove(_listResults.Where(r => r.id_status == Convert.ToInt32(__listResults[i].id_status) && r.timestamp == __listResults[i].timestamp).FirstOrDefault());
                    }

                    if (_listResults.Count() > 0)
                    {
                        _b2CConsultaStatusRepository.BulkInsertIntoTableRaw(_listResults, tableName, database);
                        //await _b2CConsultaStatusRepository.CallDbProcMerge(procName, tableName, database);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public void IntegraRegistrosSync(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = _b2CConsultaStatusRepository.GetParametersSync(tableName, "parameters_lastday");

                var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var registros = APICaller.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);

                    if (listResults.Count() > 0)
                    {
                        var list = listResults.ConvertAll(new Converter<T1, B2CConsultaStatus>(T1ToObject));
                        _b2CConsultaStatusRepository.BulkInsertIntoTableRaw(list, tableName, database);
                        //_b2CConsultaStatusRepository.CallDbProcMergeSync(procName, tableName, database);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public T1? T1ToObject(T1 t1)
        {
            try
            {
                return new T1
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
                throw new Exception($"B2CConsultaStatus - T1ToObject - Erro ao converter registro: {t1.id_status} para objeto - {ex.Message}");
            }
        }
    }
}
