using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Extensions;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Apis;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxMicrovix
{
    public class LinxProdutosCamposAdicionaisService<TEntity> : ILinxProdutosCamposAdicionaisService<TEntity> where TEntity : LinxProdutosCamposAdicionais, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly IAPICall _apiCall;
        private readonly ILinxProdutosCamposAdicionaisRepository _linxProdutosCamposAdicionaisRepository;

        public LinxProdutosCamposAdicionaisService(ILinxProdutosCamposAdicionaisRepository linxProdutosCamposAdicionaisRepository, IAPICall apiCall)
            => (_linxProdutosCamposAdicionaisRepository, _apiCall) = (linxProdutosCamposAdicionaisRepository, apiCall);

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

        public async Task IntegraRegistrosAsync(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _linxProdutosCamposAdicionaisRepository.GetParametersAsync(tableName, database, "parameters_lastday");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[data_inicio]", $"2000-01-01").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    if (listResults.Count() > 0)
                    {
                        var list = listResults.ConvertAll(new Converter<TEntity, LinxProdutosCamposAdicionais>(TEntityToObject));
                        _linxProdutosCamposAdicionaisRepository.BulkInsertIntoTableRaw(list, tableName, database);
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
                PARAMETERS = _linxProdutosCamposAdicionaisRepository.GetParametersNotAsync(tableName, database, "parameters_lastday");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[data_inicio]", $"2000-01-01").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    if (listResults.Count() > 0)
                    {
                        var list = listResults.ConvertAll(new Converter<TEntity, LinxProdutosCamposAdicionais>(TEntityToObject));
                        _linxProdutosCamposAdicionaisRepository.BulkInsertIntoTableRaw(list, tableName, database);
                    }
                }
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
                PARAMETERS = await _linxProdutosCamposAdicionaisRepository.GetParametersAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[data_inicio]", $"{DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}").Replace("[cod_produto]", $"{identificador}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                string response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    await _linxProdutosCamposAdicionaisRepository.InsereRegistroIndividualAsync(registro[0], tableName, database);
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
                PARAMETERS = _linxProdutosCamposAdicionaisRepository.GetParametersNotAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[data_inicio]", $"{DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}").Replace("[cod_produto]", $"{identificador}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                string response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    _linxProdutosCamposAdicionaisRepository.InsereRegistroIndividualNotAsync(registro[0], tableName, database);
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
                    cod_produto = t1.cod_produto,
                    campo = t1.campo,
                    valor = t1.valor,
                    timestamp = t1.timestamp
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxProdutosCamposAdicionais - TEntityToObject - Erro ao converter registro: {t1.cod_produto} para objeto - {ex.Message}");
            }
        }
    }
}
