using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Extensions;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Apis;
using System.Globalization;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxMicrovix
{
    public class LinxProdutosCodBarService<TEntity> : ILinxProdutosCodBarService<TEntity> where TEntity : LinxProdutosCodBar, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly IAPICall _apiCall;
        private readonly ILinxProdutosCodBarRepository _linxProdutosCodBarRepository;

        public LinxProdutosCodBarService(ILinxProdutosCodBarRepository linxProdutosCodBarRepository, IAPICall apiCall)
            => (_linxProdutosCodBarRepository, _apiCall) = (linxProdutosCodBarRepository, apiCall);

        public async Task<bool> IntegraRegistrosIndividualAsync(string tableName, string procName, string database, string identificador, string cnpj_emp)
        {
            try
            {
                PARAMETERS = await _linxProdutosCodBarRepository.GetParametersAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[cod_produto]", $"{identificador}"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                var response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    await _linxProdutosCodBarRepository.InsereRegistroIndividualAsync(registro[0], tableName, database);
                    //await _linxProdutosCodBarRepository.CallDbProcMergeAsync(procName, tableName, database);
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
            throw new NotImplementedException();
        }

        public Task IntegraRegistrosAsync(string tableName, string procName, string database)
        {
            throw new NotImplementedException();
        }

        public void IntegraRegistrosNotAsync(string tableName, string procName, string database)
        {
            throw new NotImplementedException();
        }

        public List<TEntity?> DeserializeResponse(List<Dictionary<string, string>> registros)
        {
            var list = new List<TEntity?>();

            for (var i = 0; i < registros.Count; i++)
            {
                try
                {
                    list.Add(new TEntity
                    {
                        lastupdateon = DateTime.Now,
                        portal = registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First(),
                        cod_produto = registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First(),
                        cod_barra = registros[i].Where(pair => pair.Key == "cod_barra").Select(pair => pair.Value).First(),
                        timestamp = registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First(),
                    });
                }
                catch (Exception ex)
                {
                    throw new Exception($"LinxProdutosCodBar - DeserializeResponse - Erro ao deserealizar registro: {registros[i].ToString()} - {ex.Message}");
                }
            }

            return list;
        }

        public TEntity? TEntityToObject(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
