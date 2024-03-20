using BloomersMicrovixIntegrations.Saida.Core.Biz;
using BloomersMicrovixIntegrations.Saida.Microvix.Models;
using BloomersMicrovixIntegrations.Saida.Microvix.Repositorys.Interfaces;
using BloomersMicrovixIntegrations.Saida.Microvix.Services.Interfaces;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Services
{
    public class LinxProdutosCamposAdicionaisService<T1> : ILinxProdutosCamposAdicionaisService<T1> where T1 : LinxProdutosCamposAdicionais, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly ILinxProdutosCamposAdicionaisRepository<LinxProdutosCamposAdicionais> _linxProdutosCamposAdicionaisRepository;

        public LinxProdutosCamposAdicionaisService(ILinxProdutosCamposAdicionaisRepository<LinxProdutosCamposAdicionais> linxProdutosCamposAdicionaisRepository)
            => (_linxProdutosCamposAdicionaisRepository) = (linxProdutosCamposAdicionaisRepository);

        public List<T1?> DeserializeResponse(List<Dictionary<string, string>> registros)
        {
            var list = new List<T1?>();

            for (int i = 0; i < registros.Count; i++)
            {
                try
                {
                    list.Add(new T1
                    {
                        lastupdateon = DateTime.Now,
                        portal = registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First(),
                        cod_produto = registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First(),
                        campo = registros[i].Where(pair => pair.Key == "campo").Select(pair => pair.Value).First(),
                        valor = registros[i].Where(pair => pair.Key == "valor").Select(pair => pair.Value).First(),
                        timestamp = registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First(),
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First();
                    throw new Exception($"LinxProdutosCamposAdicionais - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
                }
            }

            return list;
        }

        public async Task IntegraRegistros(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _linxProdutosCamposAdicionaisRepository.GetParameters(tableName, "parameters_lastday");

                var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[data_inicio]", $"2000-01-01").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var registros = APICaller.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    if (listResults.Count() > 0)
                    {
                        var list = listResults.ConvertAll(new Converter<T1, LinxProdutosCamposAdicionais>(T1ToObject));
                        _linxProdutosCamposAdicionaisRepository.BulkInsertIntoTableRaw(list, tableName, database);
                       //await _linxProdutosCamposAdicionaisRepository.CallDbProcMerge(procName, tableName, database);
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
                PARAMETERS = _linxProdutosCamposAdicionaisRepository.GetParametersSync(tableName, "parameters_lastday");

                var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[data_inicio]", $"2000-01-01").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var registros = APICaller.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    if (listResults.Count() > 0)
                    {
                        var list = listResults.ConvertAll(new Converter<T1, LinxProdutosCamposAdicionais>(T1ToObject));
                        _linxProdutosCamposAdicionaisRepository.BulkInsertIntoTableRaw(list, tableName, database);
                        //_linxProdutosCamposAdicionaisRepository.CallDbProcMergeSync(procName, tableName, database);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> IntegraRegistrosIndividual(string tableName, string procName, string database, string identificador, string cnpj_emp)
        {
            try
            {
                PARAMETERS = await _linxProdutosCamposAdicionaisRepository.GetParameters(tableName, "parameters_manual");

                string response = APICaller.CallLinxAPI(PARAMETERS.Replace("[data_inicio]", $"{DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}").Replace("[cod_produto]", $"{identificador}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var registros = APICaller.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    await _linxProdutosCamposAdicionaisRepository.InsereRegistroIndividual(registro[0], tableName, database);
                    //await _linxProdutosCamposAdicionaisRepository.CallDbProcMerge(procName, tableName, database);
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

        public bool IntegraRegistrosIndividualSync(string tableName, string procName, string database, string identificador, string cnpj_emp)
        {
            try
            {
                PARAMETERS = _linxProdutosCamposAdicionaisRepository.GetParametersSync(tableName, "parameters_manual");

                string response = APICaller.CallLinxAPI(PARAMETERS.Replace("[data_inicio]", $"{DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}").Replace("[cod_produto]", $"{identificador}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var registros = APICaller.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    _linxProdutosCamposAdicionaisRepository.InsereRegistroIndividualSync(registro[0], tableName, database);
                    //_linxProdutosCamposAdicionaisRepository.CallDbProcMergeSync(procName, tableName, database);
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
                    cod_produto = t1.cod_produto,
                    campo = t1.campo,
                    valor = t1.valor,
                    timestamp = t1.timestamp
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxProdutosCamposAdicionais - T1ToObject - Erro ao converter registro: {t1.cod_produto} para objeto - {ex.Message}");
            }
        }
    }
}
