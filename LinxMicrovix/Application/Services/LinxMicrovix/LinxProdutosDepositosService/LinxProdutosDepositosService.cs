using BloomersMicrovixIntegrations.Saida.Core.Biz;
using BloomersMicrovixIntegrations.Saida.Microvix.Models;
using BloomersMicrovixIntegrations.Saida.Microvix.Repositorys.Interfaces;
using BloomersMicrovixIntegrations.Saida.Microvix.Services.Interfaces;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Services
{
    public class LinxProdutosDepositosService<T1> : ILinxProdutosDepositosService<T1> where T1 : LinxProdutosDepositos, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly ILinxProdutosDepositosRepository<LinxProdutosDepositos> _linxProdutosDepositosRepository;

        public LinxProdutosDepositosService(ILinxProdutosDepositosRepository<LinxProdutosDepositos> linxProdutosDepositosRepository)
            => (_linxProdutosDepositosRepository) = (linxProdutosDepositosRepository);

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
                        cod_deposito = registros[i].Where(pair => pair.Key == "cod_deposito").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_deposito").Select(pair => pair.Value).First(),
                        nome_deposito = registros[i].Where(pair => pair.Key == "nome_deposito").Select(pair => pair.Value).First(),
                        disponivel = registros[i].Where(pair => pair.Key == "disponivel").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "disponivel").Select(pair => pair.Value).First(),
                        disponivel_transferencia = registros[i].Where(pair => pair.Key == "disponivel_transferencia").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "disponivel_transferencia").Select(pair => pair.Value).First(),
                        timestamp = registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First(),
                        outlet = registros[i].Where(pair => pair.Key == "outlet").Select(pair => pair.Value).First(),
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = registros[i].Where(pair => pair.Key == "cod_deposito").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_deposito").Select(pair => pair.Value).First();
                    throw new Exception($"LinxProdutosDepositos - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
                }
            }

            return list;
        }

        public async Task IntegraRegistros(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _linxProdutosDepositosRepository.GetParameters(tableName, "parameters_lastday");

                var cnpjs = await _linxProdutosDepositosRepository.GetEmpresas();

                foreach (var cnpj in cnpjs)
                {
                    var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_empresa);
                    var registros = APICaller.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<T1, LinxProdutosDepositos>(T1ToObject));
                            _linxProdutosDepositosRepository.BulkInsertIntoTableRaw(list, tableName, database);
                            await _linxProdutosDepositosRepository.CallDbProcMerge(procName, tableName, database);
                        }
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
                PARAMETERS = _linxProdutosDepositosRepository.GetParametersSync(tableName, "parameters_lastday");

                var cnpjs = _linxProdutosDepositosRepository.GetEmpresasSync();

                foreach (var cnpj in cnpjs)
                {
                    var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_empresa);
                    var registros = APICaller.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<T1, LinxProdutosDepositos>(T1ToObject));
                            _linxProdutosDepositosRepository.BulkInsertIntoTableRaw(list, tableName, database);
                            _linxProdutosDepositosRepository.CallDbProcMergeSync(procName, tableName, database);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> IntegraRegistrosIndividual(string tableName, string procName, string database, string identificador, string cnpj_emp)
        {
            try
            {
                PARAMETERS = await _linxProdutosDepositosRepository.GetParameters(tableName, "parameters_manual");

                string response = APICaller.CallLinxAPI(PARAMETERS.Replace("[cod_deposito]", $"{identificador}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var registros = APICaller.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    await _linxProdutosDepositosRepository.InsereRegistroIndividual(registro[0], tableName, database);
                    await _linxProdutosDepositosRepository.CallDbProcMerge(procName, tableName, database);
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

        public bool IntegraRegistrosIndividualSync(string tableName, string procName, string database, string identificador, string cnpj_emp)
        {
            try
            {
                PARAMETERS = _linxProdutosDepositosRepository.GetParametersSync(tableName, "parameters_manual");

                string response = APICaller.CallLinxAPI(PARAMETERS.Replace("[cod_deposito]", $"{identificador}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var registros = APICaller.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    _linxProdutosDepositosRepository.InsereRegistroIndividualSync(registro[0], tableName, database);
                    _linxProdutosDepositosRepository.CallDbProcMergeSync(procName, tableName, database);
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
                    cod_deposito = t1.cod_deposito,
                    nome_deposito = t1.nome_deposito,
                    disponivel = t1.disponivel,
                    disponivel_transferencia = t1.disponivel_transferencia,
                    timestamp = t1.timestamp,
                    outlet = t1.outlet
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxProdutosDepositos - T1ToObject - Erro ao converter registro: {t1.cod_deposito} para objeto - {ex.Message}");
            }
        }
    }
}
