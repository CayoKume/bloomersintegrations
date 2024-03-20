using BloomersMicrovixIntegrations.Saida.Core.Biz;
using BloomersMicrovixIntegrations.Saida.Microvix.Models;
using BloomersMicrovixIntegrations.Saida.Microvix.Repositorys.Interfaces;
using BloomersMicrovixIntegrations.Saida.Microvix.Services.Interfaces;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Services
{
    public class LinxProdutosTabelasPrecosService<T1> : ILinxProdutosTabelasPrecosService<T1> where T1 : LinxProdutosTabelasPrecos, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly ILinxProdutosTabelasPrecosRepository<LinxProdutosTabelasPrecos> _linxProdutosTabelasPrecosRepository;

        public LinxProdutosTabelasPrecosService(ILinxProdutosTabelasPrecosRepository<LinxProdutosTabelasPrecos> linxProdutosTabelasPrecosRepository)
            => (_linxProdutosTabelasPrecosRepository) = (linxProdutosTabelasPrecosRepository);

        public List<T1?> DeserializeResponse(List<Dictionary<string, string>> registros)
        {
            var list = new List<T1?>();

            for(var i = 0; i < registros.Count; i++)
            {
                try
                {
                    list.Add(new T1
                    {
                        lastupdateon = DateTime.Now,
                        portal = registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First(),
                        cnpj_emp = registros[i].Where(pair => pair.Key == "cnpj_emp").Select(pair => pair.Value).First(),
                        id_tabela = registros[i].Where(pair => pair.Key == "id_tabela").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_tabela").Select(pair => pair.Value).First(),
                        cod_produto = registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First(),
                        precovenda = registros[i].Where(pair => pair.Key == "precovenda").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "precovenda").Select(pair => pair.Value).First(),
                        timestamp = registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First()
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = "id_tabela: " + registros[i].Where(pair => pair.Key == "id_tabela").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_tabela").Select(pair => pair.Value).First() + " cod_produto: " + registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First();
                    throw new Exception($"LinxProdutosTabelasPrecos - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
                }
            }

            return list;
        }

        public async Task IntegraRegistros(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _linxProdutosTabelasPrecosRepository.GetParameters(tableName, "parameters_lastday");

                var cnpjs = await _linxProdutosTabelasPrecosRepository.GetEmpresas();

                foreach (var cnpj in cnpjs)
                {
                    var idsTabelas = await _linxProdutosTabelasPrecosRepository.GetIdTabelaPreco(cnpj.doc_empresa);

                    foreach (var idTabela in idsTabelas)
                    {
                        var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[0]", "0").Replace("[tabela_id]", idTabela), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_empresa);
                        var registros = APICaller.DeserializeXML(response);

                        if (registros.Count() > 0)
                        {
                            var listResults = DeserializeResponse(registros);
                            if (listResults.Count() > 0)
                            {
                                var list = listResults.ConvertAll(new Converter<T1, LinxProdutosTabelasPrecos>(T1ToObject));
                                _linxProdutosTabelasPrecosRepository.BulkInsertIntoTableRaw(list, tableName, database);
                                await _linxProdutosTabelasPrecosRepository.CallDbProcMerge(procName, tableName, database);
                            }
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
                PARAMETERS = _linxProdutosTabelasPrecosRepository.GetParametersSync(tableName, "parameters_lastday");

                var cnpjs = _linxProdutosTabelasPrecosRepository.GetEmpresasSync();

                foreach (var cnpj in cnpjs)
                {
                    var idsTabelas = _linxProdutosTabelasPrecosRepository.GetIdTabelaPrecoSync(cnpj.doc_empresa);

                    foreach (var idTabela in idsTabelas)
                    {
                        var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[0]", "0").Replace("[tabela_id]", idTabela), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_empresa);
                        var registros = APICaller.DeserializeXML(response);

                        if (registros.Count() > 0)
                        {
                            var listResults = DeserializeResponse(registros);
                            if (listResults.Count() > 0)
                            {
                                var list = listResults.ConvertAll(new Converter<T1, LinxProdutosTabelasPrecos>(T1ToObject));
                                _linxProdutosTabelasPrecosRepository.BulkInsertIntoTableRaw(list, tableName, database);
                                _linxProdutosTabelasPrecosRepository.CallDbProcMergeSync(procName, tableName, database);
                            }
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
                PARAMETERS = await _linxProdutosTabelasPrecosRepository.GetParameters(tableName, "parameters_manual");

                string response = APICaller.CallLinxAPI(PARAMETERS.Replace("[cod_produto]", $"{identificador1}").Replace("[id_tabela]", $"{identificador2}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                var registros = APICaller.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    await _linxProdutosTabelasPrecosRepository.InsereRegistroIndividual(registro[0], tableName, database);
                    await _linxProdutosTabelasPrecosRepository.CallDbProcMerge(procName, tableName, database);
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
                PARAMETERS = _linxProdutosTabelasPrecosRepository.GetParametersSync(tableName, "parameters_manual");

                string response = APICaller.CallLinxAPI(PARAMETERS.Replace("[cod_produto]", $"{identificador1}").Replace("[id_tabela]", $"{identificador2}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                var registros = APICaller.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    _linxProdutosTabelasPrecosRepository.InsereRegistroIndividualSync(registro[0], tableName, database);
                    _linxProdutosTabelasPrecosRepository.CallDbProcMergeSync(procName, tableName, database);
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
                    id_tabela = t1.id_tabela,
                    cod_produto = t1.cod_produto,
                    precovenda = t1.precovenda,
                    timestamp = t1.timestamp
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxProdutosTabelasPrecos - T1ToObject - Erro ao converter registro id_tabela: {t1.id_tabela}, cod_produto: {t1.cod_produto} para objeto - {ex.Message}");
            }
        }
    }
}
