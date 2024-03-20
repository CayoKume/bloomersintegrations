using BloomersMicrovixIntegrations.Saida.Core.Biz;
using BloomersMicrovixIntegrations.Saida.Microvix.Models;
using BloomersMicrovixIntegrations.Saida.Microvix.Repositorys.Interfaces;
using BloomersMicrovixIntegrations.Saida.Microvix.Services.Interfaces;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Services
{
    public class LinxProdutosDetalhesService<T1> : ILinxProdutosDetalhesService<T1> where T1 : LinxProdutosDetalhes, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly ILinxProdutosDetalhesRepository<LinxProdutosDetalhes> _linxProdutosDetalhesRepository;

        public LinxProdutosDetalhesService(ILinxProdutosDetalhesRepository<LinxProdutosDetalhes> linxProdutosDetalhesRepository)
            => (_linxProdutosDetalhesRepository) = (linxProdutosDetalhesRepository);

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
                        portal = registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First(),
                        cnpj_emp = registros[i].Where(pair => pair.Key == "cnpj_emp").Select(pair => pair.Value).First(),
                        cod_produto = registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First(),
                        cod_barra = registros[i].Where(pair => pair.Key == "cod_barra").Select(pair => pair.Value).First(),
                        quantidade = registros[i].Where(pair => pair.Key == "quantidade").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "quantidade").Select(pair => pair.Value).First(),
                        preco_custo = registros[i].Where(pair => pair.Key == "preco_custo").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "preco_custo").Select(pair => pair.Value).First(),
                        preco_venda = registros[i].Where(pair => pair.Key == "preco_venda").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "preco_venda").Select(pair => pair.Value).First(),
                        custo_medio = registros[i].Where(pair => pair.Key == "custo_medio").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "custo_medio").Select(pair => pair.Value).First(),
                        id_config_tributaria = registros[i].Where(pair => pair.Key == "id_config_tributaria").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_config_tributaria").Select(pair => pair.Value).First(),
                        desc_config_tributaria = registros[i].Where(pair => pair.Key == "desc_config_tributaria").Select(pair => pair.Value).First(),
                        despesas1 = registros[i].Where(pair => pair.Key == "despesas1").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "despesas1").Select(pair => pair.Value).First(),
                        qtde_minima = registros[i].Where(pair => pair.Key == "qtde_minima").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "qtde_minima").Select(pair => pair.Value).First(),
                        qtde_maxima = registros[i].Where(pair => pair.Key == "qtde_maxima").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "qtde_maxima").Select(pair => pair.Value).First(),
                        ipi = registros[i].Where(pair => pair.Key == "ipi").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "ipi").Select(pair => pair.Value).First(),
                        timestamp = registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First(),
                        custototal = registros[i].Where(pair => pair.Key == "custototal").Select(pair => pair.Value).First(),
                        empresa = registros[i].Where(pair => pair.Key == "empresa").Select(pair => pair.Value).First()
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First();
                    throw new Exception($"LinxProdutosDetalhes - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
                }
            }

            return list;
        }

        public async Task IntegraRegistros(string tableName, string procName, string database)
        {
            try
            {
                var cnpjs = await _linxProdutosDetalhesRepository.GetEmpresas();

                foreach (var cnpj in cnpjs)
                {
                    PARAMETERS = await _linxProdutosDetalhesRepository.GetParameters(tableName, "parameters_lastday");

                    var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_empresa);
                    var registros = APICaller.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<T1, LinxProdutosDetalhes>(T1ToObject));
                            _linxProdutosDetalhesRepository.BulkInsertIntoTableRaw(list, tableName, database);
                        }
                    }
                }
                await _linxProdutosDetalhesRepository.CallDbProcMerge(procName, tableName, database);
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
                var cnpjs = _linxProdutosDetalhesRepository.GetEmpresasSync();

                foreach (var cnpj in cnpjs)
                {
                    PARAMETERS = _linxProdutosDetalhesRepository.GetParametersSync(tableName, "parameters_lastday");

                    var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_empresa);
                    var registros = APICaller.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<T1, LinxProdutosDetalhes>(T1ToObject));
                            _linxProdutosDetalhesRepository.BulkInsertIntoTableRaw(list, tableName, database);
                        }
                    }
                }
                _linxProdutosDetalhesRepository.CallDbProcMergeSync(procName, tableName, database);
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
                PARAMETERS = await _linxProdutosDetalhesRepository.GetParameters(tableName, "parameters_manual");

                string response = APICaller.CallLinxAPI(PARAMETERS.Replace("[cod_produto]", $"{identificador}").Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                var registros = APICaller.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    await _linxProdutosDetalhesRepository.InsereRegistroIndividual(registro[0], tableName, database);
                    await _linxProdutosDetalhesRepository.CallDbProcMerge(procName, tableName, database);
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
                PARAMETERS = _linxProdutosDetalhesRepository.GetParametersSync(tableName, "parameters_manual");

                string response = APICaller.CallLinxAPI(PARAMETERS.Replace("[cod_produto]", $"{identificador}").Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                var registros = APICaller.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    _linxProdutosDetalhesRepository.InsereRegistroIndividualSync(registro[0], tableName, database);
                    _linxProdutosDetalhesRepository.CallDbProcMergeSync(procName, tableName, database);
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
                    cod_produto = t1.cod_produto,
                    cod_barra = t1.cod_barra,
                    quantidade = t1.quantidade,
                    preco_custo = t1.preco_custo,
                    preco_venda = t1.preco_venda,
                    custo_medio = t1.custo_medio,
                    id_config_tributaria = t1.id_config_tributaria,
                    desc_config_tributaria = t1.desc_config_tributaria,
                    despesas1 = t1.despesas1,
                    qtde_minima = t1.qtde_minima,
                    qtde_maxima = t1.qtde_maxima,
                    ipi = t1.ipi,
                    timestamp = t1.timestamp,
                    custototal = t1.custototal,
                    empresa = t1.empresa
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxProdutosDetalhes - T1ToObject - Erro ao converter registro: {t1.cod_produto} para objeto - {ex.Message}");
            }
        }
    }
}
