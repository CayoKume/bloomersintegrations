using BloomersMicrovixIntegrations.Saida.Core.Biz;
using BloomersMicrovixIntegrations.Saida.Microvix.Models;
using BloomersMicrovixIntegrations.Saida.Microvix.Repositorys.Interfaces;
using BloomersMicrovixIntegrations.Saida.Microvix.Services.Interfaces;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Services
{
    public class LinxProdutosInventarioService<T1> : ILinxProdutosInventarioService<T1> where T1 : LinxProdutosInventario, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly ILinxProdutosInventarioRepository<LinxProdutosInventario> _linxProdutosInventarioRepository;

        public LinxProdutosInventarioService(ILinxProdutosInventarioRepository<LinxProdutosInventario> linxProdutosInventarioRepository)
            => (_linxProdutosInventarioRepository) = (linxProdutosInventarioRepository);

        public List<T1?> DeserializeResponse(List<Dictionary<string, string>> registros)
        {
            var list = new List<T1?>();

            for (int i = 0; i < registros.Count; i++)
            {
                try
                {
                    list.Add(new T1
                    {
                        lastupdateon = DateTime.Now,
                        portal = registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First(),
                        cnpj_emp = registros[i].Where(pair => pair.Key == "cnpj_emp").Select(pair => pair.Value).First(),
                        cod_produto = registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First(),
                        cod_barra = registros[i].Where(pair => pair.Key == "cod_barra").Select(pair => pair.Value).First(),
                        quantidade = registros[i].Where(pair => pair.Key == "quantidade").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "quantidade").Select(pair => pair.Value).First(),
                        cod_deposito = registros[i].Where(pair => pair.Key == "cod_deposito").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_deposito").Select(pair => pair.Value).First(),
                        empresa = registros[i].Where(pair => pair.Key == "empresa").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "empresa").Select(pair => pair.Value).First()
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First();
                    throw new Exception($"LinxProdutosInventario - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
                }
            }

            return list;
        }

        public async Task IntegraRegistros(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _linxProdutosInventarioRepository.GetParameters(tableName, "parameters_lastday");

                var cnpjs = await _linxProdutosInventarioRepository.GetEmpresas();

                foreach (var cnpj in cnpjs)
                {
                    var depositos = await _linxProdutosInventarioRepository.GetCodDepositos();

                    foreach (var deposito in depositos)
                    {
                        var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[0]", "0").Replace("[codigo_despoito]", deposito).Replace("[data_inicio]", $"{DateTime.Today.ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_empresa);
                        var registros = APICaller.DeserializeXML(response);

                        if (registros.Count() > 0)
                        {
                            var listResults = DeserializeResponse(registros);
                            if (listResults.Count() > 0)
                            {
                                var list = listResults.ConvertAll(new Converter<T1, LinxProdutosInventario>(T1ToObject));
                                _linxProdutosInventarioRepository.BulkInsertIntoTableRaw(list, tableName, database);
                            }
                        }
                    } 
                }
                await _linxProdutosInventarioRepository.CallDbProcMerge(procName, tableName, database);
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
                PARAMETERS = _linxProdutosInventarioRepository.GetParametersSync(tableName, "parameters_lastday");

                var cnpjs = _linxProdutosInventarioRepository.GetEmpresasSync();

                foreach (var cnpj in cnpjs)
                {
                    var depositos = _linxProdutosInventarioRepository.GetCodDepositosSync();

                    foreach (var deposito in depositos)
                    {
                        var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[0]", "0").Replace("[codigo_despoito]", deposito).Replace("[data_inicio]", $"{DateTime.Today.ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_empresa);
                        var registros = APICaller.DeserializeXML(response);

                        if (registros.Count() > 0)
                        {
                            var listResults = DeserializeResponse(registros);
                            if (listResults.Count() > 0)
                            {
                                var list = listResults.ConvertAll(new Converter<T1, LinxProdutosInventario>(T1ToObject));
                                _linxProdutosInventarioRepository.BulkInsertIntoTableRaw(list, tableName, database);
                            }
                        }
                    }
                }
                _linxProdutosInventarioRepository.CallDbProcMergeSync(procName, tableName, database);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> IntegraRegistrosIndividual(string tableName, string procName, string database, string identificador, string identificador2, string cnpj_emp)
        {
            try
            {
                PARAMETERS = await _linxProdutosInventarioRepository.GetParameters(tableName, "parameters_manual");

                string response = APICaller.CallLinxAPI(PARAMETERS.Replace("[codigo_deposito]", $"{identificador}").Replace("[cod_produto]", $"{identificador2}").Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                var registros = APICaller.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    await _linxProdutosInventarioRepository.InsereRegistroIndividual(registro[0], tableName, database);
                    await _linxProdutosInventarioRepository.CallDbProcMerge(procName, tableName, database);
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

        public bool IntegraRegistrosIndividualSync(string tableName, string procName, string database, string identificador, string identificador2, string cnpj_emp)
        {
            try
            {
                PARAMETERS = _linxProdutosInventarioRepository.GetParametersSync(tableName, "parameters_manual");

                string response = APICaller.CallLinxAPI(PARAMETERS.Replace("[codigo_deposito]", $"{identificador}").Replace("[cod_produto]", $"{identificador2}").Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                var registros = APICaller.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    _linxProdutosInventarioRepository.InsereRegistroIndividualSync(registro[0], tableName, database);
                    _linxProdutosInventarioRepository.CallDbProcMergeSync(procName, tableName, database);
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

        public T1? T1ToObject(T1 t1)
        {
            try
            {
                return new T1
                {
                    lastupdateon = t1.lastupdateon,
                    portal = t1.portal,
                    cnpj_emp = t1.cnpj_emp,
                    cod_produto = t1.cod_produto,
                    cod_barra = t1.cod_barra,
                    quantidade = t1.quantidade,
                    cod_deposito = t1.cod_deposito,
                    empresa = t1.empresa
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxProdutosInventario - T1ToObject - Erro ao converter registro: {t1.cod_produto} para objeto - {ex.Message}");
            }
        }
    }
}
