
using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;
using BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovix.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovix.Domain.Extensions;
using BloomersMicrovixIntegrations.LinxMicrovix.Infrastructure.Apis;

namespace BloomersMicrovixIntegrations.Application.Services.LinxMicrovix
{
    public class LinxProdutosTabelasService<TEntity> : ILinxProdutosTabelasService<TEntity> where TEntity : LinxProdutosTabelas, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly IAPICall _apiCall;
        private readonly ILinxProdutosTabelasRepository _linxProdutosTabelasRepository;

        public LinxProdutosTabelasService(ILinxProdutosTabelasRepository linxProdutosTabelasRepository, IAPICall apiCall)
            => (_linxProdutosTabelasRepository, _apiCall) = (linxProdutosTabelasRepository, apiCall);

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

        public async Task IntegraRegistrosAsync(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _linxProdutosTabelasRepository.GetParametersAsync(tableName, database, "parameters_lastday");

                var cnpjs = await _linxProdutosTabelasRepository.GetCompanysAsync(tableName, database);

                foreach (var cnpj in cnpjs)
                {
                    var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_company);
                    var response = await _apiCall.CallAPIAsync(tableName, body);
                    var registros = _apiCall.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<TEntity, LinxProdutosTabelas>(TEntityToObject));
                            _linxProdutosTabelasRepository.BulkInsertIntoTableRaw(list, tableName, database);
                            await _linxProdutosTabelasRepository.CallDbProcMergeAsync(procName, tableName, database);
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
                PARAMETERS = _linxProdutosTabelasRepository.GetParametersNotAsync(tableName, database, "parameters_lastday");

                var cnpjs = _linxProdutosTabelasRepository.GetCompanysNotAsync(tableName, database);

                foreach (var cnpj in cnpjs)
                {
                    var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_company);
                    var response = _apiCall.CallAPINotAsync(tableName, body);
                    var registros = _apiCall.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<TEntity, LinxProdutosTabelas>(TEntityToObject));
                            _linxProdutosTabelasRepository.BulkInsertIntoTableRaw(list, tableName, database);
                            _linxProdutosTabelasRepository.CallDbProcMergeNotAsync(procName, tableName, database);
                        }
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
                PARAMETERS = await _linxProdutosTabelasRepository.GetParametersAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[cod_produto]", $"{identificador}").Replace("[dt_update_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                var response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    await _linxProdutosTabelasRepository.InsereRegistroIndividualAsync(registro[0], tableName, database);
                    await _linxProdutosTabelasRepository.CallDbProcMergeAsync(procName, tableName, database);
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
                PARAMETERS = _linxProdutosTabelasRepository.GetParametersNotAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[cod_produto]", $"{identificador}").Replace("[dt_update_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                var response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    _linxProdutosTabelasRepository.InsereRegistroIndividualNotAsync(registro[0], tableName, database);
                    _linxProdutosTabelasRepository.CallDbProcMergeNotAsync(procName, tableName, database);
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
                    nome_tabela = t1.nome_tabela,
                    ativa = t1.ativa,
                    timestamp = t1.timestamp,
                    tipo_tabela = t1.tipo_tabela
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxProdutosTabelas - TEntityToObject - Erro ao converter registro: {t1.id_tabela} para objeto - {ex.Message}");
            }
        }
    }
}
