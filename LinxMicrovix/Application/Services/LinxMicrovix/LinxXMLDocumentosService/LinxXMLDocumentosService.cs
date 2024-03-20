using BloomersMicrovixIntegrations.Saida.Core.Biz;
using BloomersMicrovixIntegrations.Saida.Microvix.Models;
using BloomersMicrovixIntegrations.Saida.Microvix.Repositorys.Interfaces;
using BloomersMicrovixIntegrations.Saida.Microvix.Services.Interfaces;
using System.Globalization;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Services
{
    public class LinxXMLDocumentosService<T1> : ILinxXMLDocumentosService<T1> where T1 : LinxXMLDocumentos, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly ILinxXMLDocumentosRepository<LinxXMLDocumentos> _linxXMLDocumentosRepository;

        public LinxXMLDocumentosService(ILinxXMLDocumentosRepository<LinxXMLDocumentos> linxXMLDocumentosRepository)
            => (_linxXMLDocumentosRepository) = (linxXMLDocumentosRepository);

        public List<T1?> DeserializeResponse(List<Dictionary<string, string>> registros)
        {
            var list = new List<T1?>();

            for (int i = 0; i < registros.Count(); i++)
            {
                try
                {
                    list.Add(new T1
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

        public async Task IntegraRegistros(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _linxXMLDocumentosRepository.GetParameters(tableName, "parameters_lastday");

                var cnpjs = await _linxXMLDocumentosRepository.GetEmpresas();

                foreach (var cnpj in cnpjs)
                {
                    var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_empresa);
                    var registros = APICaller.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<T1, LinxXMLDocumentos>(T1ToObject));
                            _linxXMLDocumentosRepository.BulkInsertIntoTableRaw(list, tableName, database);
                            //await _linxXMLDocumentosRepository.CallDbProcMerge(procName, tableName, database);
                        }
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
                PARAMETERS = _linxXMLDocumentosRepository.GetParametersSync(tableName, "parameters_lastday");

                var cnpjs = _linxXMLDocumentosRepository.GetEmpresasSync();

                foreach (var cnpj in cnpjs)
                {
                    var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_empresa);
                    var registros = APICaller.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<T1, LinxXMLDocumentos>(T1ToObject));
                            _linxXMLDocumentosRepository.BulkInsertIntoTableRaw(list, tableName, database);
                            //_linxXMLDocumentosRepository.CallDbProcMergeSync(procName, tableName, database);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> IntegraRegistrosIndividual(string tableName, string procName, string database, string identificador1, string identificador2, string cnpj_emp)
        {
            try
            {
                PARAMETERS = await _linxXMLDocumentosRepository.GetParameters(tableName, "parameters_manual");

                string response = APICaller.CallLinxAPI(PARAMETERS.Replace("[documento]", $"{identificador1}").Replace("[serie]", $"{identificador2}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                var registros = APICaller.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    await _linxXMLDocumentosRepository.InsereRegistroIndividual(registro[0], tableName, database);
                    //await _linxXMLDocumentosRepository.CallDbProcMerge(procName, tableName, database);
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

        public bool IntegraRegistrosIndividualSync(string tableName, string procName, string database, string identificador1, string identificador2, string cnpj_emp)
        {
            try
            {
                PARAMETERS = _linxXMLDocumentosRepository.GetParametersSync(tableName, "parameters_manual");

                string response = APICaller.CallLinxAPI(PARAMETERS.Replace("[documento]", $"{identificador1}").Replace("[serie]", $"{identificador2}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                var registros = APICaller.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    _linxXMLDocumentosRepository.InsereRegistroIndividualSync(registro[0], tableName, database);
                    //_linxXMLDocumentosRepository.CallDbProcMergeSync(procName, tableName, database);
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

        public T1? T1ToObject(T1 t1)
        {
            try
            {
                return new T1
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
                throw new Exception($"LinxXMLDocumentos - T1ToObject - Erro ao converter registro: {t1.documento} para objeto - {ex.Message}");
            }
        }
    }
}
