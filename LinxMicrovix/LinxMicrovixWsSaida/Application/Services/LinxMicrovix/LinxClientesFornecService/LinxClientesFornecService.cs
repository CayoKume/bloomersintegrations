using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Extensions;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Apis;
using System.Globalization;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxMicrovix
{
    public class LinxClientesFornecService<TEntity> : ILinxClientesFornecService<TEntity> where TEntity : LinxClientesFornec, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly IAPICall _apiCall;
        private readonly ILinxClientesFornecRepository _linxClientesFornecRepository;

        public LinxClientesFornecService(ILinxClientesFornecRepository linxClientesFornecRepository, IAPICall apiCall)
            => (_linxClientesFornecRepository, _apiCall) = (linxClientesFornecRepository, apiCall);

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
                        cod_cliente = registros[i].Where(pair => pair.Key == "cod_cliente").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_cliente").Select(pair => pair.Value).First(),
                        razao_cliente = registros[i].Where(pair => pair.Key == "razao_cliente").Select(pair => pair.Value).First(),
                        nome_cliente = registros[i].Where(pair => pair.Key == "nome_cliente").Select(pair => pair.Value).First(),
                        doc_cliente = registros[i].Where(pair => pair.Key == "doc_cliente").Select(pair => pair.Value).First(),
                        tipo_cliente = registros[i].Where(pair => pair.Key == "tipo_cliente").Select(pair => pair.Value).First(),
                        endereco_cliente = registros[i].Where(pair => pair.Key == "endereco_cliente").Select(pair => pair.Value).First(),
                        numero_rua_cliente = registros[i].Where(pair => pair.Key == "numero_rua_cliente").Select(pair => pair.Value).First(),
                        complement_end_cli = registros[i].Where(pair => pair.Key == "complement_end_cli").Select(pair => pair.Value).First(),
                        bairro_cliente = registros[i].Where(pair => pair.Key == "bairro_cliente").Select(pair => pair.Value).First(),
                        cep_cliente = registros[i].Where(pair => pair.Key == "cep_cliente").Select(pair => pair.Value).First(),
                        cidade_cliente = registros[i].Where(pair => pair.Key == "cidade_cliente").Select(pair => pair.Value).First(),
                        uf_cliente = registros[i].Where(pair => pair.Key == "uf_cliente").Select(pair => pair.Value).First(),
                        pais = registros[i].Where(pair => pair.Key == "pais").Select(pair => pair.Value).First(),
                        fone_cliente = registros[i].Where(pair => pair.Key == "fone_cliente").Select(pair => pair.Value).First(),
                        email_cliente = registros[i].Where(pair => pair.Key == "email_cliente").Select(pair => pair.Value).First(),
                        sexo = registros[i].Where(pair => pair.Key == "sexo").Select(pair => pair.Value).First(),
                        data_cadastro = registros[i].Where(pair => pair.Key == "data_cadastro").Select(pair => pair.Value).First() == String.Empty ? new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar).ToString("yyyy-MM-dd HH:mm:ss") : registros[i].Where(pair => pair.Key == "data_cadastro").Select(pair => pair.Value).First(),
                        data_nascimento = registros[i].Where(pair => pair.Key == "data_nascimento").Select(pair => pair.Value).First() == String.Empty ? new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar).ToString("yyyy-MM-dd HH:mm:ss") : registros[i].Where(pair => pair.Key == "data_nascimento").Select(pair => pair.Value).First(),
                        cel_cliente = registros[i].Where(pair => pair.Key == "cel_cliente").Select(pair => pair.Value).First(),
                        ativo = registros[i].Where(pair => pair.Key == "ativo").Select(pair => pair.Value).First(),
                        dt_update = registros[i].Where(pair => pair.Key == "dt_update").Select(pair => pair.Value).First() == String.Empty ? new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar).ToString("yyyy-MM-dd HH:mm:ss") : registros[i].Where(pair => pair.Key == "dt_update").Select(pair => pair.Value).First(),
                        inscricao_estadual = registros[i].Where(pair => pair.Key == "inscricao_estadual").Select(pair => pair.Value).First(),
                        incricao_municipal = registros[i].Where(pair => pair.Key == "incricao_municipal").Select(pair => pair.Value).First(),
                        identidade_cliente = registros[i].Where(pair => pair.Key == "identidade_cliente").Select(pair => pair.Value).First(),
                        cartao_fidelidade = registros[i].Where(pair => pair.Key == "cartao_fidelidade").Select(pair => pair.Value).First(),
                        cod_ibge_municipio = registros[i].Where(pair => pair.Key == "cod_ibge_municipio").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_ibge_municipio").Select(pair => pair.Value).First(),
                        classe_cliente = registros[i].Where(pair => pair.Key == "classe_cliente").Select(pair => pair.Value).First(),
                        matricula_conveniado = registros[i].Where(pair => pair.Key == "matricula_conveniado").Select(pair => pair.Value).First(),
                        tipo_cadastro = registros[i].Where(pair => pair.Key == "tipo_cadastro").Select(pair => pair.Value).First(),
                        empresa_cadastro = registros[i].Where(pair => pair.Key == "empresa_cadastro").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "empresa_cadastro").Select(pair => pair.Value).First(),
                        id_estado_civil = registros[i].Where(pair => pair.Key == "id_estado_civil").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_estado_civil").Select(pair => pair.Value).First(),
                        fax_cliente = registros[i].Where(pair => pair.Key == "fax_cliente").Select(pair => pair.Value).First(),
                        site_cliente = registros[i].Where(pair => pair.Key == "site_cliente").Select(pair => pair.Value).First(),
                        timestamp = registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First(),
                        cliente_anonimo = registros[i].Where(pair => pair.Key == "cliente_anonimo").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cliente_anonimo").Select(pair => pair.Value).First(),
                        limite_compras = registros[i].Where(pair => pair.Key == "limite_compras").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "limite_compras").Select(pair => pair.Value).First(),
                        codigo_ws = registros[i].Where(pair => pair.Key == "codigo_ws").Select(pair => pair.Value).First(),
                        limite_credito_compra = registros[i].Where(pair => pair.Key == "limite_credito_compra").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "limite_credito_compra").Select(pair => pair.Value).First(),
                        id_classe_fiscal = registros[i].Where(pair => pair.Key == "id_classe_fiscal").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_classe_fiscal").Select(pair => pair.Value).First(),
                        obs = registros[i].Where(pair => pair.Key == "obs").Select(pair => pair.Value).First(),
                        mae = registros[i].Where(pair => pair.Key == "mae").Select(pair => pair.Value).First(),
                        cliente_contribuinte = registros[i].Where(pair => pair.Key == "cliente_contribuinte").Select(pair => pair.Value).First()
                    });
                }
                catch (Exception ex)
                {
                    throw new Exception($"LinxClientesFornec - DeserializeResponse - Erro ao deserealizar registro: {registros[i].ToString()} - {ex.Message}");
                }
            }
            
            return list;
        }

        public async Task IntegraRegistrosAsync(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _linxClientesFornecRepository.GetParametersAsync(tableName, database, "parameters_lastday");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    if (listResults.Count() > 0)
                    {
                        var list = listResults.ConvertAll(new Converter<TEntity, LinxClientesFornec>(TEntityToObject));
                        _linxClientesFornecRepository.BulkInsertIntoTableRaw(list, tableName, database);
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
                PARAMETERS = _linxClientesFornecRepository.GetParametersNotAsync(tableName, database, "parameters_lastday");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    if (listResults.Count() > 0)
                    {
                        var list = listResults.ConvertAll(new Converter<TEntity, LinxClientesFornec>(TEntityToObject));
                        _linxClientesFornecRepository.BulkInsertIntoTableRaw(list, tableName, database);
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
                PARAMETERS = await _linxClientesFornecRepository.GetParametersAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[doc_cliente]", $"{identificador}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                string response = await _apiCall.CallAPIAsync(tableName, body);
                var registro = _apiCall.DeserializeXML(response);
                var cliente = DeserializeResponse(registro);

                if (cliente.Count() > 0)
                {
                    await _linxClientesFornecRepository.InsereRegistroIndividualAsync(cliente[0], tableName, database);
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
                PARAMETERS = _linxClientesFornecRepository.GetParametersNotAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[doc_cliente]", $"{identificador}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                string response = _apiCall.CallAPINotAsync(tableName, body);
                var registro = _apiCall.DeserializeXML(response);
                var cliente = DeserializeResponse(registro);

                if (cliente.Count() > 0)
                {
                    _linxClientesFornecRepository.InsereRegistroIndividualNotAsync(cliente[0], tableName, database);
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
                    cod_cliente = t1.cod_cliente,
                    razao_cliente = t1.razao_cliente,
                    nome_cliente = t1.nome_cliente,
                    doc_cliente = t1.doc_cliente,
                    tipo_cliente = t1.tipo_cliente,
                    endereco_cliente = t1.endereco_cliente,
                    numero_rua_cliente = t1.numero_rua_cliente,
                    complement_end_cli = t1.complement_end_cli,
                    bairro_cliente = t1.bairro_cliente,
                    cep_cliente = t1.cep_cliente,
                    cidade_cliente = t1.cidade_cliente,
                    uf_cliente = t1.uf_cliente,
                    pais = t1.pais,
                    fone_cliente = t1.fone_cliente,
                    email_cliente = t1.email_cliente,
                    sexo = t1.sexo,
                    data_cadastro = t1.data_cadastro,
                    data_nascimento = t1.data_nascimento,
                    cel_cliente = t1.cel_cliente,
                    ativo = t1.ativo,
                    dt_update = t1.dt_update,
                    inscricao_estadual = t1.inscricao_estadual,
                    incricao_municipal = t1.incricao_municipal,
                    identidade_cliente = t1.identidade_cliente,
                    cartao_fidelidade = t1.cartao_fidelidade,
                    cod_ibge_municipio = t1.cod_ibge_municipio,
                    classe_cliente = t1.classe_cliente,
                    matricula_conveniado = t1.matricula_conveniado,
                    tipo_cadastro = t1.tipo_cadastro,
                    empresa_cadastro = t1.empresa_cadastro,
                    id_estado_civil = t1.id_estado_civil,
                    fax_cliente = t1.fax_cliente,
                    site_cliente = t1.site_cliente,
                    timestamp = t1.timestamp,
                    cliente_anonimo = t1.cliente_anonimo,
                    limite_compras = t1.limite_compras,
                    codigo_ws = t1.codigo_ws,
                    limite_credito_compra = t1.limite_credito_compra,
                    id_classe_fiscal = t1.id_classe_fiscal,
                    obs = t1.obs,
                    mae = t1.mae,
                    cliente_contribuinte = t1.cliente_contribuinte
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxClientesFornec - TEntityToObject - Erro ao converter registro: {t1.doc_cliente} para objeto - {ex.Message}");
            }
        }
    }
}
