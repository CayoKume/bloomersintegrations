using BloomersMicrovixIntegrations.Saida.Core.Biz;
using BloomersMicrovixIntegrations.Saida.Microvix.Models;
using BloomersMicrovixIntegrations.Saida.Microvix.Repositorys.Interfaces;
using BloomersMicrovixIntegrations.Saida.Microvix.Services.Interfaces;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Services
{
    public class LinxProdutosTabelasService<T1> : ILinxProdutosTabelasService<T1> where T1 : LinxProdutosTabelas, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly ILinxProdutosTabelasRepository<LinxProdutosTabelas> _linxProdutosTabelasRepository;

        public LinxProdutosTabelasService(ILinxProdutosTabelasRepository<LinxProdutosTabelas> linxProdutosTabelasRepository)
            => (_linxProdutosTabelasRepository) = (linxProdutosTabelasRepository);

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
                        cnpj_emp = registros[i].Where(pair => pair.Key == "cnpj_emp").Select(pair => pair.Value).First(),
                        id_tabela = registros[i].Where(pair => pair.Key == "id_tabela").Select(pair => pair.Value).First(),
                        nome_tabela = registros[i].Where(pair => pair.Key == "nome_tabela").Select(pair => pair.Value).First(),
                        ativa = registros[i].Where(pair => pair.Key == "ativa").Select(pair => pair.Value).First(),
                        timestamp = registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First(),
                        tipo_tabela = registros[i].Where(pair => pair.Key == "tipo_tabela").Select(pair => pair.Value).First()
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = registros[i].Where(pair => pair.Key == "id_tabela").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_tabela").Select(pair => pair.Value).First();
                    throw new Exception($"LinxProdutosTabelas - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
                }
            }

            return list;
        }

        public async Task IntegraRegistros(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _linxProdutosTabelasRepository.GetParameters(tableName, "parameters_lastday");

                var cnpjs = await _linxProdutosTabelasRepository.GetEmpresas();

                foreach (var cnpj in cnpjs)
                {
                    var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_empresa);
                    var registros = APICaller.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<T1, LinxProdutosTabelas>(T1ToObject));
                            _linxProdutosTabelasRepository.BulkInsertIntoTableRaw(list, tableName, database);
                            await _linxProdutosTabelasRepository.CallDbProcMerge(procName, tableName, database);
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
                PARAMETERS = _linxProdutosTabelasRepository.GetParametersSync(tableName, "parameters_lastday");

                var cnpjs = _linxProdutosTabelasRepository.GetEmpresasSync();

                foreach (var cnpj in cnpjs)
                {
                    var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_empresa);
                    var registros = APICaller.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<T1, LinxProdutosTabelas>(T1ToObject));
                            _linxProdutosTabelasRepository.BulkInsertIntoTableRaw(list, tableName, database);
                            _linxProdutosTabelasRepository.CallDbProcMergeSync(procName, tableName, database);
                        }
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
                PARAMETERS = await _linxProdutosTabelasRepository.GetParameters(tableName, "parameters_manual");

                string response = APICaller.CallLinxAPI(PARAMETERS.Replace("[cod_produto]", $"{identificador}").Replace("[dt_update_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                var registros = APICaller.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    await _linxProdutosTabelasRepository.InsereRegistroIndividual(registro[0], tableName, database);
                    await _linxProdutosTabelasRepository.CallDbProcMerge(procName, tableName, database);
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
                PARAMETERS = _linxProdutosTabelasRepository.GetParametersSync(tableName, "parameters_manual");

                string response = APICaller.CallLinxAPI(PARAMETERS.Replace("[cod_produto]", $"{identificador}").Replace("[dt_update_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                var registros = APICaller.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    _linxProdutosTabelasRepository.InsereRegistroIndividualSync(registro[0], tableName, database);
                    _linxProdutosTabelasRepository.CallDbProcMergeSync(procName, tableName, database);
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
                    nome_tabela = t1.nome_tabela,
                    ativa = t1.ativa,
                    timestamp = t1.timestamp,
                    tipo_tabela = t1.tipo_tabela
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxProdutosTabelas - T1ToObject - Erro ao converter registro: {t1.id_tabela} para objeto - {ex.Message}");
            }
        }
    }
}
