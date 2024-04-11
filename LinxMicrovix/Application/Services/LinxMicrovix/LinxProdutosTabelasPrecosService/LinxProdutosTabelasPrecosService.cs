using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;
using BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovix.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovix.Domain.Extensions;
using BloomersMicrovixIntegrations.LinxMicrovix.Infrastructure.Apis;

namespace BloomersMicrovixIntegrations.Application.Services.LinxMicrovix
{
    public class LinxProdutosTabelasPrecosService<TEntity> : ILinxProdutosTabelasPrecosService<TEntity> where TEntity : LinxProdutosTabelasPrecos, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly IAPICall _apiCall;
        private readonly ILinxProdutosTabelasPrecosRepository _linxProdutosTabelasPrecosRepository;

        public LinxProdutosTabelasPrecosService(ILinxProdutosTabelasPrecosRepository linxProdutosTabelasPrecosRepository, IAPICall apiCall)
            => (_linxProdutosTabelasPrecosRepository, _apiCall) = (linxProdutosTabelasPrecosRepository, apiCall);

        public List<TEntity?> DeserializeResponse(List<Dictionary<string, string>> registros)
        {
            var list = new List<TEntity?>();

            for(var i = 0; i < registros.Count; i++)
            {
                try
                {
                    list.Add(new TEntity
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

        public async Task IntegraRegistrosAsync(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _linxProdutosTabelasPrecosRepository.GetParametersAsync(tableName, database, "parameters_lastday");

                var cnpjs = await _linxProdutosTabelasPrecosRepository.GetCompanysAsync(tableName, database);

                foreach (var cnpj in cnpjs)
                {
                    var idsTabelas = await _linxProdutosTabelasPrecosRepository.GetIdTabelaPrecoAsync(cnpj.doc_company, tableName, database);

                    foreach (var idTabela in idsTabelas)
                    {
                        var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[tabela_id]", idTabela), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_company);
                        var response = await _apiCall.CallAPIAsync(tableName, body);
                        var registros = _apiCall.DeserializeXML(response);

                        if (registros.Count() > 0)
                        {
                            var listResults = DeserializeResponse(registros);
                            if (listResults.Count() > 0)
                            {
                                var list = listResults.ConvertAll(new Converter<TEntity, LinxProdutosTabelasPrecos>(TEntityToObject));
                                _linxProdutosTabelasPrecosRepository.BulkInsertIntoTableRaw(list, tableName, database);
                                await _linxProdutosTabelasPrecosRepository.CallDbProcMergeAsync(procName, tableName, database);
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

        public void IntegraRegistrosNotAsync(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = _linxProdutosTabelasPrecosRepository.GetParametersNotAsync(tableName, database, "parameters_lastday");

                var cnpjs = _linxProdutosTabelasPrecosRepository.GetCompanysNotAsync(tableName, database);

                foreach (var cnpj in cnpjs)
                {
                    var idsTabelas = _linxProdutosTabelasPrecosRepository.GetIdTabelaPrecoNotAsync(cnpj.doc_company, tableName, database);

                    foreach (var idTabela in idsTabelas)
                    {
                        var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[tabela_id]", idTabela), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_company);
                        var response = _apiCall.CallAPINotAsync(tableName, body);
                        var registros = _apiCall.DeserializeXML(response);

                        if (registros.Count() > 0)
                        {
                            var listResults = DeserializeResponse(registros);
                            if (listResults.Count() > 0)
                            {
                                var list = listResults.ConvertAll(new Converter<TEntity, LinxProdutosTabelasPrecos>(TEntityToObject));
                                _linxProdutosTabelasPrecosRepository.BulkInsertIntoTableRaw(list, tableName, database);
                                _linxProdutosTabelasPrecosRepository.CallDbProcMergeNotAsync(procName, tableName, database);
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

        public async Task<bool> IntegraRegistrosIndividualAsync(string tableName, string procName, string database, string identificador1, string identificador2, string cnpj_emp)
        {
            try
            {
                PARAMETERS = await _linxProdutosTabelasPrecosRepository.GetParametersAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[cod_produto]", $"{identificador1}").Replace("[id_tabela]", $"{identificador2}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                var response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    await _linxProdutosTabelasPrecosRepository.InsereRegistroIndividualAsync(registro[0], tableName, database);
                    await _linxProdutosTabelasPrecosRepository.CallDbProcMergeAsync(procName, tableName, database);
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

        public bool IntegraRegistrosIndividualNotAsync(string tableName, string procName, string database, string identificador1, string identificador2, string cnpj_emp)
        {
            try
            {
                PARAMETERS = _linxProdutosTabelasPrecosRepository.GetParametersNotAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[cod_produto]", $"{identificador1}").Replace("[id_tabela]", $"{identificador2}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                string response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    _linxProdutosTabelasPrecosRepository.InsereRegistroIndividualNotAsync(registro[0], tableName, database);
                    _linxProdutosTabelasPrecosRepository.CallDbProcMergeNotAsync(procName, tableName, database);
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
                    id_tabela = t1.id_tabela,
                    cod_produto = t1.cod_produto,
                    precovenda = t1.precovenda,
                    timestamp = t1.timestamp
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxProdutosTabelasPrecos - TEntityToObject - Erro ao converter registro id_tabela: {t1.id_tabela}, cod_produto: {t1.cod_produto} para objeto - {ex.Message}");
            }
        }
    }
}
