using BloomersMicrovixIntegrations.Repositorys.Ecommerce;
using BloomersMicrovixIntegrations.Saida.Core.Biz;
using BloomersMicrovixIntegrations.Saida.Ecommerce.Models.Ecommerce;
using BloomersMicrovixIntegrations.Saida.Ecommerce.Repositorys.Interfaces;
using BloomersMicrovixIntegrations.Saida.Ecommerce.Services.Interfaces;

namespace BloomersMicrovixIntegrations.Saida.Ecommerce.Services
{
    public class B2CConsultaNFeSituacaoService<T1> : IB2CConsultaNFeSituacaoService<T1> where T1 : B2CConsultaNFeSituacao, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveB2C.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationB2C.ToName();
        private readonly IB2CConsultaNFeSituacaoRepository<B2CConsultaNFeSituacao> _b2CConsultaNFeSituacaoRepository;

        public B2CConsultaNFeSituacaoService(IB2CConsultaNFeSituacaoRepository<B2CConsultaNFeSituacao> b2CConsultaNFeSituacaoRepository) =>
            _b2CConsultaNFeSituacaoRepository = b2CConsultaNFeSituacaoRepository;

        public List<T1> DeserializeResponse(List<Dictionary<string, string>> registros)
        {
            long timestamp;
            int id_nfe_situacao, portal;

            var list = new List<T1>();

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

                    list.Add(new T1
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

        public async Task IntegraRegistros(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _b2CConsultaNFeSituacaoRepository.GetParameters(tableName, "parameters_lastday");

                var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var registros = APICaller.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    var _listResults = listResults.ConvertAll(new Converter<T1, B2CConsultaNFeSituacao>(T1ToObject));
                    var __listResults = await _b2CConsultaNFeSituacaoRepository.GetRegistersExists(_listResults, tableName, database);

                    for (int i = 0; i < __listResults.Count; i++)
                    {
                        _listResults.Remove(_listResults.Where(r => r.id_nfe_situacao == Convert.ToInt64(__listResults[i].id_nfe_situacao) && r.timestamp == __listResults[i].timestamp).FirstOrDefault());
                    }

                    if (_listResults.Count() > 0)
                    {
                        _b2CConsultaNFeSituacaoRepository.BulkInsertIntoTableRaw(_listResults, tableName, database);
                        //await _b2CConsultaNFeSituacaoRepository.CallDbProcMerge(procName, tableName, database);
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
                PARAMETERS = _b2CConsultaNFeSituacaoRepository.GetParametersSync(tableName, "parameters_lastday");

                var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var registros = APICaller.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);

                    if (listResults.Count() > 0)
                    {
                        var list = listResults.ConvertAll(new Converter<T1, B2CConsultaNFeSituacao>(T1ToObject));
                        _b2CConsultaNFeSituacaoRepository.BulkInsertIntoTableRaw(list, tableName, database);
                        //_b2CConsultaNFeSituacaoRepository.CallDbProcMergeSync(procName, tableName, database);
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
                    id_nfe_situacao = t1.id_nfe_situacao,
                    descricao = t1.descricao,
                    timestamp = t1.timestamp,
                    portal = t1.portal
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"B2CConsultaNFeSituacao - T1ToObject - Erro ao converter registro: {t1.id_nfe_situacao} para objeto - {ex.Message}");
            }
        }
    }
}
