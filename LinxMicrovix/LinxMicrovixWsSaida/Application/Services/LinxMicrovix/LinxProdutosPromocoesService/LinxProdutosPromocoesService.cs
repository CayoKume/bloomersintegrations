using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Extensions;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Apis;
using System.Globalization;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxMicrovix
{
    public class LinxProdutosPromocoesService<TEntity> : ILinxProdutosPromocoesService<TEntity> where TEntity : LinxProdutosPromocoes, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly IAPICall _apiCall;
        private readonly ILinxProdutosPromocoesRepository _linxProdutosPromocoesRepository;

        public LinxProdutosPromocoesService(ILinxProdutosPromocoesRepository linxProdutosPromocoesRepository, IAPICall apiCall)
            => (_linxProdutosPromocoesRepository, _apiCall) = (linxProdutosPromocoesRepository, apiCall);

        public List<TEntity?> DeserializeResponse(List<Dictionary<string, string>> registros)
        {
            var list = new List<TEntity?>();

            for (var i = 0; i < registros.Count; i++) 
            {
                try
                {
                    list.Add(new TEntity
                    {
                        lastupdateon = DateTime.Now,
                        portal = registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First(),
                        cnpj_emp = registros[i].Where(pair => pair.Key == "cnpj_emp").Select(pair => pair.Value).First(),
                        cod_produto = registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First(),
                        preco_promocao = registros[i].Where(pair => pair.Key == "preco_promocao").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "preco_promocao").Select(pair => pair.Value).First(),
                        data_inicio_promocao = registros[i].Where(pair => pair.Key == "data_inicio_promocao").Select(pair => pair.Value).First() == String.Empty ? new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar).ToString("yyyy-MM-dd HH:mm:ss") : registros[i].Where(pair => pair.Key == "data_inicio_promocao").Select(pair => pair.Value).First(),
                        data_termino_promocao = registros[i].Where(pair => pair.Key == "data_termino_promocao").Select(pair => pair.Value).First() == String.Empty ? new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar).ToString("yyyy-MM-dd HH:mm:ss") : registros[i].Where(pair => pair.Key == "data_termino_promocao").Select(pair => pair.Value).First(),
                        data_cadastro_promocao = registros[i].Where(pair => pair.Key == "data_cadastro_promocao").Select(pair => pair.Value).First() == String.Empty ? new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar).ToString("yyyy-MM-dd HH:mm:ss") : registros[i].Where(pair => pair.Key == "data_cadastro_promocao").Select(pair => pair.Value).First(),
                        promocao_ativa = registros[i].Where(pair => pair.Key == "promocao_ativa").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "promocao_ativa").Select(pair => pair.Value).First(),
                        id_campanha = registros[i].Where(pair => pair.Key == "id_campanha").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_campanha").Select(pair => pair.Value).First(),
                        nome_campanha = registros[i].Where(pair => pair.Key == "nome_campanha").Select(pair => pair.Value).First(),
                        promocao_opcional = registros[i].Where(pair => pair.Key == "promocao_opcional").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "promocao_opcional").Select(pair => pair.Value).First(),
                        custo_total_campanha = registros[i].Where(pair => pair.Key == "custo_total_campanha").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "custo_total_campanha").Select(pair => pair.Value).First()
                    });
                }
                catch (Exception ex)
                {
                    throw new Exception($"LinxProdutosPromocoes - DeserializeResponse - Erro ao deserealizar registro: {registros[i].ToString()} - {ex.Message}");
                }
            }

            return list;
        }

        public async Task IntegraRegistrosAsync(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _linxProdutosPromocoesRepository.GetParametersAsync(tableName, database, "parameters_lastday");

                var cnpjs = await _linxProdutosPromocoesRepository.GetCompanysAsync(tableName, database);

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
                            var list = listResults.ConvertAll(new Converter<TEntity, LinxProdutosPromocoes>(TEntityToObject));
                            _linxProdutosPromocoesRepository.BulkInsertIntoTableRaw(list, tableName, database);
                        }
                    }
                }
                await _linxProdutosPromocoesRepository.CallDbProcMergeAsync(procName, tableName, database);
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
                PARAMETERS = _linxProdutosPromocoesRepository.GetParametersNotAsync(tableName, database, "parameters_lastday");

                var cnpjs = _linxProdutosPromocoesRepository.GetCompanysNotAsync(tableName, database);

                foreach (var cnpj in cnpjs)
                {
                    var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_company);
                    var response = _apiCall.CallAPINotAsync(tableName, body);
                    var registros =  _apiCall.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<TEntity, LinxProdutosPromocoes>(TEntityToObject));
                            _linxProdutosPromocoesRepository.BulkInsertIntoTableRaw(list, tableName, database);
                        }
                    }
                }
                _linxProdutosPromocoesRepository.CallDbProcMergeNotAsync(procName, tableName, database);
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
                    preco_promocao = t1.preco_promocao,
                    data_inicio_promocao = t1.data_inicio_promocao,
                    data_termino_promocao = t1.data_termino_promocao,
                    data_cadastro_promocao = t1.data_cadastro_promocao,
                    promocao_ativa = t1.promocao_ativa,
                    id_campanha = t1.id_campanha,
                    nome_campanha = t1.nome_campanha,
                    promocao_opcional = t1.promocao_opcional,
                    custo_total_campanha = t1.custo_total_campanha
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxProdutosPromocoes - TEntityToObject - Erro ao converter registro: {t1.cod_produto} para objeto - {ex.Message}");
            }
        }
    }
}
