using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;
using BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovix.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovix.Domain.Extensions;
using BloomersMicrovixIntegrations.LinxMicrovix.Infrastructure.Apis;
using System.Globalization;

namespace BloomersMicrovixIntegrations.Application.Services.LinxMicrovix
{
    public class LinxLojasService<TEntity> : ILinxLojasService<TEntity> where TEntity : LinxLojas, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly IAPICall _apiCall;
        private readonly ILinxLojasRepository _linxLojasRepository;

        public LinxLojasService(ILinxLojasRepository linxLojasRepository, IAPICall apiCall) =>
            (_linxLojasRepository, _apiCall) = (linxLojasRepository, apiCall);

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
                        portal = registros[i].Where(pair => pair.Key == "Portal").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "Portal").Select(pair => pair.Value).First(),
                        empresa = registros[i].Where(pair => pair.Key == "Empresa").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "Empresa").Select(pair => pair.Value).First(),
                        nome_emp = registros[i].Where(pair => pair.Key == "nome_emp").Select(pair => pair.Value).First(),
                        razao_emp = registros[i].Where(pair => pair.Key == "razao_emp").Select(pair => pair.Value).First(),
                        cnpj_emp = registros[i].Where(pair => pair.Key == "cnpj_emp").Select(pair => pair.Value).First(),
                        inscricao_emp = registros[i].Where(pair => pair.Key == "inscricao_emp").Select(pair => pair.Value).First(),
                        endereco_emp = registros[i].Where(pair => pair.Key == "endereco_emp").Select(pair => pair.Value).First(),
                        num_emp = registros[i].Where(pair => pair.Key == "num_emp").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "num_emp").Select(pair => pair.Value).First(),
                        complement_emp = registros[i].Where(pair => pair.Key == "complement_emp").Select(pair => pair.Value).First(),
                        bairro_emp = registros[i].Where(pair => pair.Key == "bairro_emp").Select(pair => pair.Value).First(),
                        cep_emp = registros[i].Where(pair => pair.Key == "cep_emp").Select(pair => pair.Value).First(),
                        cidade_emp = registros[i].Where(pair => pair.Key == "cidade_emp").Select(pair => pair.Value).First(),
                        estado_emp = registros[i].Where(pair => pair.Key == "estado_emp").Select(pair => pair.Value).First(),
                        fone_emp = registros[i].Where(pair => pair.Key == "fone_emp").Select(pair => pair.Value).First(),
                        email_emp = registros[i].Where(pair => pair.Key == "email_emp").Select(pair => pair.Value).First(),
                        cod_ibge_municipio = registros[i].Where(pair => pair.Key == "cod_ibge_municipio").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_ibge_municipio").Select(pair => pair.Value).First(),
                        data_criacao_emp = registros[i].Where(pair => pair.Key == "data_criacao_emp").Select(pair => pair.Value).First() == String.Empty ? new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar).ToString("yyyy-MM-dd HH:mm:ss") : registros[i].Where(pair => pair.Key == "data_criacao_emp").Select(pair => pair.Value).First(),
                        data_criacao_portal = registros[i].Where(pair => pair.Key == "data_criacao_portal").Select(pair => pair.Value).First() == String.Empty ? new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar).ToString("yyyy-MM-dd HH:mm:ss") : registros[i].Where(pair => pair.Key == "data_criacao_portal").Select(pair => pair.Value).First(),
                        sistema_tributacao = registros[i].Where(pair => pair.Key == "sistema_tributacao").Select(pair => pair.Value).First(),
                        regime_tributario = registros[i].Where(pair => pair.Key == "regime_tributario").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "regime_tributario").Select(pair => pair.Value).First(),
                        area_empresa = registros[i].Where(pair => pair.Key == "area_empresa").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "area_empresa").Select(pair => pair.Value).First(),
                        timestamp = registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First(),
                        sigla_empresa = registros[i].Where(pair => pair.Key == "sigla_empresa").Select(pair => pair.Value).First(),
                        id_classe_fiscal = registros[i].Where(pair => pair.Key == "id_classe_fiscal").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_classe_fiscal").Select(pair => pair.Value).First(),
                        centro_distribuicao = registros[i].Where(pair => pair.Key == "centro_distribuicao").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "centro_distribuicao").Select(pair => pair.Value).First(),
                        inscricao_municipal_emp = registros[i].Where(pair => pair.Key == "inscricao_municipal_emp").Select(pair => pair.Value).First(),
                        cnae_emp = registros[i].Where(pair => pair.Key == "cnae_emp").Select(pair => pair.Value).First(),
                        cod_cliente_linx = registros[i].Where(pair => pair.Key == "cod_cliente_linx").Select(pair => pair.Value).First(),
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = registros[i].Where(pair => pair.Key == "nome_empresa").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "nome_empresa").Select(pair => pair.Value).First();
                    throw new Exception($"LinxLojas - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
                }
            }

            return list;
        }

        public async Task IntegraRegistrosAsync(string tableName, string procName, string database)
        {
            try
            {
                var body = _apiCall.BuildBodyRequest(tableName, AUTENTIFICACAO, CHAVE);
                var response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    if (listResults.Count() > 0)
                    {
                        var list = listResults.ConvertAll(new Converter<TEntity, LinxLojas>(TEntityToObject));
                        _linxLojasRepository.BulkInsertIntoTableRaw(list, tableName, database);
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
                var body = _apiCall.BuildBodyRequest(tableName, AUTENTIFICACAO, CHAVE);
                var response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    if (listResults.Count() > 0)
                    {
                        var list = listResults.ConvertAll(new Converter<TEntity, LinxLojas>(TEntityToObject));
                        _linxLojasRepository.BulkInsertIntoTableRaw(list, tableName, database);
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
                PARAMETERS = await _linxLojasRepository.GetParametersAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(String.Empty, tableName, AUTENTIFICACAO, CHAVE, identificador);
                string response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    await _linxLojasRepository.InsereRegistroIndividualAsync(registro[0], tableName, database);
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
                PARAMETERS = _linxLojasRepository.GetParametersNotAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(String.Empty, tableName, AUTENTIFICACAO, CHAVE, identificador);
                string response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    _linxLojasRepository.InsereRegistroIndividualNotAsync(registro[0], tableName, database);
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
                    empresa = t1.empresa,
                    nome_emp = t1.nome_emp,
                    razao_emp = t1.razao_emp,
                    cnpj_emp = t1.cnpj_emp,
                    inscricao_emp = t1.inscricao_emp,
                    endereco_emp = t1.endereco_emp,
                    num_emp = t1.num_emp,
                    complement_emp = t1.complement_emp,
                    bairro_emp = t1.bairro_emp,
                    cep_emp = t1.cep_emp,
                    cidade_emp = t1.cidade_emp,
                    estado_emp = t1.estado_emp,
                    fone_emp = t1.fone_emp,
                    email_emp = t1.email_emp,
                    cod_ibge_municipio = t1.cod_ibge_municipio,
                    data_criacao_emp = t1.data_criacao_emp,
                    data_criacao_portal = t1.data_criacao_portal,
                    sistema_tributacao = t1.sistema_tributacao,
                    regime_tributario = t1.regime_tributario,
                    area_empresa = t1.area_empresa,
                    timestamp = t1.timestamp,
                    sigla_empresa = t1.sigla_empresa,
                    id_classe_fiscal = t1.id_classe_fiscal,
                    centro_distribuicao = t1.centro_distribuicao,
                    inscricao_municipal_emp = t1.inscricao_municipal_emp,
                    cnae_emp = t1.cnae_emp,
                    cod_cliente_linx = t1.cod_cliente_linx
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxLojas - TEntityToObject - Erro ao converter registro: {t1.nome_emp} para objeto - {ex.Message}");
            }
        }
    }
}
