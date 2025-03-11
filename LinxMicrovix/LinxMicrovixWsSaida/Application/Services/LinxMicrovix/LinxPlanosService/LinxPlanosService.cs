using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Extensions;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Apis;
using System.Globalization;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxMicrovix
{
    public class LinxPlanosService<TEntity> : ILinxPlanosService<TEntity> where TEntity : LinxPlanos, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly IAPICall _apiCall;
        private readonly ILinxPlanosRepository _linxPlanosRepository;

        public LinxPlanosService(ILinxPlanosRepository linxPlanosRepository, IAPICall apiCall) =>
            (_linxPlanosRepository, _apiCall) = (linxPlanosRepository, apiCall);

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
                        plano = registros[i].Where(pair => pair.Key == "plano").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "plano").Select(pair => pair.Value).First(),
                        desc_plano = registros[i].Where(pair => pair.Key == "desc_plano").Select(pair => pair.Value).First(),
                        qtde_parcelas = registros[i].Where(pair => pair.Key == "qtde_parcelas").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "qtde_parcelas").Select(pair => pair.Value).First(),
                        prazo_entre_parcelas = registros[i].Where(pair => pair.Key == "prazo_entre_parcelas").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "prazo_entre_parcelas").Select(pair => pair.Value).First(),
                        tipo_plano = registros[i].Where(pair => pair.Key == "tipo_plano").Select(pair => pair.Value).First(),
                        indice_plano = registros[i].Where(pair => pair.Key == "indice_plano").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "indice_plano").Select(pair => pair.Value).First(),
                        cod_forma_pgto = registros[i].Where(pair => pair.Key == "cod_forma_pgto").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_forma_pgto").Select(pair => pair.Value).First(),
                        forma_pgto = registros[i].Where(pair => pair.Key == "forma_pgto").Select(pair => pair.Value).First(),
                        conta_central = registros[i].Where(pair => pair.Key == "conta_central").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "conta_central").Select(pair => pair.Value).First(),
                        tipo_transacao = registros[i].Where(pair => pair.Key == "tipo_transacao").Select(pair => pair.Value).First(),
                        taxa_financeira = registros[i].Where(pair => pair.Key == "taxa_financeira").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "taxa_financeira").Select(pair => pair.Value).First(),
                        dt_upd = registros[i].Where(pair => pair.Key == "dt_upd").Select(pair => pair.Value).First() == String.Empty ? new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar).ToString("yyyy-MM-dd HH:mm:ss") : registros[i].Where(pair => pair.Key == "dt_upd").Select(pair => pair.Value).First(),
                        desativado = registros[i].Where(pair => pair.Key == "desativado").Select(pair => pair.Value).First(),
                        usa_tef = registros[i].Where(pair => pair.Key == "usa_tef").Select(pair => pair.Value).First(),
                        timestamp = registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First(),
                    });
                }
                catch (Exception ex)
                {
                    throw new Exception($"LinxPlanos - DeserializeResponse - Erro ao deserealizar registro: {registros[i].ToString()} - {ex.Message}");
                }
            }

            return list;
        }

        public async Task IntegraRegistrosAsync(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _linxPlanosRepository.GetParametersAsync(tableName, database, "parameters_lastday");

                //var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"2000-01-01").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    if (listResults.Count() > 0)
                    {
                        var list = listResults.ConvertAll(new Converter<TEntity, LinxPlanos>(TEntityToObject));
                        _linxPlanosRepository.BulkInsertIntoTableRaw(list, tableName, database);
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
                PARAMETERS = _linxPlanosRepository.GetParametersNotAsync(tableName, database, "parameters_lastday");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    if (listResults.Count() > 0)
                    {
                        var list = listResults.ConvertAll(new Converter<TEntity, LinxPlanos>(TEntityToObject));
                        _linxPlanosRepository.BulkInsertIntoTableRaw(list, tableName, database);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> IntegraRegistrosIndividualAsync(string tableName, string procName, string database, string identificador)
        {
            try
            {
                PARAMETERS = await _linxPlanosRepository.GetParametersAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[plano]", $"{identificador}").Replace("[0]", "0").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                string response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    await _linxPlanosRepository.InsereRegistroIndividualAsync(registro[0], tableName, database);
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

        public bool IntegraRegistrosIndividualNotAsync(string tableName, string procName, string database, string identificador)
        {
            try
            {
                PARAMETERS = _linxPlanosRepository.GetParametersNotAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[plano]", $"{identificador}").Replace("[0]", "0").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                string response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registros.Count() > 0)
                {
                    _linxPlanosRepository.InsereRegistroIndividualNotAsync(registro[0], tableName, database);
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
                    plano = t1.plano,
                    desc_plano = t1.desc_plano,
                    qtde_parcelas = t1.qtde_parcelas,
                    prazo_entre_parcelas = t1.prazo_entre_parcelas,
                    tipo_plano = t1.tipo_plano,
                    indice_plano = t1.indice_plano,
                    cod_forma_pgto = t1.cod_forma_pgto,
                    forma_pgto = t1.forma_pgto,
                    conta_central = t1.conta_central,
                    tipo_transacao = t1.tipo_transacao,
                    taxa_financeira = t1.taxa_financeira,
                    dt_upd = t1.dt_upd,
                    desativado = t1.desativado,
                    usa_tef = t1.usa_tef,
                    timestamp = t1.timestamp
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxPlanos - TEntityToObject - Erro ao converter registro: {t1.plano} para objeto - {ex.Message}");
            }
        }
    }
}
