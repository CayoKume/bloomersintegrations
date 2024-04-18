using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxEcommerce;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxCommerce;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Extensions;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Apis;
using System.Globalization;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Enums;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxCommerce
{
    public class B2CConsultaNFeService<TEntity> : IB2CConsultaNFeService<TEntity> where TEntity : B2CConsultaNFe, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveB2C.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationB2C.ToName();
        private readonly IB2CConsultaNFeRepository _b2CConsultaNFeRepository;
        private readonly IAPICall _apiCall;

        public B2CConsultaNFeService(IB2CConsultaNFeRepository b2CConsultaNFeRepository, IAPICall apiCall) => 
            (_b2CConsultaNFeRepository, _apiCall) = (b2CConsultaNFeRepository, apiCall);

        public List<TEntity> DeserializeResponse(List<Dictionary<string, string>> registros)
        {
            Guid identificador_microvix;
            long timestamp;
            int id_nfe, id_pedido, documento, situacao, portal;
            DateTime data_emissao, dt_insert;
            bool excluido;
            decimal valor_nota, frete;

            var listNotasFiscais = new List<TEntity>();

            for (int i = 0; i < registros.Count(); i++)
            {
                try
                {
                    if (long.TryParse(registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First(), out long result))
                        timestamp = result;
                    else
                        timestamp = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "id_nfe").Select(pair => pair.Value).First(), out int result_0))
                        id_nfe = result_0;
                    else
                        id_nfe = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "id_pedido").Select(pair => pair.Value).First(), out int result_1))
                        id_pedido = result_1;
                    else
                        id_pedido = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "documento").Select(pair => pair.Value).First(), out int result_2))
                        documento = result_2;
                    else
                        documento = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "situacao").Select(pair => pair.Value).First(), out int result_3))
                        situacao = result_3;
                    else
                        situacao = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First(), out int result_4))
                        portal = result_4;
                    else
                        portal = 0;

                    if (DateTime.TryParse(registros[i].Where(pair => pair.Key == "data_emissao").Select(pair => pair.Value).First(), out DateTime result_5))
                        data_emissao = result_5;
                    else
                        data_emissao = new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar);

                    if (DateTime.TryParse(registros[i].Where(pair => pair.Key == "dt_insert").Select(pair => pair.Value).First(), out DateTime result_6))
                        dt_insert = result_6;
                    else
                        dt_insert = new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar);

                    if (bool.TryParse(registros[i].Where(pair => pair.Key == "excluido").Select(pair => pair.Value).First(), out bool result_7))
                        excluido = result_7;
                    else
                        excluido = false;

                    if (decimal.TryParse(registros[i].Where(pair => pair.Key == "valor_nota").Select(pair => pair.Value).First(), out decimal result_8))
                        valor_nota = result_8;
                    else
                        valor_nota = 0;

                    if (decimal.TryParse(registros[i].Where(pair => pair.Key == "frete").Select(pair => pair.Value).First(), out decimal result_9))
                        frete = result_9;
                    else
                        frete = 0;

                    if (Guid.TryParse(registros[i].Where(pair => pair.Key == "identificador_microvix").Select(pair => pair.Value).First(), out Guid result_10))
                        identificador_microvix = result_10;
                    else
                        identificador_microvix = new Guid();

                    listNotasFiscais.Add(new TEntity
                    {
                        lastupdateon = DateTime.Now,
                        id_nfe = id_nfe,
                        id_pedido = id_pedido,
                        documento = documento,
                        data_emissao = data_emissao,
                        chave_nfe = registros[i].Where(pair => pair.Key == "chave_nfe").Select(pair => pair.Value).First(),
                        situacao = situacao,
                        xml = registros[i].Where(pair => pair.Key == "xml").Select(pair => pair.Value).First(),
                        excluido = excluido,
                        identificador_microvix = identificador_microvix,
                        dt_insert = dt_insert,
                        valor_nota = valor_nota,
                        serie = registros[i].Where(pair => pair.Key == "serie").Select(pair => pair.Value).First(),
                        frete = frete,
                        timestamp = timestamp,
                        portal = portal,
                        nProt = registros[i].Where(pair => pair.Key == "nProt").Select(pair => pair.Value).First(),
                        codigo_modelo_nf = registros[i].Where(pair => pair.Key == "codigo_modelo_nf").Select(pair => pair.Value).First(),
                        justificativa = registros[i].Where(pair => pair.Key == "justificativa").Select(pair => pair.Value).First()
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = registros[i].Where(pair => pair.Key == "documento").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "documento").Select(pair => pair.Value).First();
                    throw new Exception($"B2CConsultaNFe - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
                }
            }
            return listNotasFiscais;
        }

        public async Task IntegraRegistrosAsync(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _b2CConsultaNFeRepository.GetParametersAsync(tableName, database, "parameters_lastday");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    var _listResults = listResults.ConvertAll(new Converter<TEntity, B2CConsultaNFe>(TEntityToObject));
                    var __listResults = await _b2CConsultaNFeRepository.GetRegistersExistsAsync(_listResults, tableName, database);

                    for (int i = 0; i < __listResults.Count; i++)
                    {
                        _listResults.Remove(_listResults.Where(r => r.chave_nfe == __listResults[i].chave_nfe && r.timestamp == __listResults[i].timestamp).FirstOrDefault());
                    }

                    if (_listResults.Count() > 0)
                        _b2CConsultaNFeRepository.BulkInsertIntoTableRaw(_listResults, tableName, database);
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
                PARAMETERS = _b2CConsultaNFeRepository.GetParametersNotAsync(tableName, database, "parameters_lastday");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);

                    if (listResults.Count() > 0)
                    {
                        var listNotasFiscais = listResults.ConvertAll(new Converter<TEntity, B2CConsultaNFe>(TEntityToObject));
                        _b2CConsultaNFeRepository.BulkInsertIntoTableRaw(listNotasFiscais, tableName, database);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> IntegraRegistrosIndividualAsync(string tableName, string procName, string database, string id_pedido)
        {
            try
            {
                PARAMETERS = await _b2CConsultaNFeRepository.GetParametersAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[id_pedido]", $"{id_pedido}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = await _apiCall.CallAPIAsync(tableName, body);
                var registro = _apiCall.DeserializeXML(response);
                var notaFiscal = DeserializeResponse(registro);

                if (notaFiscal.Count() > 0)
                {
                    await _b2CConsultaNFeRepository.InsereRegistroIndividualAsync(notaFiscal[0], tableName, database);
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
                PARAMETERS = _b2CConsultaNFeRepository.GetParametersNotAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[id_pedido]", $"{identificador}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = _apiCall.CallAPINotAsync(tableName, body);
                var registro = _apiCall.DeserializeXML(response);
                var notaFiscal = DeserializeResponse(registro);

                if (notaFiscal.Count() > 0)
                {
                    _b2CConsultaNFeRepository.InsereRegistroIndividualNotAsync(notaFiscal[0], tableName, database);
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
                    id_nfe = t1.id_nfe,
                    id_pedido = t1.id_pedido,
                    documento = t1.documento,
                    data_emissao = t1.data_emissao,
                    chave_nfe = t1.chave_nfe,
                    situacao = t1.situacao,
                    xml = t1.xml,
                    excluido = t1.excluido,
                    identificador_microvix = t1.identificador_microvix,
                    dt_insert = t1.dt_insert,
                    valor_nota = t1.valor_nota,
                    serie = t1.serie,
                    frete = t1.frete,
                    timestamp = t1.timestamp,
                    portal = t1.portal,
                    nProt = t1.nProt,
                    codigo_modelo_nf = t1.codigo_modelo_nf,
                    justificativa = t1.justificativa
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"B2CConsultaNFe - TEntityToObject - Erro ao converter registro: {t1.documento} para objeto - {ex.Message}");
            }
        }



        
    }
}
