using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Extensions;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Apis;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxMicrovix
{
    public class LinxMovimentoService<TEntity> : ILinxMovimentoService<TEntity> where TEntity : LinxMovimento, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly IAPICall _apiCall;
        private readonly ILinxMovimentoRepository _linxMovimentoRepository;

        public LinxMovimentoService(ILinxMovimentoRepository linxMovimentoRepository, IAPICall apiCall)
            => (_linxMovimentoRepository, _apiCall) = (linxMovimentoRepository, apiCall);

        public List<TEntity?> DeserializeResponse(List<Dictionary<string, string>> registros)
        {
            Guid identificador = Guid.NewGuid();

            var list = new List<TEntity?>();

            for (int i = 0; i < registros.Count; i++)
            {
                try
                {
                    if (Guid.TryParse(registros[i].Where(pair => pair.Key == "identificador").Select(pair => pair.Value).First(), out Guid result))
                        identificador = result;
                    else
                        identificador = new Guid();

                    list.Add(new TEntity
                    {
                        lastupdateon = DateTime.Now,
                        portal = registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First(),
                        cnpj_emp = registros[i].Where(pair => pair.Key == "cnpj_emp").Select(pair => pair.Value).First(),
                        transacao = registros[i].Where(pair => pair.Key == "transacao").Select(pair => pair.Value).First(),
                        usuario = registros[i].Where(pair => pair.Key == "usuario").Select(pair => pair.Value).First(),
                        documento = registros[i].Where(pair => pair.Key == "documento").Select(pair => pair.Value).First(),
                        chave_nf = registros[i].Where(pair => pair.Key == "chave_nf").Select(pair => pair.Value).First(),
                        ecf = registros[i].Where(pair => pair.Key == "ecf").Select(pair => pair.Value).First(),
                        numero_serie_ecf = registros[i].Where(pair => pair.Key == "numero_serie_ecf").Select(pair => pair.Value).First(),
                        modelo_nf = registros[i].Where(pair => pair.Key == "modelo_nf").Select(pair => pair.Value).First(),
                        data_documento = registros[i].Where(pair => pair.Key == "data_documento").Select(pair => pair.Value).First(),
                        data_lancamento = registros[i].Where(pair => pair.Key == "data_lancamento").Select(pair => pair.Value).First(),
                        codigo_cliente = registros[i].Where(pair => pair.Key == "codigo_cliente").Select(pair => pair.Value).First(),
                        serie = registros[i].Where(pair => pair.Key == "serie").Select(pair => pair.Value).First(),
                        desc_cfop = registros[i].Where(pair => pair.Key == "desc_cfop").Select(pair => pair.Value).First(),
                        id_cfop = registros[i].Where(pair => pair.Key == "id_cfop").Select(pair => pair.Value).First(),
                        cod_vendedor = registros[i].Where(pair => pair.Key == "cod_vendedor").Select(pair => pair.Value).First(),
                        quantidade = registros[i].Where(pair => pair.Key == "quantidade").Select(pair => pair.Value).First(),
                        preco_custo = registros[i].Where(pair => pair.Key == "preco_custo").Select(pair => pair.Value).First(),
                        valor_liquido = registros[i].Where(pair => pair.Key == "valor_liquido").Select(pair => pair.Value).First(),
                        desconto = registros[i].Where(pair => pair.Key == "desconto").Select(pair => pair.Value).First(),
                        cst_icms = registros[i].Where(pair => pair.Key == "cst_icms").Select(pair => pair.Value).First(),
                        cst_pis = registros[i].Where(pair => pair.Key == "cst_pis").Select(pair => pair.Value).First(),
                        cst_cofins = registros[i].Where(pair => pair.Key == "cst_cofins").Select(pair => pair.Value).First(),
                        cst_ipi = registros[i].Where(pair => pair.Key == "cst_ipi").Select(pair => pair.Value).First(),
                        valor_icms = registros[i].Where(pair => pair.Key == "valor_icms").Select(pair => pair.Value).First(),
                        aliquota_icms = registros[i].Where(pair => pair.Key == "aliquota_icms").Select(pair => pair.Value).First(),
                        base_icms = registros[i].Where(pair => pair.Key == "base_icms").Select(pair => pair.Value).First(),
                        valor_pis = registros[i].Where(pair => pair.Key == "valor_pis").Select(pair => pair.Value).First(),
                        aliquota_pis = registros[i].Where(pair => pair.Key == "aliquota_pis").Select(pair => pair.Value).First(),
                        base_pis = registros[i].Where(pair => pair.Key == "base_pis").Select(pair => pair.Value).First(),
                        valor_cofins = registros[i].Where(pair => pair.Key == "valor_cofins").Select(pair => pair.Value).First(),
                        aliquota_cofins = registros[i].Where(pair => pair.Key == "aliquota_cofins").Select(pair => pair.Value).First(),
                        base_cofins = registros[i].Where(pair => pair.Key == "base_cofins").Select(pair => pair.Value).First(),
                        valor_icms_st = registros[i].Where(pair => pair.Key == "valor_icms_st").Select(pair => pair.Value).First(),
                        aliquota_icms_st = registros[i].Where(pair => pair.Key == "aliquota_icms_st").Select(pair => pair.Value).First(),
                        base_icms_st = registros[i].Where(pair => pair.Key == "base_icms_st").Select(pair => pair.Value).First(),
                        valor_ipi = registros[i].Where(pair => pair.Key == "valor_ipi").Select(pair => pair.Value).First(),
                        aliquota_ipi = registros[i].Where(pair => pair.Key == "aliquota_ipi").Select(pair => pair.Value).First(),
                        base_ipi = registros[i].Where(pair => pair.Key == "base_ipi").Select(pair => pair.Value).First(),
                        valor_total = registros[i].Where(pair => pair.Key == "valor_total").Select(pair => pair.Value).First(),
                        forma_dinheiro = registros[i].Where(pair => pair.Key == "forma_dinheiro").Select(pair => pair.Value).First(),
                        total_dinheiro = registros[i].Where(pair => pair.Key == "total_dinheiro").Select(pair => pair.Value).First(),
                        forma_cheque = registros[i].Where(pair => pair.Key == "forma_cheque").Select(pair => pair.Value).First(),
                        total_cheque = registros[i].Where(pair => pair.Key == "total_cheque").Select(pair => pair.Value).First(),
                        forma_cartao = registros[i].Where(pair => pair.Key == "forma_cartao").Select(pair => pair.Value).First(),
                        total_cartao = registros[i].Where(pair => pair.Key == "total_cartao").Select(pair => pair.Value).First(),
                        forma_crediario = registros[i].Where(pair => pair.Key == "forma_crediario").Select(pair => pair.Value).First(),
                        total_crediario = registros[i].Where(pair => pair.Key == "total_crediario").Select(pair => pair.Value).First(),
                        forma_convenio = registros[i].Where(pair => pair.Key == "forma_convenio").Select(pair => pair.Value).First(),
                        total_convenio = registros[i].Where(pair => pair.Key == "total_convenio").Select(pair => pair.Value).First(),
                        frete = registros[i].Where(pair => pair.Key == "frete").Select(pair => pair.Value).First(),
                        operacao = registros[i].Where(pair => pair.Key == "operacao").Select(pair => pair.Value).First(),
                        tipo_transacao = registros[i].Where(pair => pair.Key == "tipo_transacao").Select(pair => pair.Value).First(),
                        cod_produto = registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First(),
                        cod_barra = registros[i].Where(pair => pair.Key == "cod_barra").Select(pair => pair.Value).First(),
                        cancelado = registros[i].Where(pair => pair.Key == "cancelado").Select(pair => pair.Value).First(),
                        excluido = registros[i].Where(pair => pair.Key == "excluido").Select(pair => pair.Value).First(),
                        soma_relatorio = registros[i].Where(pair => pair.Key == "soma_relatorio").Select(pair => pair.Value).First(),
                        identificador = identificador,
                        deposito = registros[i].Where(pair => pair.Key == "deposito").Select(pair => pair.Value).First(),
                        obs = registros[i].Where(pair => pair.Key == "obs").Select(pair => pair.Value).First(),
                        preco_unitario = registros[i].Where(pair => pair.Key == "preco_unitario").Select(pair => pair.Value).First(),
                        hora_lancamento = registros[i].Where(pair => pair.Key == "hora_lancamento").Select(pair => pair.Value).First(),
                        natureza_operacao = registros[i].Where(pair => pair.Key == "natureza_operacao").Select(pair => pair.Value).First(),
                        tabela_preco = registros[i].Where(pair => pair.Key == "tabela_preco").Select(pair => pair.Value).First(),
                        nome_tabela_preco = registros[i].Where(pair => pair.Key == "nome_tabela_preco").Select(pair => pair.Value).First(),
                        cod_sefaz_situacao = registros[i].Where(pair => pair.Key == "cod_sefaz_situacao").Select(pair => pair.Value).First(),
                        desc_sefaz_situacao = registros[i].Where(pair => pair.Key == "desc_sefaz_situacao").Select(pair => pair.Value).First(),
                        protocolo_aut_nfe = registros[i].Where(pair => pair.Key == "protocolo_aut_nfe").Select(pair => pair.Value).First(),
                        dt_update = registros[i].Where(pair => pair.Key == "dt_update").Select(pair => pair.Value).First(),
                        forma_cheque_prazo = registros[i].Where(pair => pair.Key == "forma_cheque_prazo").Select(pair => pair.Value).First(),
                        total_cheque_prazo = registros[i].Where(pair => pair.Key == "total_cheque_prazo").Select(pair => pair.Value).First(),
                        cod_natureza_operacao = registros[i].Where(pair => pair.Key == "cod_natureza_operacao").Select(pair => pair.Value).First(),
                        preco_tabela_epoca = registros[i].Where(pair => pair.Key == "preco_tabela_epoca").Select(pair => pair.Value).First(),
                        desconto_total_item = registros[i].Where(pair => pair.Key == "desconto_total_item").Select(pair => pair.Value).First(),
                        conferido = registros[i].Where(pair => pair.Key == "conferido").Select(pair => pair.Value).First(),
                        transacao_pedido_venda = registros[i].Where(pair => pair.Key == "transacao_pedido_venda").Select(pair => pair.Value).First(),
                        codigo_modelo_nf = registros[i].Where(pair => pair.Key == "codigo_modelo_nf").Select(pair => pair.Value).First(),
                        acrescimo = registros[i].Where(pair => pair.Key == "acrescimo").Select(pair => pair.Value).First(),
                        mob_checkout = registros[i].Where(pair => pair.Key == "mob_checkout").Select(pair => pair.Value).First(),
                        aliquota_iss = registros[i].Where(pair => pair.Key == "aliquota_iss").Select(pair => pair.Value).First(),
                        base_iss = registros[i].Where(pair => pair.Key == "base_iss").Select(pair => pair.Value).First(),
                        ordem = registros[i].Where(pair => pair.Key == "ordem").Select(pair => pair.Value).First(),
                        codigo_rotina_origem = registros[i].Where(pair => pair.Key == "codigo_rotina_origem").Select(pair => pair.Value).First(),
                        timestamp = registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First(),
                        troco = registros[i].Where(pair => pair.Key == "troco").Select(pair => pair.Value).First(),
                        transportador = registros[i].Where(pair => pair.Key == "transportador").Select(pair => pair.Value).First(),
                        icms_aliquota_desonerado = registros[i].Where(pair => pair.Key == "icms_aliquota_desonerado").Select(pair => pair.Value).First(),
                        icms_valor_desonerado_item = registros[i].Where(pair => pair.Key == "icms_valor_desonerado_item").Select(pair => pair.Value).First(),
                        empresa = registros[i].Where(pair => pair.Key == "empresa").Select(pair => pair.Value).First(),
                        desconto_item = registros[i].Where(pair => pair.Key == "desconto_item").Select(pair => pair.Value).First(),
                        aliq_iss = registros[i].Where(pair => pair.Key == "aliq_iss").Select(pair => pair.Value).First(),
                        iss_base_item = registros[i].Where(pair => pair.Key == "iss_base_item").Select(pair => pair.Value).First(),
                        despesas = registros[i].Where(pair => pair.Key == "despesas").Select(pair => pair.Value).First(),
                        seguro_total_item = registros[i].Where(pair => pair.Key == "seguro_total_item").Select(pair => pair.Value).First(),
                        acrescimo_total_item = registros[i].Where(pair => pair.Key == "acrescimo_total_item").Select(pair => pair.Value).First(),
                        despesas_total_item = registros[i].Where(pair => pair.Key == "despesas_total_item").Select(pair => pair.Value).First(),
                        forma_pix = registros[i].Where(pair => pair.Key == "forma_pix").Select(pair => pair.Value).First(),
                        total_pix = registros[i].Where(pair => pair.Key == "total_pix").Select(pair => pair.Value).First(),
                        forma_deposito_bancario = registros[i].Where(pair => pair.Key == "forma_deposito_bancario").Select(pair => pair.Value).First(),
                        total_deposito_bancario = registros[i].Where(pair => pair.Key == "total_deposito_bancario").Select(pair => pair.Value).First()
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = registros[i].Where(pair => pair.Key == "documento").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "documento").Select(pair => pair.Value).First();
                    throw new Exception($"LinxMovimento - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
                }
            }

            return list;
        }

        public async Task IntegraRegistrosAsync(string tableName, string procName, string database)
        {
            try
            {
                var cnpjs = await _linxMovimentoRepository.GetCompanysAsync(tableName, database);

                foreach (var cnpj in cnpjs)
                {
                    PARAMETERS = await _linxMovimentoRepository.GetParametersAsync(tableName, database, "parameters_lastday");

                    var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_company);
                    var response = await _apiCall.CallAPIAsync(tableName, body);
                    var registros = _apiCall.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<TEntity, LinxMovimento>(TEntityToObject));
                            _linxMovimentoRepository.BulkInsertIntoTableRaw(list, tableName, database);
                        }
                    }
                };
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
                var cnpjs = _linxMovimentoRepository.GetCompanysNotAsync(tableName, database);

                foreach (var cnpj in cnpjs)
                {
                    PARAMETERS = _linxMovimentoRepository.GetParametersNotAsync(tableName, database, "parameters_lastday");

                    var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_company);
                    var response = _apiCall.CallAPINotAsync(tableName, body);
                    var registros = _apiCall.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<TEntity, LinxMovimento>(TEntityToObject));
                            _linxMovimentoRepository.BulkInsertIntoTableRaw(list, tableName, database);
                        }
                    }
                };
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
                PARAMETERS = await _linxMovimentoRepository.GetParametersAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[documento]", $"{identificador}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                string response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    await _linxMovimentoRepository.InsereRegistroIndividualAsync(registro[0], tableName, database);
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
                PARAMETERS = _linxMovimentoRepository.GetParametersNotAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[documento]", $"{identificador}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                string response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    _linxMovimentoRepository.InsereRegistroIndividualNotAsync(registro[0], tableName, database);
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
                    cnpj_emp = t1.cnpj_emp,
                    transacao = t1.transacao,
                    usuario = t1.usuario,
                    documento = t1.documento,
                    chave_nf = t1.chave_nf,
                    ecf = t1.ecf,
                    numero_serie_ecf = t1.numero_serie_ecf,
                    modelo_nf = t1.modelo_nf,
                    data_documento = t1.data_documento,
                    data_lancamento = t1.data_lancamento,
                    codigo_cliente = t1.codigo_cliente,
                    serie = t1.serie,
                    desc_cfop = t1.desc_cfop,
                    id_cfop = t1.id_cfop,
                    cod_vendedor = t1.cod_vendedor,
                    quantidade = t1.quantidade,
                    preco_custo = t1.preco_custo,
                    valor_liquido = t1.valor_liquido,
                    desconto = t1.desconto,
                    cst_icms = t1.cst_icms,
                    cst_pis = t1.cst_pis,
                    cst_cofins = t1.cst_cofins,
                    cst_ipi = t1.cst_ipi,
                    valor_icms = t1.valor_icms,
                    aliquota_icms = t1.aliquota_icms,
                    base_icms = t1.base_icms,
                    valor_pis = t1.valor_pis,
                    aliquota_pis = t1.aliquota_pis,
                    base_pis = t1.base_pis,
                    valor_cofins = t1.valor_cofins,
                    aliquota_cofins = t1.aliquota_cofins,
                    base_cofins = t1.base_cofins,
                    valor_icms_st = t1.valor_icms_st,
                    aliquota_icms_st = t1.aliquota_icms_st,
                    base_icms_st = t1.base_icms_st,
                    valor_ipi = t1.valor_ipi,
                    aliquota_ipi = t1.aliquota_ipi,
                    base_ipi = t1.base_ipi,
                    valor_total = t1.valor_total,
                    forma_dinheiro = t1.forma_dinheiro,
                    total_dinheiro = t1.total_dinheiro,
                    forma_cheque = t1.forma_cheque,
                    total_cheque = t1.total_cheque,
                    forma_cartao = t1.forma_cartao,
                    total_cartao = t1.total_cartao,
                    forma_crediario = t1.forma_crediario,
                    total_crediario = t1.total_crediario,
                    forma_convenio = t1.forma_convenio,
                    total_convenio = t1.total_convenio,
                    frete = t1.frete,
                    operacao = t1.operacao,
                    tipo_transacao = t1.tipo_transacao,
                    cod_produto = t1.cod_produto,
                    cod_barra = t1.cod_barra,
                    cancelado = t1.cancelado,
                    excluido = t1.excluido,
                    soma_relatorio = t1.soma_relatorio,
                    identificador = t1.identificador,
                    deposito = t1.deposito,
                    obs = t1.obs,
                    preco_unitario = t1.preco_unitario,
                    hora_lancamento = t1.hora_lancamento,
                    natureza_operacao = t1.natureza_operacao,
                    tabela_preco = t1.tabela_preco,
                    nome_tabela_preco = t1.nome_tabela_preco,
                    cod_sefaz_situacao = t1.cod_sefaz_situacao,
                    desc_sefaz_situacao = t1.desc_sefaz_situacao,
                    protocolo_aut_nfe = t1.protocolo_aut_nfe,
                    dt_update = t1.dt_update,
                    forma_cheque_prazo = t1.forma_cheque_prazo,
                    total_cheque_prazo = t1.total_cheque_prazo,
                    cod_natureza_operacao = t1.cod_natureza_operacao,
                    preco_tabela_epoca = t1.preco_tabela_epoca,
                    desconto_total_item = t1.desconto_total_item,
                    conferido = t1.conferido,
                    transacao_pedido_venda = t1.transacao_pedido_venda,
                    codigo_modelo_nf = t1.codigo_modelo_nf,
                    acrescimo = t1.acrescimo,
                    mob_checkout = t1.mob_checkout,
                    aliquota_iss = t1.aliquota_iss,
                    base_iss = t1.base_iss,
                    ordem = t1.ordem,
                    codigo_rotina_origem = t1.codigo_rotina_origem,
                    timestamp = t1.timestamp,
                    troco = t1.troco,
                    transportador = t1.transportador,
                    icms_aliquota_desonerado = t1.icms_aliquota_desonerado,
                    icms_valor_desonerado_item = t1.icms_valor_desonerado_item,
                    empresa = t1.empresa,
                    desconto_item = t1.desconto_item,
                    aliq_iss = t1.aliq_iss,
                    iss_base_item = t1.iss_base_item,
                    despesas = t1.despesas,
                    seguro_total_item = t1.seguro_total_item,
                    acrescimo_total_item = t1.acrescimo_total_item,
                    despesas_total_item = t1.despesas_total_item,
                    forma_pix = t1.forma_pix,
                    total_pix = t1.total_pix,
                    forma_deposito_bancario = t1.forma_deposito_bancario,
                    total_deposito_bancario = t1.total_deposito_bancario
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxMovimento - TEntityToObject - Erro ao converter registro: {t1.documento} para objeto - {ex.Message}");
            }
        }
    }
}
