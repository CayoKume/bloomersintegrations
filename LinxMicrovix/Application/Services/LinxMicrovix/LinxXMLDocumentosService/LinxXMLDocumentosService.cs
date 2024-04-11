using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;
using BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovix.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovix.Domain.Extensions;
using BloomersMicrovixIntegrations.LinxMicrovix.Infrastructure.Apis;
using System.Globalization;

namespace BloomersMicrovixIntegrations.Application.Services.LinxMicrovix
{
    public class LinxXMLDocumentosService<TEntity> : ILinxXMLDocumentosService<TEntity> where TEntity : LinxXMLDocumentos, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly IAPICall _apiCall;
        private readonly ILinxXMLDocumentosRepository _linxXMLDocumentosRepository;

        public LinxXMLDocumentosService(ILinxXMLDocumentosRepository linxXMLDocumentosRepository, IAPICall apiCall)
            => (_linxXMLDocumentosRepository, _apiCall) = (linxXMLDocumentosRepository, apiCall);

        public List<TEntity?> DeserializeResponse(List<Dictionary<string, string>> registros)
        {
            var list = new List<TEntity?>();

            for (int i = 0; i < registros.Count(); i++)
            {
                try
                {
                    list.Add(new TEntity
                    {
                        lastupdateon = DateTime.Now,
                        portal = registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First(),
                        cnpj_emp = registros[i].Where(pair => pair.Key == "cnpj_emp").Select(pair => pair.Value).First(),
                        documento = registros[i].Where(pair => pair.Key == "documento").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "documento").Select(pair => pair.Value).First(),
                        serie = registros[i].Where(pair => pair.Key == "serie").Select(pair => pair.Value).First(),
                        data_emissao = registros[i].Where(pair => pair.Key == "data_emissao").Select(pair => pair.Value).First() == String.Empty ? new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar).ToString("yyyy-MM-dd HH:mm:ss") : registros[i].Where(pair => pair.Key == "data_emissao").Select(pair => pair.Value).First(),
                        chave_nfe = registros[i].Where(pair => pair.Key == "chave_nfe").Select(pair => pair.Value).First(),
                        situacao = registros[i].Where(pair => pair.Key == "situacao").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "situacao").Select(pair => pair.Value).First(),
                        xml = registros[i].Where(pair => pair.Key == "xml").Select(pair => pair.Value).First(),
                        excluido = registros[i].Where(pair => pair.Key == "excluido").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "excluido").Select(pair => pair.Value).First(),
                        identificador_microvix = registros[i].Where(pair => pair.Key == "identificador_microvix").Select(pair => pair.Value).First(),
                        dt_insert = registros[i].Where(pair => pair.Key == "dt_insert").Select(pair => pair.Value).First() == String.Empty ? new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar).ToString("yyyy-MM-dd") : registros[i].Where(pair => pair.Key == "dt_insert").Select(pair => pair.Value).First(),
                        timestamp = registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First(),
                        nProtCanc = registros[i].Where(pair => pair.Key == "nProtCanc").Select(pair => pair.Value).First(),
                        nProtInut = registros[i].Where(pair => pair.Key == "nProtInut").Select(pair => pair.Value).First(),
                        xmlDistribuicao = registros[i].Where(pair => pair.Key == "xmlDistribuicao").Select(pair => pair.Value).First(),
                        nProtDeneg = registros[i].Where(pair => pair.Key == "nProtDeneg").Select(pair => pair.Value).First(),
                        cStat = registros[i].Where(pair => pair.Key == "cStat").Select(pair => pair.Value).First(),
                        id_nfe = registros[i].Where(pair => pair.Key == "id_nfe").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_nfe").Select(pair => pair.Value).First(),
                        justificativa = registros[i].Where(pair => pair.Key == "justificativa").Select(pair => pair.Value).First()
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = registros[i].Where(pair => pair.Key == "documento").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "documento").Select(pair => pair.Value).First();
                    throw new Exception($"LinxXMLDocumentos - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
                }
            }

            return list;
        }

        public async Task IntegraRegistrosAsync(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _linxXMLDocumentosRepository.GetParametersAsync(tableName, database, "parameters_lastday");

                var cnpjs = await _linxXMLDocumentosRepository.GetCompanysAsync(tableName, database);

                foreach (var cnpj in cnpjs)
                {
                    var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_company);
                    var response = await _apiCall.CallAPIAsync(tableName, body);
                    var registros = _apiCall.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<TEntity, LinxXMLDocumentos>(TEntityToObject));
                            _linxXMLDocumentosRepository.BulkInsertIntoTableRaw(list, tableName, database);
                        }
                    } 
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
                PARAMETERS = _linxXMLDocumentosRepository.GetParametersNotAsync(tableName, database, "parameters_lastday");

                var cnpjs = _linxXMLDocumentosRepository.GetCompanysNotAsync(tableName, database);

                foreach (var cnpj in cnpjs)
                {
                    var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_company);
                    var response = _apiCall.CallAPINotAsync(tableName, body);
                    var registros = _apiCall.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<TEntity, LinxXMLDocumentos>(TEntityToObject));
                            _linxXMLDocumentosRepository.BulkInsertIntoTableRaw(list, tableName, database);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> IntegraRegistrosIndividualAsync(string tableName, string procName, string database, string identificador1, string identificador2, string cnpj_emp)
        {
            try
            {
                PARAMETERS = await _linxXMLDocumentosRepository.GetParametersAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[documento]", $"{identificador1}").Replace("[serie]", $"{identificador2}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                var response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    await _linxXMLDocumentosRepository.InsereRegistroIndividualAsync(registro[0], tableName, database);
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

        public bool IntegraRegistrosIndividualNotAsync(string tableName, string procName, string database, string identificador1, string identificador2, string cnpj_emp)
        {
            try
            {
                PARAMETERS = _linxXMLDocumentosRepository.GetParametersNotAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[documento]", $"{identificador1}").Replace("[serie]", $"{identificador2}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                string response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    _linxXMLDocumentosRepository.InsereRegistroIndividualNotAsync(registro[0], tableName, database);
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
                    portal = t1.portal,
                    cnpj_emp = t1.cnpj_emp,
                    documento = t1.documento,
                    serie = t1.serie,
                    data_emissao = t1.data_emissao,
                    chave_nfe = t1.chave_nfe,
                    situacao = t1.situacao,
                    xml = t1.xml,
                    excluido = t1.excluido,
                    identificador_microvix = t1.identificador_microvix,
                    dt_insert = t1.dt_insert,
                    timestamp = t1.timestamp,
                    nProtCanc = t1.nProtCanc,
                    nProtInut = t1.nProtInut,
                    xmlDistribuicao = t1.xmlDistribuicao,
                    nProtDeneg = t1.nProtDeneg,
                    cStat = t1.cStat,
                    id_nfe = t1.id_nfe,
                    justificativa = t1.justificativa
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxXMLDocumentos - TEntityToObject - Erro ao converter registro: {t1.documento} para objeto - {ex.Message}");
            }
        }
    }
}
