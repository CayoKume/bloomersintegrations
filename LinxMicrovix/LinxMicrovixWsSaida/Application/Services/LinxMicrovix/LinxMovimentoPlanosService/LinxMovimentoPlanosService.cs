using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Extensions;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Apis;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxMicrovix
{
    public class LinxMovimentoPlanosService<TEntity> : ILinxMovimentoPlanosService<TEntity> where TEntity : LinxMovimentoPlanos, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly IAPICall _apiCall;
        private readonly ILinxMovimentoPlanosRepository _linxMovimentoPlanosRepository;

        public LinxMovimentoPlanosService(ILinxMovimentoPlanosRepository linxMovimentoPlanosRepository, IAPICall apiCall) =>
            (_linxMovimentoPlanosRepository, _apiCall) = (linxMovimentoPlanosRepository, apiCall);

        public List<TEntity?> DeserializeResponse(List<Dictionary<string, string>> registros)
        {
            var list = new List<TEntity>();

            for (int i = 0; i < registros.Count(); i++)
            {
                try
                {
                    list.Add(new TEntity
                    {
                        lastupdateon = DateTime.Now,
                        portal = registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First(),
                        cnpj_emp = registros[i].Where(pair => pair.Key == "cnpj_emp").Select(pair => pair.Value).First(),
                        identificador = registros[i].Where(pair => pair.Key == "identificador").Select(pair => pair.Value).First(),
                        plano = registros[i].Where(pair => pair.Key == "plano").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "plano").Select(pair => pair.Value).First(),
                        desc_plano = registros[i].Where(pair => pair.Key == "desc_plano").Select(pair => pair.Value).First(),
                        total = registros[i].Where(pair => pair.Key == "total").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "total").Select(pair => pair.Value).First(),
                        qtde_parcelas = registros[i].Where(pair => pair.Key == "qtde_parcelas").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "qtde_parcelas").Select(pair => pair.Value).First(),
                        indice_plano = registros[i].Where(pair => pair.Key == "indice_plano").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "indice_plano").Select(pair => pair.Value).First(),
                        cod_forma_pgto = registros[i].Where(pair => pair.Key == "cod_forma_pgto").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_forma_pgto").Select(pair => pair.Value).First(),
                        tipo_transacao = registros[i].Where(pair => pair.Key == "tipo_transacao").Select(pair => pair.Value).First(),
                        taxa_financeira = registros[i].Where(pair => pair.Key == "taxa_financeira").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "taxa_financeira").Select(pair => pair.Value).First(),
                        ordem_cartao = registros[i].Where(pair => pair.Key == "ordem_cartao").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "ordem_cartao").Select(pair => pair.Value).First(),
                        empresa = registros[i].Where(pair => pair.Key == "empresa").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "empresa").Select(pair => pair.Value).First(),
                        timestamp = registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First(),
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = registros[i].Where(pair => pair.Key == "identificador").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "identificador").Select(pair => pair.Value).First();
                    throw new Exception($"LinxMovimentoPlanos - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
                }
            }

            return list;
        }

        public async Task IntegraRegistrosAsync(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _linxMovimentoPlanosRepository.GetParametersAsync(tableName, database, "parameters_lastday");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    if (listResults.Count() > 0)
                    {
                        var list = listResults.ConvertAll(new Converter<TEntity, LinxMovimentoPlanos>(TEntityToObject));
                        _linxMovimentoPlanosRepository.BulkInsertIntoTableRaw(list, tableName, database);
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
                PARAMETERS = _linxMovimentoPlanosRepository.GetParametersNotAsync(tableName, database, "parameters_lastday");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    if (listResults.Count() > 0)
                    {
                        var list = listResults.ConvertAll(new Converter<TEntity, LinxMovimentoPlanos>(TEntityToObject));
                        _linxMovimentoPlanosRepository.BulkInsertIntoTableRaw(list, tableName, database);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> IntegraRegistrosIndividualAsync(string tableName, string procName, string database, string identificador, string identificador2)
        {
            try
            {
                PARAMETERS = await _linxMovimentoPlanosRepository.GetParametersAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[identificador]", $"{identificador}").Replace("[0]", "0").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, identificador2);
                var response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    await _linxMovimentoPlanosRepository.InsereRegistroIndividualAsync(registro[0], tableName, database);
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

        public bool IntegraRegistrosIndividualNotAsync(string tableName, string procName, string database, string identificador, string identificador2)
        {
            try
            {
                PARAMETERS = _linxMovimentoPlanosRepository.GetParametersNotAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[identificador]", $"{identificador}").Replace("[0]", "0").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, identificador2);
                string response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registros.Count() > 0)
                {
                    _linxMovimentoPlanosRepository.InsereRegistroIndividualNotAsync(registro[0], tableName, database);
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
                    identificador = t1.identificador,
                    plano = t1.plano,
                    desc_plano = t1.desc_plano,
                    total = t1.total,
                    qtde_parcelas = t1.qtde_parcelas,
                    indice_plano = t1.indice_plano,
                    cod_forma_pgto = t1.cod_forma_pgto,
                    forma_pgto = t1.forma_pgto,
                    tipo_transacao = t1.tipo_transacao,
                    taxa_financeira = t1.taxa_financeira,
                    ordem_cartao = t1.ordem_cartao,
                    timestamp = t1.timestamp,
                    empresa = t1.empresa
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxMovimentoPlanos - TEntityToObject - Erro ao converter registro: {t1.plano} para objeto - {ex.Message}");
            }
        }
    }
}
