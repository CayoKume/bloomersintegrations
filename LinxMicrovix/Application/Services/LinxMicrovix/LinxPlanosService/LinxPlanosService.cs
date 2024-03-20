using BloomersMicrovixIntegrations.Saida.Core.Biz;
using BloomersMicrovixIntegrations.Saida.Microvix.Models;
using BloomersMicrovixIntegrations.Saida.Microvix.Repositorys.Interfaces;
using BloomersMicrovixIntegrations.Saida.Microvix.Services.Interfaces;
using System.Globalization;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Services
{
    public class LinxPlanosService<T1> : ILinxPlanosService<T1> where T1 : LinxPlanos, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly ILinxPlanosRepository<LinxPlanos> _linxPlanosRepository;

        public LinxPlanosService(ILinxPlanosRepository<LinxPlanos> linxPlanosRepository) =>
            _linxPlanosRepository = linxPlanosRepository;

        public List<T1?> DeserializeResponse(List<Dictionary<string, string>> registros)
        {
            var list = new List<T1>();

            for (int i = 0; i < registros.Count(); i++)
            {
                try
                {
                    list.Add(new T1
                    {
                        lastupdateon = DateTime.Now,
                        portal = registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First(),
                        plano = registros[i].Where(pair => pair.Key == "plano").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "plano").Select(pair => pair.Value).First(),
                        desc_plano = registros[i].Where(pair => pair.Key == "desc_plano").Select(pair => pair.Value).First(),
                        qtde_parcelas = registros[i].Where(pair => pair.Key == "qtde_parcelas").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "qtde_parcelas").Select(pair => pair.Value).First(),
                        prazo_entre_parcelas = registros[i].Where(pair => pair.Key == "prazo_entre_parcelas").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "prazo_entre_parcelas").Select(pair => pair.Value).First(),
                        tipo_plano = registros[i].Where(pair => pair.Key == "tipo_plano").Select(pair => pair.Value).First(),
                        indice_plano = registros[i].Where(pair => pair.Key == "indice_plano").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "indice_plano").Select(pair => pair.Value).First(),
                        cod_forma_pgto = registros[i].Where(pair => pair.Key == "cod_forma_pgto").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_forma_pgto").Select(pair => pair.Value).First(),
                        forma_pgto = registros[i].Where(pair => pair.Key == "forma_pgto").Select(pair => pair.Value).First(),
                        conta_central = registros[i].Where(pair => pair.Key == "conta_central").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "conta_central").Select(pair => pair.Value).First(),
                        tipo_transacao = registros[i].Where(pair => pair.Key == "tipo_transacao").Select(pair => pair.Value).First(),
                        taxa_financeira = registros[i].Where(pair => pair.Key == "taxa_financeira").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "taxa_financeira").Select(pair => pair.Value).First(),
                        dt_upd = registros[i].Where(pair => pair.Key == "dt_upd").Select(pair => pair.Value).First() == String.Empty ? new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar).ToString("yyyy-MM-dd HH:mm:ss") : registros[i].Where(pair => pair.Key == "dt_upd").Select(pair => pair.Value).First(),
                        desativado = registros[i].Where(pair => pair.Key == "desativado").Select(pair => pair.Value).First(),
                        usa_tef = registros[i].Where(pair => pair.Key == "usa_tef").Select(pair => pair.Value).First(),
                        timestamp = registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First(),
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = registros[i].Where(pair => pair.Key == "plano").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "plano").Select(pair => pair.Value).First();
                    throw new Exception($"LinxPlanos - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
                }
            }

            return list;
        }

        public async Task IntegraRegistros(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _linxPlanosRepository.GetParameters(tableName, "parameters_lastday");

                var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var registros = APICaller.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    if (listResults.Count() > 0)
                    {
                        var list = listResults.ConvertAll(new Converter<T1, LinxPlanos>(T1ToObject));
                        _linxPlanosRepository.BulkInsertIntoTableRaw(list, tableName, database);
                        //await _linxPlanosRepository.CallDbProcMerge(procName, tableName, database);
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
                PARAMETERS = _linxPlanosRepository.GetParametersSync(tableName, "parameters_lastday");

                var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var registros = APICaller.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    if (listResults.Count() > 0)
                    {
                        var list = listResults.ConvertAll(new Converter<T1, LinxPlanos>(T1ToObject));
                        _linxPlanosRepository.BulkInsertIntoTableRaw(list, tableName, database);
                        //_linxPlanosRepository.CallDbProcMergeSync(procName, tableName, database);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> IntegraRegistrosIndividual(string tableName, string procName, string database, string identificador)
        {
            try
            {
                PARAMETERS = await _linxPlanosRepository.GetParameters(tableName, "parameters_manual");

                string response = APICaller.CallLinxAPI(PARAMETERS.Replace("[plano]", $"{identificador}").Replace("[0]", "0").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var registros = APICaller.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    await _linxPlanosRepository.InsereRegistroIndividual(registro[0], tableName, database);
                    //await _linxPlanosRepository.CallDbProcMerge(procName, tableName, database);
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

        public bool IntegraRegistrosIndividualSync(string tableName, string procName, string database, string identificador)
        {
            try
            {
                PARAMETERS = _linxPlanosRepository.GetParametersSync(tableName, "parameters_manual");

                string response = APICaller.CallLinxAPI(PARAMETERS.Replace("[plano]", $"{identificador}").Replace("[0]", "0").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var registros = APICaller.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registros.Count() > 0)
                {
                    _linxPlanosRepository.InsereRegistroIndividualSync(registro[0], tableName, database);
                    //_linxPlanosRepository.CallDbProcMergeSync(procName, tableName, database);
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
                    plano = t1.plano,
                    desc_plano = t1.desc_plano,
                    qtde_parcelas = t1.qtde_parcelas,
                    prazo_entre_parcelas = t1.prazo_entre_parcelas,
                    tipo_plano = t1.tipo_plano,
                    indice_plano = t1.indice_plano,
                    cod_forma_pgto = t1.cod_forma_pgto,
                    forma_pgto = t1.forma_pgto,
                    conta_central = t1.conta_central,
                    tipo_transacao = t1.tipo_transacao,
                    taxa_financeira = t1.taxa_financeira,
                    dt_upd = t1.dt_upd,
                    desativado = t1.desativado,
                    usa_tef = t1.usa_tef,
                    timestamp = t1.timestamp
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxPlanos - T1ToObject - Erro ao converter registro: {t1.plano} para objeto - {ex.Message}");
            }
        }
    }
}
