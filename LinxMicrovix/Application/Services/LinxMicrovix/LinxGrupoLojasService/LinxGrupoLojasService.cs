using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;
using BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovix.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovix.Domain.Extensions;
using BloomersMicrovixIntegrations.LinxMicrovix.Infrastructure.Apis;

namespace BloomersMicrovixIntegrations.Application.Services.LinxMicrovix
{
    public class LinxGrupoLojasService<TEntity> : ILinxGrupoLojasService<TEntity> where TEntity : LinxGrupoLojas, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly IAPICall _apiCall;
        private readonly ILinxGrupoLojasRepository _linxGrupoLojasRepository;

        public LinxGrupoLojasService(ILinxGrupoLojasRepository linxGrupoLojasRepository, IAPICall apiCall) =>
            (_linxGrupoLojasRepository, _apiCall) = (linxGrupoLojasRepository, apiCall);

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
                        cnpj = registros[i].Where(pair => pair.Key == "CNPJ").Select(pair => pair.Value).First(),
                        nome_empresa = registros[i].Where(pair => pair.Key == "nome_empresa").Select(pair => pair.Value).First(),
                        id_empresas_rede = registros[i].Where(pair => pair.Key == "id_empresas_rede").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_empresas_rede").Select(pair => pair.Value).First(),
                        rede = registros[i].Where(pair => pair.Key == "rede").Select(pair => pair.Value).First(),
                        portal = registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First(),
                        nome_portal = registros[i].Where(pair => pair.Key == "nome_portal").Select(pair => pair.Value).First(),
                        empresa = registros[i].Where(pair => pair.Key == "empresa").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "empresa").Select(pair => pair.Value).First(),
                        lojas_proprias = registros[i].Where(pair => pair.Key == "lojas_proprias").Select(pair => pair.Value).First(),
                        classificacao_portal = registros[i].Where(pair => pair.Key == "classificacao_portal").Select(pair => pair.Value).First(),
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = registros[i].Where(pair => pair.Key == "nome_empresa").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "nome_empresa").Select(pair => pair.Value).First();
                    throw new Exception($"LinxGrupoLojas - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
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
                        var list = listResults.ConvertAll(new Converter<TEntity, LinxGrupoLojas>(TEntityToObject));
                        _linxGrupoLojasRepository.BulkInsertIntoTableRaw(list, tableName, database);
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
                        var list = listResults.ConvertAll(new Converter<TEntity, LinxGrupoLojas>(TEntityToObject));
                        _linxGrupoLojasRepository.BulkInsertIntoTableRaw(list, tableName, database);
                    }
                }
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
                    cnpj = t1.cnpj,
                    nome_empresa = t1.nome_empresa,
                    id_empresas_rede = t1.id_empresas_rede,
                    rede = t1.rede,
                    portal = t1.portal,
                    nome_portal = t1.nome_portal,
                    empresa = t1.empresa,
                    classificacao_portal = t1.classificacao_portal
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxGrupoLojas - TEntityToObject - Erro ao converter registro: {t1.nome_empresa} para objeto - {ex.Message}");
            }
        }
    }
}
