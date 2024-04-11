
using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;
using BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovix.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovix.Domain.Extensions;
using BloomersMicrovixIntegrations.LinxMicrovix.Infrastructure.Apis;

namespace BloomersMicrovixIntegrations.Application.Services.LinxMicrovix
{
    public class LinxProdutosDepositosService<TEntity> : ILinxProdutosDepositosService<TEntity> where TEntity : LinxProdutosDepositos, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly IAPICall _apiCall;
        private readonly ILinxProdutosDepositosRepository _linxProdutosDepositosRepository;

        public LinxProdutosDepositosService(ILinxProdutosDepositosRepository linxProdutosDepositosRepository, IAPICall apiCall)
            => (_linxProdutosDepositosRepository, _apiCall) = (linxProdutosDepositosRepository, apiCall);

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
                        cod_deposito = registros[i].Where(pair => pair.Key == "cod_deposito").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_deposito").Select(pair => pair.Value).First(),
                        nome_deposito = registros[i].Where(pair => pair.Key == "nome_deposito").Select(pair => pair.Value).First(),
                        disponivel = registros[i].Where(pair => pair.Key == "disponivel").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "disponivel").Select(pair => pair.Value).First(),
                        disponivel_transferencia = registros[i].Where(pair => pair.Key == "disponivel_transferencia").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "disponivel_transferencia").Select(pair => pair.Value).First(),
                        timestamp = registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First(),
                        outlet = registros[i].Where(pair => pair.Key == "outlet").Select(pair => pair.Value).First(),
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = registros[i].Where(pair => pair.Key == "cod_deposito").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_deposito").Select(pair => pair.Value).First();
                    throw new Exception($"LinxProdutosDepositos - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
                }
            }

            return list;
        }

        public async Task IntegraRegistrosAsync(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _linxProdutosDepositosRepository.GetParametersAsync(tableName, database, "parameters_lastday");

                var cnpjs = await _linxProdutosDepositosRepository.GetCompanysAsync(tableName, database);

                foreach (var cnpj in cnpjs)
                {
                    var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_company);
                    var response = await _apiCall.CallAPIAsync(tableName, body);
                    var registros = _apiCall.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<TEntity, LinxProdutosDepositos>(TEntityToObject));
                            _linxProdutosDepositosRepository.BulkInsertIntoTableRaw(list, tableName, database);
                            await _linxProdutosDepositosRepository.CallDbProcMergeAsync(procName, tableName, database);
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
                PARAMETERS = _linxProdutosDepositosRepository.GetParametersNotAsync(tableName, database, "parameters_lastday");

                var cnpjs = _linxProdutosDepositosRepository.GetCompanysNotAsync(tableName, database);

                foreach (var cnpj in cnpjs)
                {
                    var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_company);
                    var response = _apiCall.CallAPINotAsync(tableName, body);
                    var registros = _apiCall.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<TEntity, LinxProdutosDepositos>(TEntityToObject));
                            _linxProdutosDepositosRepository.BulkInsertIntoTableRaw(list, tableName, database);
                            _linxProdutosDepositosRepository.CallDbProcMergeNotAsync(procName, tableName, database);
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
                PARAMETERS = await _linxProdutosDepositosRepository.GetParametersAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[cod_deposito]", $"{identificador}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                string response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    await _linxProdutosDepositosRepository.InsereRegistroIndividualAsync(registro[0], tableName, database);
                    await _linxProdutosDepositosRepository.CallDbProcMergeAsync(procName, tableName, database);
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
                PARAMETERS = _linxProdutosDepositosRepository.GetParametersNotAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[cod_deposito]", $"{identificador}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                string response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    _linxProdutosDepositosRepository.InsereRegistroIndividualNotAsync(registro[0], tableName, database);
                    _linxProdutosDepositosRepository.CallDbProcMergeNotAsync(procName, tableName, database);
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
                    cod_deposito = t1.cod_deposito,
                    nome_deposito = t1.nome_deposito,
                    disponivel = t1.disponivel,
                    disponivel_transferencia = t1.disponivel_transferencia,
                    timestamp = t1.timestamp,
                    outlet = t1.outlet
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxProdutosDepositos - TEntityToObject - Erro ao converter registro: {t1.cod_deposito} para objeto - {ex.Message}");
            }
        }
    }
}
