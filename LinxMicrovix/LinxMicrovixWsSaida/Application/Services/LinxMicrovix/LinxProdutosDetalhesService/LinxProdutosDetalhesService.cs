using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Extensions;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Apis;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxMicrovix
{
    public class LinxProdutosDetalhesService<TEntity> : ILinxProdutosDetalhesService<TEntity> where TEntity : LinxProdutosDetalhes, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly IAPICall _apiCall;
        private readonly ILinxProdutosDetalhesRepository _linxProdutosDetalhesRepository;

        public LinxProdutosDetalhesService(ILinxProdutosDetalhesRepository linxProdutosDetalhesRepository, IAPICall apiCall)
            => (_linxProdutosDetalhesRepository, _apiCall) = (linxProdutosDetalhesRepository, apiCall);

        public List<TEntity?> DeserializeResponse(List<Dictionary<string, string>> registros)
        {
            var list = new List<TEntity?>();

            for (int i = 0; i < registros.Count; i++)
            {
                try
                {
                    list.Add(new TEntity
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
                    throw new Exception($"LinxProdutosDetalhes - DeserializeResponse - Erro ao deserealizar registro: {registros[i].ToString()} - {ex.Message}");
                }
            }

            return list;
        }

        public async Task IntegraRegistrosAsync(string tableName, string procName, string database)
        {
            try
            {
                var cnpjs = await _linxProdutosDetalhesRepository.GetCompanysAsync(tableName, database);

                foreach (var cnpj in cnpjs)
                {
                    PARAMETERS = await _linxProdutosDetalhesRepository.GetParametersAsync(tableName, database, "parameters_lastday");

                    var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_company);
                    var response = await _apiCall.CallAPIAsync(tableName, body);
                    var registros = _apiCall.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<TEntity, LinxProdutosDetalhes>(TEntityToObject));
                            _linxProdutosDetalhesRepository.BulkInsertIntoTableRaw(list, tableName, database);
                        }
                    }
                }
                await _linxProdutosDetalhesRepository.CallDbProcMergeAsync(procName, tableName, database);
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
                var cnpjs = _linxProdutosDetalhesRepository.GetCompanysNotAsync(tableName, database);

                foreach (var cnpj in cnpjs)
                {
                    PARAMETERS = _linxProdutosDetalhesRepository.GetParametersNotAsync(tableName, database, "parameters_lastday");

                    var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_company);
                    var response = _apiCall.CallAPINotAsync(tableName, body);
                    var registros = _apiCall.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<TEntity, LinxProdutosDetalhes>(TEntityToObject));
                            _linxProdutosDetalhesRepository.BulkInsertIntoTableRaw(list, tableName, database);
                        }
                    }
                }
                _linxProdutosDetalhesRepository.CallDbProcMergeNotAsync(procName, tableName, database);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> IntegraRegistrosIndividualAsync(string tableName, string procName, string database, string identificador, string cnpj_emp)
        {
            try
            {
                PARAMETERS = await _linxProdutosDetalhesRepository.GetParametersAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[cod_produto]", $"{identificador}").Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                string response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    await _linxProdutosDetalhesRepository.InsereRegistroIndividualAsync(registro[0], tableName, database);
                    await _linxProdutosDetalhesRepository.CallDbProcMergeAsync(procName, tableName, database);
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

        public bool IntegraRegistrosIndividualNotAsync(string tableName, string procName, string database, string identificador, string cnpj_emp)
        {
            try
            {
                PARAMETERS = _linxProdutosDetalhesRepository.GetParametersNotAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[cod_produto]", $"{identificador}").Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                string response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    _linxProdutosDetalhesRepository.InsereRegistroIndividualNotAsync(registro[0], tableName, database);
                    _linxProdutosDetalhesRepository.CallDbProcMergeNotAsync(procName, tableName, database);
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
                throw new Exception($"LinxProdutosDetalhes - TEntityToObject - Erro ao converter registro: {t1.cod_produto} para objeto - {ex.Message}");
            }
        }
    }
}
