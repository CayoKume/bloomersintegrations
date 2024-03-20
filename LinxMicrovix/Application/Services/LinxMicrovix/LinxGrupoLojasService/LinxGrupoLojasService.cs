using BloomersMicrovixIntegrations.Saida.Core.Biz;
using BloomersMicrovixIntegrations.Saida.Microvix.Models;
using BloomersMicrovixIntegrations.Saida.Microvix.Repositorys.Interfaces;
using BloomersMicrovixIntegrations.Saida.Microvix.Services.Interfaces;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Services
{
    public class LinxGrupoLojasService<T1> : ILinxGrupoLojasService<T1> where T1 : LinxGrupoLojas, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly ILinxGrupoLojasRepository<LinxGrupoLojas> _linxGrupoLojasRepository;

        public LinxGrupoLojasService(ILinxGrupoLojasRepository<LinxGrupoLojas> linxGrupoLojasRepository) =>
            _linxGrupoLojasRepository = linxGrupoLojasRepository;

        public List<T1?> DeserializeResponse(List<Dictionary<string, string>> registros)
        {
            var list = new List<T1>();

            for (int i = 0; i < registros.Count(); i++)
            {
                try
                {
                    list.Add(new T1
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

        public async Task IntegraRegistros(string tableName, string procName, string database)
        {
            try
            {
                var response = APICaller.CallLinxAPISimplificado(tableName, AUTENTIFICACAO, CHAVE);
                var registros = APICaller.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    if (listResults.Count() > 0)
                    {
                        var list = listResults.ConvertAll(new Converter<T1, LinxGrupoLojas>(T1ToObject));
                        _linxGrupoLojasRepository.BulkInsertIntoTableRaw(list, tableName, database);
                        //await _linxGrupoLojasRepository.CallDbProcMerge(procName, tableName, database);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public void IntegraRegistrosSync(string tableName, string procName, string database)
        {
            try
            {
                var response = APICaller.CallLinxAPISimplificado(tableName, AUTENTIFICACAO, CHAVE);
                var registros = APICaller.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    if (listResults.Count() > 0)
                    {
                        var list = listResults.ConvertAll(new Converter<T1, LinxGrupoLojas>(T1ToObject));
                        _linxGrupoLojasRepository.BulkInsertIntoTableRaw(list, tableName, database);
                        //_linxGrupoLojasRepository.CallDbProcMergeSync(procName, tableName, database);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public T1? T1ToObject(T1 t1)
        {
            try
            {
                return new T1
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
                throw new Exception($"LinxGrupoLojas - T1ToObject - Erro ao converter registro: {t1.nome_empresa} para objeto - {ex.Message}");
            }
        }
    }
}
