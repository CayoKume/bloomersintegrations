using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;
using BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovix.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovix.Domain.Extensions;
using BloomersMicrovixIntegrations.LinxMicrovix.Infrastructure.Apis;
using System.Globalization;

namespace BloomersMicrovixIntegrations.Application.Services.LinxMicrovix
{
    public class LinxProdutosService<TEntity> : ILinxProdutosService<TEntity> where TEntity : LinxProdutos, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly IAPICall _apiCall;
        private readonly ILinxProdutosRepository _linxProdutosRepository;

        public LinxProdutosService(ILinxProdutosRepository linxProdutosRepository, IAPICall apiCall)
            => (_linxProdutosRepository, _apiCall) = (linxProdutosRepository, apiCall);

        public List<TEntity?> DeserializeResponse(List<Dictionary<string, string>> registros)
        {
            var list = new List<TEntity?>();

            for (var i = 0;  i < registros.Count; i++)
            {
                try
                {
                    list.Add(new TEntity
                    {
                        lastupdateon = DateTime.Now,
                        portal = registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First(),
                        cod_produto = registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First(),
                        cod_barra = registros[i].Where(pair => pair.Key == "cod_barra").Select(pair => pair.Value).First(),
                        nome = registros[i].Where(pair => pair.Key == "nome").Select(pair => pair.Value).First(),
                        ncm = registros[i].Where(pair => pair.Key == "ncm").Select(pair => pair.Value).First(),
                        cest = registros[i].Where(pair => pair.Key == "cest").Select(pair => pair.Value).First(),
                        referencia = registros[i].Where(pair => pair.Key == "referencia").Select(pair => pair.Value).First(),
                        cod_auxiliar = registros[i].Where(pair => pair.Key == "cod_auxiliar").Select(pair => pair.Value).First(),
                        unidade = registros[i].Where(pair => pair.Key == "unidade").Select(pair => pair.Value).First(),
                        desc_cor = registros[i].Where(pair => pair.Key == "desc_cor").Select(pair => pair.Value).First(),
                        desc_tamanho = registros[i].Where(pair => pair.Key == "desc_tamanho").Select(pair => pair.Value).First(),
                        desc_setor = registros[i].Where(pair => pair.Key == "desc_setor").Select(pair => pair.Value).First(),
                        desc_linha = registros[i].Where(pair => pair.Key == "desc_linha").Select(pair => pair.Value).First(),
                        desc_marca = registros[i].Where(pair => pair.Key == "desc_marca").Select(pair => pair.Value).First(),
                        desc_colecao = registros[i].Where(pair => pair.Key == "desc_colecao").Select(pair => pair.Value).First(),
                        dt_update = registros[i].Where(pair => pair.Key == "dt_update").Select(pair => pair.Value).First() == String.Empty ? new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar).ToString("yyyy-MM-dd HH:mm:ss") : registros[i].Where(pair => pair.Key == "dt_update").Select(pair => pair.Value).First(),
                        cod_fornecedor = registros[i].Where(pair => pair.Key == "cod_fornecedor").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_fornecedor").Select(pair => pair.Value).First(),
                        desativado = registros[i].Where(pair => pair.Key == "desativado").Select(pair => pair.Value).First(),
                        desc_espessura = registros[i].Where(pair => pair.Key == "desc_espessura").Select(pair => pair.Value).First(),
                        id_espessura = registros[i].Where(pair => pair.Key == "id_espessura").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_espessura").Select(pair => pair.Value).First(),
                        desc_classificacao = registros[i].Where(pair => pair.Key == "desc_classificacao").Select(pair => pair.Value).First(),
                        id_classificacao = registros[i].Where(pair => pair.Key == "id_classificacao").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_classificacao").Select(pair => pair.Value).First(),
                        origem_mercadoria = registros[i].Where(pair => pair.Key == "origem_mercadoria").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "origem_mercadoria").Select(pair => pair.Value).First(),
                        peso_liquido = registros[i].Where(pair => pair.Key == "peso_liquido").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "peso_liquido").Select(pair => pair.Value).First(),
                        peso_bruto = registros[i].Where(pair => pair.Key == "peso_bruto").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "peso_bruto").Select(pair => pair.Value).First(),
                        id_cor = registros[i].Where(pair => pair.Key == "id_cor").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_cor").Select(pair => pair.Value).First(),
                        id_tamanho = registros[i].Where(pair => pair.Key == "id_tamanho").Select(pair => pair.Value).First(),
                        id_setor = registros[i].Where(pair => pair.Key == "id_setor").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_setor").Select(pair => pair.Value).First(),
                        id_linha = registros[i].Where(pair => pair.Key == "id_linha").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_linha").Select(pair => pair.Value).First(),
                        id_marca = registros[i].Where(pair => pair.Key == "id_marca").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_marca").Select(pair => pair.Value).First(),
                        id_colecao = registros[i].Where(pair => pair.Key == "id_colecao").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_colecao").Select(pair => pair.Value).First(),
                        dt_inclusao = registros[i].Where(pair => pair.Key == "dt_inclusao").Select(pair => pair.Value).First() == String.Empty ? new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar).ToString("yyyy-MM-dd HH:mm:ss") : registros[i].Where(pair => pair.Key == "dt_inclusao").Select(pair => pair.Value).First(),
                        timestamp = registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First(),
                        fator_conversao = registros[i].Where(pair => pair.Key == "fator_conversao").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "fator_conversao").Select(pair => pair.Value).First(),
                        codigo_integracao_ws = registros[i].Where(pair => pair.Key == "codigo_integracao_ws").Select(pair => pair.Value).First(),
                        codigo_integracao_reshop = registros[i].Where(pair => pair.Key == "codigo_integracao_reshop").Select(pair => pair.Value).First(),
                        id_produtos_opticos_tipo = registros[i].Where(pair => pair.Key == "id_produtos_opticos_tipo").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_produtos_opticos_tipo").Select(pair => pair.Value).First(),
                        id_sped_tipo_item = registros[i].Where(pair => pair.Key == "id_sped_tipo_item").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_sped_tipo_item").Select(pair => pair.Value).First(),
                        componente = registros[i].Where(pair => pair.Key == "componente").Select(pair => pair.Value).First(),
                        altura_para_frete = registros[i].Where(pair => pair.Key == "altura_para_frete").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "altura_para_frete").Select(pair => pair.Value).First(),
                        largura_para_frete = registros[i].Where(pair => pair.Key == "largura_para_frete").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "largura_para_frete").Select(pair => pair.Value).First(),
                        comprimento_para_frete = registros[i].Where(pair => pair.Key == "comprimento_para_frete").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "comprimento_para_frete").Select(pair => pair.Value).First(),
                        loja_virtual = registros[i].Where(pair => pair.Key == "loja_virtual").Select(pair => pair.Value).First(),
                        codigoproduto_io = registros[i].Where(pair => pair.Key == "codigoproduto_io").Select(pair => pair.Value).First(),
                        cod_comprador = registros[i].Where(pair => pair.Key == "cod_comprador").Select(pair => pair.Value).First(),
                        altura = registros[i].Where(pair => pair.Key == "altura_para_frete").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "altura_para_frete").Select(pair => pair.Value).First(),
                        largura = registros[i].Where(pair => pair.Key == "largura_para_frete").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "largura_para_frete").Select(pair => pair.Value).First(),
                        comprimento = registros[i].Where(pair => pair.Key == "comprimento_para_frete").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "comprimento_para_frete").Select(pair => pair.Value).First(),
                        codigo_integracao_oms = registros[i].Where(pair => pair.Key == "codigo_integracao_oms").Select(pair => pair.Value).First()
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First();
                    throw new Exception($"LinxProdutos - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
                }
            }

            return list;
        }

        public async Task IntegraRegistrosAsync(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _linxProdutosRepository.GetParametersAsync(tableName, database, "parameters_lastday");

                string[] idsSetores = { "2", "4" };

                foreach (var idSetor in idsSetores)
                {
                    var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[id_setor]", idSetor).Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                    var response = await _apiCall.CallAPIAsync(tableName, body);
                    var registros = _apiCall.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<TEntity, LinxProdutos>(TEntityToObject));
                            _linxProdutosRepository.BulkInsertIntoTableRaw(list, tableName, database);
                            await _linxProdutosRepository.CallDbProcMergeAsync(procName, tableName, database);
                        }
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
                PARAMETERS = _linxProdutosRepository.GetParametersNotAsync(tableName, database, "parameters_lastday");

                string[] idsSetores = { "2", "4" };

                foreach (var idSetor in idsSetores)
                {
                    var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[id_setor]", idSetor).Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                    var response = _apiCall.CallAPINotAsync(tableName, body);
                    var registros = _apiCall.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<TEntity, LinxProdutos>(TEntityToObject));
                            _linxProdutosRepository.BulkInsertIntoTableRaw(list, tableName, database);
                            _linxProdutosRepository.CallDbProcMergeNotAsync(procName, tableName, database);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> IntegraRegistrosIndividualAsync(string tableName, string procName, string database, string identificador, string cnpj_emp)
        {
            try
            {
                PARAMETERS = await _linxProdutosRepository.GetParametersAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[cod_produto]", $"{identificador}").Replace("[dt_update_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                var response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    await _linxProdutosRepository.InsereRegistroIndividualAsync(registro[0], tableName, database);
                    await _linxProdutosRepository.CallDbProcMergeAsync(procName, tableName, database);
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
            try
            {
                PARAMETERS = _linxProdutosRepository.GetParametersNotAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[cod_produto]", $"{identificador}").Replace("[dt_update_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                var response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    _linxProdutosRepository.InsereRegistroIndividualNotAsync(registro[0], tableName, database);
                    _linxProdutosRepository.CallDbProcMergeNotAsync(procName, tableName, database);
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
                    cod_produto = t1.cod_produto,
                    cod_barra = t1.cod_barra,
                    nome = t1.nome,
                    ncm = t1.ncm,
                    cest = t1.cest,
                    referencia = t1.referencia,
                    cod_auxiliar = t1.cod_auxiliar,
                    unidade = t1.unidade,
                    desc_cor = t1.desc_cor,
                    desc_tamanho = t1.desc_tamanho,
                    desc_setor = t1.desc_setor,
                    desc_linha = t1.desc_linha,
                    desc_marca = t1.desc_marca,
                    desc_colecao = t1.desc_colecao,
                    dt_update = t1.dt_update,
                    cod_fornecedor = t1.cod_fornecedor,
                    desativado = t1.desativado,
                    desc_espessura = t1.desc_espessura,
                    id_espessura = t1.id_espessura,
                    desc_classificacao = t1.desc_classificacao,
                    id_classificacao = t1.id_classificacao,
                    origem_mercadoria = t1.origem_mercadoria,
                    peso_liquido = t1.peso_liquido,
                    peso_bruto = t1.peso_bruto,
                    id_cor = t1.id_cor,
                    id_tamanho = t1.id_tamanho,
                    id_setor = t1.id_setor,
                    id_linha = t1.id_linha,
                    id_marca = t1.id_marca,
                    id_colecao = t1.id_colecao,
                    dt_inclusao = t1.dt_inclusao,
                    timestamp = t1.timestamp,
                    fator_conversao = t1.fator_conversao,
                    codigo_integracao_ws = t1.codigo_integracao_ws,
                    codigo_integracao_reshop = t1.codigo_integracao_reshop,
                    id_produtos_opticos_tipo = t1.id_produtos_opticos_tipo,
                    id_sped_tipo_item = t1.id_sped_tipo_item,
                    componente = t1.componente,
                    altura_para_frete = t1.altura_para_frete,
                    largura_para_frete = t1.largura_para_frete,
                    comprimento_para_frete = t1.comprimento_para_frete,
                    loja_virtual = t1.loja_virtual,
                    codigoproduto_io = t1.codigoproduto_io,
                    cod_comprador = t1.cod_comprador,
                    altura = t1.altura,
                    largura = t1.largura,
                    comprimento = t1.comprimento,
                    codigo_integracao_oms = t1.codigo_integracao_oms
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxProdutos - TEntityToObject - Erro ao converter registro: {t1.cod_produto} para objeto - {ex.Message}");
            }
        }
    }
}
