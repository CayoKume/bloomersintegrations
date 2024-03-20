using BloomersMicrovixIntegrations.Saida.Core.Biz;
using BloomersMicrovixIntegrations.Saida.Microvix.Models;
using BloomersMicrovixIntegrations.Saida.Microvix.Repositorys;
using BloomersMicrovixIntegrations.Saida.Microvix.Repositorys.Interfaces;
using BloomersMicrovixIntegrations.Saida.Microvix.Services.Interfaces;
using System.Globalization;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Services
{
    public class LinxMovimentoPlanosService<T1> : ILinxMovimentoPlanosService<T1> where T1 : LinxMovimentoPlanos, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly ILinxMovimentoPlanosRepository<LinxMovimentoPlanos> _linxMovimentoPlanosRepository;

        public LinxMovimentoPlanosService(ILinxMovimentoPlanosRepository<LinxMovimentoPlanos> linxMovimentoPlanosRepository) =>
            _linxMovimentoPlanosRepository = linxMovimentoPlanosRepository;

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
                        cnpj_emp = registros[i].Where(pair => pair.Key == "cnpj_emp").Select(pair => pair.Value).First(),
                        identificador = registros[i].Where(pair => pair.Key == "identificador").Select(pair => pair.Value).First(),
                        plano = registros[i].Where(pair => pair.Key == "plano").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "plano").Select(pair => pair.Value).First(),
                        desc_plano = registros[i].Where(pair => pair.Key == "desc_plano").Select(pair => pair.Value).First(),
                        total = registros[i].Where(pair => pair.Key == "total").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "total").Select(pair => pair.Value).First(),
                        qtde_parcelas = registros[i].Where(pair => pair.Key == "qtde_parcelas").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "qtde_parcelas").Select(pair => pair.Value).First(),
                        indice_plano = registros[i].Where(pair => pair.Key == "indice_plano").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "indice_plano").Select(pair => pair.Value).First(),
                        cod_forma_pgto = registros[i].Where(pair => pair.Key == "cod_forma_pgto").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_forma_pgto").Select(pair => pair.Value).First(),
                        tipo_transacao = registros[i].Where(pair => pair.Key == "tipo_transacao").Select(pair => pair.Value).First(),
                        taxa_financeira = registros[i].Where(pair => pair.Key == "taxa_financeira").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "taxa_financeira").Select(pair => pair.Value).First(),
                        ordem_cartao = registros[i].Where(pair => pair.Key == "ordem_cartao").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "ordem_cartao").Select(pair => pair.Value).First(),
                        empresa = registros[i].Where(pair => pair.Key == "empresa").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "empresa").Select(pair => pair.Value).First(),
                        timestamp = registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First(),
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = registros[i].Where(pair => pair.Key == "identificador").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "identificador").Select(pair => pair.Value).First();
                    throw new Exception($"LinxMovimentoPlanos - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
                }
            }

            return list;
        }

        public async Task IntegraRegistros(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _linxMovimentoPlanosRepository.GetParameters(tableName, "parameters_lastday");

                var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var registros = APICaller.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    if (listResults.Count() > 0)
                    {
                        var list = listResults.ConvertAll(new Converter<T1, LinxMovimentoPlanos>(T1ToObject));
                        _linxMovimentoPlanosRepository.BulkInsertIntoTableRaw(list, tableName, database);
                        //await _linxMovimentoPlanosRepository.CallDbProcMerge(procName, tableName, database);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> IntegraRegistrosIndividual(string tableName, string procName, string database, string identificador, string identificador2)
        {
            try
            {
                PARAMETERS = await _linxMovimentoPlanosRepository.GetParameters(tableName, "parameters_manual");

                string response = APICaller.CallLinxAPI(PARAMETERS.Replace("[identificador]", $"{identificador}").Replace("[0]", "0").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, identificador2);
                var registros = APICaller.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    await _linxMovimentoPlanosRepository.InsereRegistroIndividual(registro[0], tableName, database);
                    //await _linxMovimentoPlanosRepository.CallDbProcMerge(procName, tableName, database);
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

        public bool IntegraRegistrosIndividualSync(string tableName, string procName, string database, string identificador, string identificador2)
        {
            try
            {
                PARAMETERS = _linxMovimentoPlanosRepository.GetParametersSync(tableName, "parameters_manual");

                string response = APICaller.CallLinxAPI(PARAMETERS.Replace("[identificador]", $"{identificador}").Replace("[0]", "0").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, identificador2);
                var registros = APICaller.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registros.Count() > 0)
                {
                    _linxMovimentoPlanosRepository.InsereRegistroIndividualSync(registro[0], tableName, database);
                    //_linxMovimentoPlanosRepository.CallDbProcMergeSync(procName, tableName, database);
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

        public void IntegraRegistrosSync(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = _linxMovimentoPlanosRepository.GetParametersSync(tableName, "parameters_lastday");

                var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var registros = APICaller.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    if (listResults.Count() > 0)
                    {
                        var list = listResults.ConvertAll(new Converter<T1, LinxMovimentoPlanos>(T1ToObject));
                        _linxMovimentoPlanosRepository.BulkInsertIntoTableRaw(list, tableName, database);
                        //_linxMovimentoPlanosRepository.CallDbProcMergeSync(procName, tableName, database);
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
                    portal = t1.portal,
                    cnpj_emp = t1.cnpj_emp,
                    identificador = t1.identificador,
                    plano = t1.plano,
                    desc_plano = t1.desc_plano,
                    total = t1.total,
                    qtde_parcelas = t1.qtde_parcelas,
                    indice_plano = t1.indice_plano,
                    cod_forma_pgto = t1.cod_forma_pgto,
                    forma_pgto = t1.forma_pgto,
                    tipo_transacao = t1.tipo_transacao,
                    taxa_financeira = t1.taxa_financeira,
                    ordem_cartao = t1.ordem_cartao,
                    timestamp = t1.timestamp,
                    empresa = t1.empresa
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxMovimentoPlanos - T1ToObject - Erro ao converter registro: {t1.plano} para objeto - {ex.Message}");
            }
        }
    }
}
