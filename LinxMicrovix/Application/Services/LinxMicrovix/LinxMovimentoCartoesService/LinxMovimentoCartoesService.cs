using BloomersMicrovixIntegrations.Saida.Core.Biz;
using BloomersMicrovixIntegrations.Saida.Microvix.Models;
using BloomersMicrovixIntegrations.Saida.Microvix.Repositorys.Interfaces;
using BloomersMicrovixIntegrations.Saida.Microvix.Services.Interfaces;
using System.Globalization;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Services
{
    public class LinxMovimentoCartoesService<T1> : ILinxMovimentoCartoesService<T1> where T1 : LinxMovimentoCartoes, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly ILinxMovimentoCartoesRepository<LinxMovimentoCartoes> _linxMovimentoCartoesRepository;

        public LinxMovimentoCartoesService(ILinxMovimentoCartoesRepository<LinxMovimentoCartoes> linxMovimentoCartoesRepository)
            => (_linxMovimentoCartoesRepository) = (linxMovimentoCartoesRepository);

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
                        portal = registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First(),
                        cnpj_emp = registros[i].Where(pair => pair.Key == "cnpj_emp").Select(pair => pair.Value).First(),
                        codlojasitef = registros[i].Where(pair => pair.Key == "codlojasitef").Select(pair => pair.Value).First(),
                        data_lancamento = registros[i].Where(pair => pair.Key == "data_lancamento").Select(pair => pair.Value).First() == String.Empty ? new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar).ToString("yyyy-MM-dd HH:mm:ss") : registros[i].Where(pair => pair.Key == "data_lancamento").Select(pair => pair.Value).First(),
                        identificador = registros[i].Where(pair => pair.Key == "identificador").Select(pair => pair.Value).First(),
                        cupomfiscal = registros[i].Where(pair => pair.Key == "cupomfiscal").Select(pair => pair.Value).First(),
                        credito_debito = registros[i].Where(pair => pair.Key == "credito_debito").Select(pair => pair.Value).First(),
                        id_cartao_bandeira = registros[i].Where(pair => pair.Key == "id_cartao_bandeira").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_cartao_bandeira").Select(pair => pair.Value).First(),
                        descricao_bandeira = registros[i].Where(pair => pair.Key == "descricao_bandeira").Select(pair => pair.Value).First(),
                        valor = registros[i].Where(pair => pair.Key == "valor").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "valor").Select(pair => pair.Value).First(),
                        ordem_cartao = registros[i].Where(pair => pair.Key == "ordem_cartao").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "ordem_cartao").Select(pair => pair.Value).First(),
                        nsu_host = registros[i].Where(pair => pair.Key == "nsu_host").Select(pair => pair.Value).First(),
                        nsu_sitef = registros[i].Where(pair => pair.Key == "nsu_sitef").Select(pair => pair.Value).First(),
                        cod_autorizacao = registros[i].Where(pair => pair.Key == "cod_autorizacao").Select(pair => pair.Value).First(),
                        id_antecipacoes_financeiras = registros[i].Where(pair => pair.Key == "id_antecipacoes_financeiras").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_antecipacoes_financeiras").Select(pair => pair.Value).First(),
                        transacao_servico_terceiro = registros[i].Where(pair => pair.Key == "transacao_servico_terceiro").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "transacao_servico_terceiro").Select(pair => pair.Value).First(),
                        texto_comprovante = registros[i].Where(pair => pair.Key == "texto_comprovante").Select(pair => pair.Value).First(),
                        id_maquineta_pos = registros[i].Where(pair => pair.Key == "id_maquineta_pos").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_maquineta_pos").Select(pair => pair.Value).First(),
                        descricao_maquineta = registros[i].Where(pair => pair.Key == "descricao_maquineta").Select(pair => pair.Value).First(),
                        serie_maquineta = registros[i].Where(pair => pair.Key == "serie_maquineta").Select(pair => pair.Value).First(),
                        timestamp = registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First()
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = registros[i].Where(pair => pair.Key == "cupomfiscal").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cupomfiscal").Select(pair => pair.Value).First();
                    throw new Exception($"LinxMovimentoCartoes - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
                }
            }
            return list;
        }

        public async Task IntegraRegistros(string tableName, string procName, string database)
        {
            try
            {
                var cnpjs = await _linxMovimentoCartoesRepository.GetEmpresas();

                foreach(var cnpj in cnpjs)
                {
                    PARAMETERS = await _linxMovimentoCartoesRepository.GetParameters(tableName, "parameters_lastday");

                    var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_empresa);
                    var registros = APICaller.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<T1, LinxMovimentoCartoes>(T1ToObject));
                            _linxMovimentoCartoesRepository.BulkInsertIntoTableRaw(list, tableName, database);
                            //await _linxMovimentoCartoesRepository.CallDbProcMerge(procName, tableName, database);
                        }
                    }
                };
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
                var cnpjs = _linxMovimentoCartoesRepository.GetEmpresasSync();

                foreach (var cnpj in cnpjs)
                {
                    PARAMETERS = _linxMovimentoCartoesRepository.GetParametersSync(tableName, "parameters_lastday");

                    var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_empresa);
                    var registros = APICaller.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<T1, LinxMovimentoCartoes>(T1ToObject));
                            _linxMovimentoCartoesRepository.BulkInsertIntoTableRaw(list, tableName, database);
                            //_linxMovimentoCartoesRepository.CallDbProcMergeSync(procName, tableName, database);
                        }
                    }
                };
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
                    codlojasitef = t1.codlojasitef,
                    data_lancamento = t1.data_lancamento,
                    identificador = t1.identificador,
                    cupomfiscal = t1.cupomfiscal,
                    credito_debito = t1.credito_debito,
                    id_cartao_bandeira = t1.id_cartao_bandeira,
                    descricao_bandeira = t1.descricao_bandeira,
                    valor = t1.valor,
                    ordem_cartao = t1.ordem_cartao,
                    nsu_host = t1.nsu_host,
                    nsu_sitef = t1.nsu_sitef,
                    cod_autorizacao = t1.cod_autorizacao,
                    id_antecipacoes_financeiras = t1.id_antecipacoes_financeiras,
                    transacao_servico_terceiro = t1.transacao_servico_terceiro,
                    texto_comprovante = t1.texto_comprovante,
                    id_maquineta_pos = t1.id_maquineta_pos,
                    descricao_maquineta = t1.descricao_maquineta,
                    serie_maquineta = t1.serie_maquineta,
                    timestamp = t1.timestamp
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxMovimentoCartoes - T1ToObject - Erro ao converter registro: {t1.codlojasitef} para objeto - {ex.Message}");
            }
        }
    }
}
