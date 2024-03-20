using BloomersMicrovixIntegrations.Saida.Core.Biz;
using BloomersMicrovixIntegrations.Saida.Microvix.Models;
using BloomersMicrovixIntegrations.Saida.Microvix.Repositorys.Interfaces;
using BloomersMicrovixIntegrations.Saida.Microvix.Services.Interfaces;
using System.Globalization;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Services
{
    public class LinxProdutosPromocoesService<T1> : ILinxProdutosPromocoesService<T1> where T1 : LinxProdutosPromocoes, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly ILinxProdutosPromocoesRepository<LinxProdutosPromocoes> _linxProdutosPromocoesRepository;

        public LinxProdutosPromocoesService(ILinxProdutosPromocoesRepository<LinxProdutosPromocoes> linxProdutosPromocoesRepository)
            => (_linxProdutosPromocoesRepository) = (linxProdutosPromocoesRepository);

        public List<T1?> DeserializeResponse(List<Dictionary<string, string>> registros)
        {
            var list = new List<T1?>();

            for (var i = 0; i < registros.Count; i++) 
            {
                try
                {
                    list.Add(new T1
                    {
                        lastupdateon = DateTime.Now,
                        portal = registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First(),
                        cnpj_emp = registros[i].Where(pair => pair.Key == "cnpj_emp").Select(pair => pair.Value).First(),
                        cod_produto = registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First(),
                        preco_promocao = registros[i].Where(pair => pair.Key == "preco_promocao").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "preco_promocao").Select(pair => pair.Value).First(),
                        data_inicio_promocao = registros[i].Where(pair => pair.Key == "data_inicio_promocao").Select(pair => pair.Value).First() == String.Empty ? new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar).ToString("yyyy-MM-dd HH:mm:ss") : registros[i].Where(pair => pair.Key == "data_inicio_promocao").Select(pair => pair.Value).First(),
                        data_termino_promocao = registros[i].Where(pair => pair.Key == "data_termino_promocao").Select(pair => pair.Value).First() == String.Empty ? new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar).ToString("yyyy-MM-dd HH:mm:ss") : registros[i].Where(pair => pair.Key == "data_termino_promocao").Select(pair => pair.Value).First(),
                        data_cadastro_promocao = registros[i].Where(pair => pair.Key == "data_cadastro_promocao").Select(pair => pair.Value).First() == String.Empty ? new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar).ToString("yyyy-MM-dd HH:mm:ss") : registros[i].Where(pair => pair.Key == "data_cadastro_promocao").Select(pair => pair.Value).First(),
                        promocao_ativa = registros[i].Where(pair => pair.Key == "promocao_ativa").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "promocao_ativa").Select(pair => pair.Value).First(),
                        id_campanha = registros[i].Where(pair => pair.Key == "id_campanha").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_campanha").Select(pair => pair.Value).First(),
                        nome_campanha = registros[i].Where(pair => pair.Key == "nome_campanha").Select(pair => pair.Value).First(),
                        promocao_opcional = registros[i].Where(pair => pair.Key == "promocao_opcional").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "promocao_opcional").Select(pair => pair.Value).First(),
                        custo_total_campanha = registros[i].Where(pair => pair.Key == "custo_total_campanha").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "custo_total_campanha").Select(pair => pair.Value).First()
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First();
                    throw new Exception($"LinxProdutosPromocoes - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
                }
            }

            return list;
        }

        public async Task IntegraRegistros(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _linxProdutosPromocoesRepository.GetParameters(tableName, "parameters_lastday");

                var cnpjs = await _linxProdutosPromocoesRepository.GetEmpresas();

                foreach (var cnpj in cnpjs)
                {
                    var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_empresa);
                    var registros = APICaller.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<T1, LinxProdutosPromocoes>(T1ToObject));
                            _linxProdutosPromocoesRepository.BulkInsertIntoTableRaw(list, tableName, database);
                        }
                    }
                }
                await _linxProdutosPromocoesRepository.CallDbProcMerge(procName, tableName, database);
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
                PARAMETERS = _linxProdutosPromocoesRepository.GetParametersSync(tableName, "parameters_lastday");

                var cnpjs = _linxProdutosPromocoesRepository.GetEmpresasSync();

                foreach (var cnpj in cnpjs)
                {
                    var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_empresa);
                    var registros = APICaller.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<T1, LinxProdutosPromocoes>(T1ToObject));
                            _linxProdutosPromocoesRepository.BulkInsertIntoTableRaw(list, tableName, database);
                        }
                    }
                }
                _linxProdutosPromocoesRepository.CallDbProcMergeSync(procName, tableName, database);
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
                    preco_promocao = t1.preco_promocao,
                    data_inicio_promocao = t1.data_inicio_promocao,
                    data_termino_promocao = t1.data_termino_promocao,
                    data_cadastro_promocao = t1.data_cadastro_promocao,
                    promocao_ativa = t1.promocao_ativa,
                    id_campanha = t1.id_campanha,
                    nome_campanha = t1.nome_campanha,
                    promocao_opcional = t1.promocao_opcional,
                    custo_total_campanha = t1.custo_total_campanha
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxProdutosPromocoes - T1ToObject - Erro ao converter registro: {t1.cod_produto} para objeto - {ex.Message}");
            }
        }
    }
}
