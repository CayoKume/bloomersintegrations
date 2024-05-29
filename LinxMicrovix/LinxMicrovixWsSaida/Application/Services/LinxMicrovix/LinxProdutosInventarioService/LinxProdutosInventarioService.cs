using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Extensions;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Apis;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxMicrovix
{
    public class LinxProdutosInventarioService<TEntity> : ILinxProdutosInventarioService<TEntity> where TEntity : LinxProdutosInventario, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly IAPICall _apiCall;
        private readonly ILinxProdutosInventarioRepository _linxProdutosInventarioRepository;

        public LinxProdutosInventarioService(ILinxProdutosInventarioRepository linxProdutosInventarioRepository, IAPICall apiCall)
            => (_linxProdutosInventarioRepository, _apiCall) = (linxProdutosInventarioRepository, apiCall);

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
                        cod_deposito = registros[i].Where(pair => pair.Key == "cod_deposito").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_deposito").Select(pair => pair.Value).First(),
                        empresa = registros[i].Where(pair => pair.Key == "empresa").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "empresa").Select(pair => pair.Value).First()
                    });
                }
                catch (Exception ex)
                {
                    throw new Exception($"LinxProdutosInventario - DeserializeResponse - Erro ao deserealizar registro: {registros[i].ToString()} - {ex.Message}");
                }
            }

            return list;
        }

        public async Task IntegraRegistrosAsync(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _linxProdutosInventarioRepository.GetParametersAsync(tableName, database, "parameters_lastday");

                var cnpjs = await _linxProdutosInventarioRepository.GetCompanysAsync(tableName, database);

                foreach (var cnpj in cnpjs)
                {
                    var depositos = await _linxProdutosInventarioRepository.GetCodDepositosAsync(tableName);

                    foreach (var deposito in depositos)
                    {
                        var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[codigo_despoito]", deposito).Replace("[data_inicio]", $"{DateTime.Today.ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_company);
                        var response = await _apiCall.CallAPIAsync(tableName, body);
                        var registros = _apiCall.DeserializeXML(response);

                        if (registros.Count() > 0)
                        {
                            var listResults = DeserializeResponse(registros);
                            if (listResults.Count() > 0)
                            {
                                var list = listResults.ConvertAll(new Converter<TEntity, LinxProdutosInventario>(TEntityToObject));
                                _linxProdutosInventarioRepository.BulkInsertIntoTableRaw(list, tableName, database);
                            }
                        }
                    } 
                }
                await _linxProdutosInventarioRepository.CallDbProcMergeAsync(procName, tableName, database);
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
                PARAMETERS = _linxProdutosInventarioRepository.GetParametersNotAsync(tableName, database, "parameters_lastday");

                var cnpjs = _linxProdutosInventarioRepository.GetCompanysNotAsync(tableName, database);

                foreach (var cnpj in cnpjs)
                {
                    var depositos = _linxProdutosInventarioRepository.GetCodDepositosNotAsync(tableName);

                    foreach (var deposito in depositos)
                    {
                        var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[codigo_despoito]", deposito).Replace("[data_inicio]", $"{DateTime.Today.ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_company);
                        var response = _apiCall.CallAPINotAsync(tableName, body);
                        var registros = _apiCall.DeserializeXML(response);

                        if (registros.Count() > 0)
                        {
                            var listResults = DeserializeResponse(registros);
                            if (listResults.Count() > 0)
                            {
                                var list = listResults.ConvertAll(new Converter<TEntity, LinxProdutosInventario>(TEntityToObject));
                                _linxProdutosInventarioRepository.BulkInsertIntoTableRaw(list, tableName, database);
                            }
                        }
                    }
                }
                _linxProdutosInventarioRepository.CallDbProcMergeNotAsync(procName, tableName, database);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> IntegraRegistrosIndividualAsync(string tableName, string procName, string database, string identificador, string identificador2, string cnpj_emp)
        {
            try
            {
                PARAMETERS = await _linxProdutosInventarioRepository.GetParametersAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[codigo_deposito]", $"{identificador}").Replace("[cod_produto]", $"{identificador2}").Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                var response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    await _linxProdutosInventarioRepository.InsereRegistroIndividualAsync(registro[0], tableName, database);
                    await _linxProdutosInventarioRepository.CallDbProcMergeAsync(procName, tableName, database);
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

        public bool IntegraRegistrosIndividualNotAsync(string tableName, string procName, string database, string identificador, string identificador2, string cnpj_emp)
        {
            try
            {
                PARAMETERS = _linxProdutosInventarioRepository.GetParametersNotAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[codigo_deposito]", $"{identificador}").Replace("[cod_produto]", $"{identificador2}").Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                var response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    _linxProdutosInventarioRepository.InsereRegistroIndividualNotAsync(registro[0], tableName, database);
                    _linxProdutosInventarioRepository.CallDbProcMergeNotAsync(procName, tableName, database);
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
                    cod_deposito = t1.cod_deposito,
                    empresa = t1.empresa
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxProdutosInventario - TEntityToObject - Erro ao converter registro: {t1.cod_produto} para objeto - {ex.Message}");
            }
        }
    }
}
