using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxEcommerce;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Extensions;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Apis;
using System.Globalization;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxCommerce;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxCommerce
{
    public class B2CConsultaClientesService<TEntity> : IB2CConsultaClientesServices<TEntity> where TEntity : B2CConsultaClientes, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveB2C.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationB2C.ToName();
        private readonly IAPICall _apiCall;
        private readonly IB2CConsultaClientesRepository _b2CConsultaClientesRepository;

        public B2CConsultaClientesService(IB2CConsultaClientesRepository b2CConsultaClientesRepository, IAPICall apiCall) => 
            (_b2CConsultaClientesRepository, _apiCall) = (b2CConsultaClientesRepository, apiCall);

        public async Task IntegraRegistrosAsync(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _b2CConsultaClientesRepository.GetParametersAsync(tableName, database, "parameters_lastday");

                var timestamp = await _b2CConsultaClientesRepository.GetTableLastTimestampAsync(database, tableName);
                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", timestamp), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    var _listResults = listResults.ConvertAll(new Converter<TEntity, B2CConsultaClientes>(TEntityToObject));
                    var __listResults = await _b2CConsultaClientesRepository.GetRegistersExistsAsync(_listResults, tableName, database);
                    
                    for (int i = 0; i < __listResults.Count; i++)
                    {
                        _listResults.Remove(_listResults.Where(r => r.doc_cliente == __listResults[i].doc_cliente && r.timestamp == __listResults[i].timestamp).FirstOrDefault());
                    }

                    if (_listResults.Count() > 0)
                        _b2CConsultaClientesRepository.BulkInsertIntoTableRaw(_listResults, tableName, database);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void IntegraRegistrosNotAsync(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = _b2CConsultaClientesRepository.GetParametersNotAsync(tableName, database, "parameters_lastday");

                var timestamp = _b2CConsultaClientesRepository.GetTableLastTimestampNotAsync(database, tableName);
                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", timestamp), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    
                    if (listResults.Count() > 0)
                    {
                        var list = listResults.ConvertAll(new Converter<TEntity, B2CConsultaClientes>(TEntityToObject));
                        _b2CConsultaClientesRepository.BulkInsertIntoTableRaw(list, tableName, database);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> IntegraRegistrosIndividualAsync(string tableName, string procName, string database, string doc_cliente)
        {
            try
            {
                PARAMETERS = await _b2CConsultaClientesRepository.GetParametersAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[doc_cliente]", $"{doc_cliente}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = await _apiCall.CallAPIAsync(tableName, body);
                var registro = _apiCall.DeserializeXML(response);
                var cliente = DeserializeResponse(registro);

                if (cliente is not null)
                {
                    await _b2CConsultaClientesRepository.InsereRegistroIndividualAsync(cliente[0], tableName, database);
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
                PARAMETERS = _b2CConsultaClientesRepository.GetParametersNotAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[doc_cliente]", $"{identificador}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = _apiCall.CallAPINotAsync(tableName, body);
                var registro = _apiCall.DeserializeXML(response);
                var cliente = DeserializeResponse(registro);

                if (cliente.Count() > 0)
                {
                    _b2CConsultaClientesRepository.InsereRegistroIndividualNotAsync(cliente[0], tableName, database);
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

        public List<TEntity?> DeserializeResponse(List<Dictionary<string, string>> registros)
        {
            long timestamp;
            int cod_cliente_b2c, cod_cliente_erp, estado_civil_cliente, tempo_residencia, portal;
            DateTime dt_cadastro, dt_update, dt_nasc_cliente, dt_expedicao_rg;
            char uf_cliente, sexo_cliente, tipo_pessoa;
            bool ativo, receber_email;
            decimal renda;

            var list = new List<TEntity>();

            for (int i = 0; i < registros.Count(); i++)
            {
                try
                {
                    if (long.TryParse(registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First(), out long result))
                        timestamp = result;
                    else
                        timestamp = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "cod_cliente_b2c").Select(pair => pair.Value).First(), out int result_0))
                        cod_cliente_b2c = result_0;
                    else
                        cod_cliente_b2c = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "cod_cliente_erp").Select(pair => pair.Value).First(), out int result_1))
                        cod_cliente_erp = result_1;
                    else
                        cod_cliente_erp = 0;

                    if (bool.TryParse(registros[i].Where(pair => pair.Key == "ativo").Select(pair => pair.Value).First(), out bool result_2))
                        ativo = result_2;
                    else
                        ativo = false;

                    if (bool.TryParse(registros[i].Where(pair => pair.Key == "receber_email").Select(pair => pair.Value).First(), out bool result_3))
                        receber_email = result_3;
                    else
                        receber_email = false;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "estado_civil_cliente").Select(pair => pair.Value).First(), out int result_4))
                        estado_civil_cliente = result_4;
                    else
                        estado_civil_cliente = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "tempo_residencia").Select(pair => pair.Value).First(), out int result_5))
                        tempo_residencia = result_4;
                    else
                        tempo_residencia = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First(), out int result_6))
                        portal = result_6;
                    else
                        portal = 0;

                    if (DateTime.TryParse(registros[i].Where(pair => pair.Key == "dt_cadastro").Select(pair => pair.Value).First(), out DateTime result_7))
                        dt_cadastro = result_7;
                    else
                        dt_cadastro = new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar);

                    if (DateTime.TryParse(registros[i].Where(pair => pair.Key == "dt_update").Select(pair => pair.Value).First(), out DateTime result_8))
                        dt_update = result_8;
                    else
                        dt_update = new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar);

                    if (DateTime.TryParse(registros[i].Where(pair => pair.Key == "dt_nasc_cliente").Select(pair => pair.Value).First(), out DateTime result_9))
                        dt_nasc_cliente = result_9;
                    else
                        dt_nasc_cliente = new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar);

                    if (DateTime.TryParse(registros[i].Where(pair => pair.Key == "dt_expedicao_rg").Select(pair => pair.Value).First(), out DateTime result_10))
                        dt_expedicao_rg = result_10;
                    else
                        dt_expedicao_rg = new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar);

                    if (char.TryParse(registros[i].Where(pair => pair.Key == "uf_cliente").Select(pair => pair.Value).First(), out char result_11))
                        uf_cliente = result_11;
                    else
                        uf_cliente = '\0';

                    if (char.TryParse(registros[i].Where(pair => pair.Key == "sexo_cliente").Select(pair => pair.Value).First(), out char result_12))
                        sexo_cliente = result_12;
                    else
                        sexo_cliente = '\0';

                    if (char.TryParse(registros[i].Where(pair => pair.Key == "tipo_pessoa").Select(pair => pair.Value).First(), out char result_13))
                        tipo_pessoa = result_13;
                    else
                        tipo_pessoa = '\0';

                    if (decimal.TryParse(registros[i].Where(pair => pair.Key == "renda").Select(pair => pair.Value).First(), out decimal result_14))
                        renda = result_14;
                    else
                        renda = 0;

                    list.Add(new TEntity
                    {
                        lastupdateon = DateTime.Now,
                        cod_cliente_b2c = cod_cliente_b2c,
                        cod_cliente_erp = cod_cliente_erp,
                        doc_cliente = registros[i].Where(pair => pair.Key == "doc_cliente").Select(pair => pair.Value).First(),
                        nm_cliente = registros[i].Where(pair => pair.Key == "nm_cliente").Select(pair => pair.Value).First(),
                        nm_mae = registros[i].Where(pair => pair.Key == "nm_mae").Select(pair => pair.Value).First(),
                        nm_pai = registros[i].Where(pair => pair.Key == "nm_pai").Select(pair => pair.Value).First(),
                        nm_conjuge = registros[i].Where(pair => pair.Key == "nm_conjuge").Select(pair => pair.Value).First(),
                        dt_cadastro = dt_cadastro,
                        dt_nasc_cliente = dt_nasc_cliente,
                        end_cliente = registros[i].Where(pair => pair.Key == "end_cliente").Select(pair => pair.Value).First(),
                        complemento_end_cliente = registros[i].Where(pair => pair.Key == "complemento_end_cliente").Select(pair => pair.Value).First(),
                        nr_rua_cliente = registros[i].Where(pair => pair.Key == "nr_rua_cliente").Select(pair => pair.Value).First(),
                        bairro_cliente = registros[i].Where(pair => pair.Key == "bairro_cliente").Select(pair => pair.Value).First(),
                        cep_cliente = registros[i].Where(pair => pair.Key == "cep_cliente").Select(pair => pair.Value).First(),
                        cidade_cliente = registros[i].Where(pair => pair.Key == "cidade_cliente").Select(pair => pair.Value).First(),
                        uf_cliente = uf_cliente,
                        fone_cliente = registros[i].Where(pair => pair.Key == "fone_cliente").Select(pair => pair.Value).First(),
                        fone_comercial = registros[i].Where(pair => pair.Key == "fone_comercial").Select(pair => pair.Value).First(),
                        cel_cliente = registros[i].Where(pair => pair.Key == "cel_cliente").Select(pair => pair.Value).First(),
                        email_cliente = registros[i].Where(pair => pair.Key == "email_cliente").Select(pair => pair.Value).First(),
                        rg_cliente = registros[i].Where(pair => pair.Key == "rg_cliente").Select(pair => pair.Value).First(),
                        rg_orgao_emissor = registros[i].Where(pair => pair.Key == "rg_orgao_emissor").Select(pair => pair.Value).First(),
                        estado_civil_cliente = estado_civil_cliente,
                        empresa_cliente = registros[i].Where(pair => pair.Key == "empresa_cliente").Select(pair => pair.Value).First(),
                        cargo_cliente = registros[i].Where(pair => pair.Key == "cargo_cliente").Select(pair => pair.Value).First(),
                        sexo_cliente = sexo_cliente,
                        dt_update = dt_update,
                        ativo = ativo,
                        receber_email = receber_email,
                        dt_expedicao_rg = dt_expedicao_rg,
                        naturalidade = registros[i].Where(pair => pair.Key == "naturalidade").Select(pair => pair.Value).First(),
                        tempo_residencia = tempo_residencia,
                        renda = renda,
                        numero_compl_rua_cliente = registros[i].Where(pair => pair.Key == "numero_compl_rua_cliente").Select(pair => pair.Value).First(),
                        timestamp = timestamp,
                        tipo_pessoa = tipo_pessoa,
                        portal = portal
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = registros[i].Where(pair => pair.Key == "doc_cliente").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "doc_cliente").Select(pair => pair.Value).First();
                    throw new Exception($"B2CConsultaClientes - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
                }
            }
            return list;
        }

        public TEntity? TEntityToObject(TEntity t1)
        {
            try
            {
                return new TEntity
                {
                    lastupdateon = t1.lastupdateon,
                    cod_cliente_b2c = t1.cod_cliente_b2c,
                    cod_cliente_erp = t1.cod_cliente_erp,
                    doc_cliente = t1.doc_cliente,
                    nm_cliente = t1.nm_cliente,
                    nm_mae = t1.nm_mae,
                    nm_pai = t1.nm_pai,
                    nm_conjuge = t1.nm_conjuge,
                    dt_cadastro = t1.dt_cadastro,
                    dt_nasc_cliente = t1.dt_nasc_cliente,
                    end_cliente = t1.end_cliente,
                    complemento_end_cliente = t1.complemento_end_cliente,
                    nr_rua_cliente = t1.nr_rua_cliente,
                    bairro_cliente = t1.bairro_cliente,
                    cep_cliente = t1.cep_cliente,
                    cidade_cliente = t1.cidade_cliente,
                    uf_cliente = t1.uf_cliente,
                    fone_cliente = t1.fone_cliente,
                    fone_comercial = t1.fone_comercial,
                    cel_cliente = t1.cel_cliente,
                    email_cliente = t1.email_cliente,
                    rg_cliente = t1.rg_cliente,
                    rg_orgao_emissor = t1.rg_orgao_emissor,
                    estado_civil_cliente = t1.estado_civil_cliente,
                    empresa_cliente = t1.empresa_cliente,
                    cargo_cliente = t1.cargo_cliente,
                    sexo_cliente = t1.sexo_cliente,
                    dt_update = t1.dt_update,
                    ativo = t1.ativo,
                    receber_email = t1.receber_email,
                    dt_expedicao_rg = t1.dt_expedicao_rg,
                    naturalidade = t1.naturalidade,
                    tempo_residencia = t1.tempo_residencia,
                    renda = t1.renda,
                    numero_compl_rua_cliente = t1.numero_compl_rua_cliente,
                    timestamp = t1.timestamp,
                    tipo_pessoa = t1.tipo_pessoa,
                    portal = t1.portal
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"B2CConsultaClientes - TEntityToObject - Erro ao converter registro: {t1.doc_cliente} para objeto - {ex.Message}");
            }
        }
    }
}
