using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;
using BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovix.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovix.Domain.Extensions;
using BloomersMicrovixIntegrations.LinxMicrovix.Infrastructure.Apis;

namespace BloomersMicrovixIntegrations.Application.Services.LinxMicrovix
{
    public class LinxPedidosVendaService<TEntity> : ILinxPedidosVendaService<TEntity> where TEntity : LinxPedidosVenda, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly IAPICall _apiCall;
        private readonly ILinxPedidosVendaRepository _linxPedidosVendaRepository;

        public LinxPedidosVendaService(ILinxPedidosVendaRepository linxPedidosVendaRepository, IAPICall apiCall)
            => (_linxPedidosVendaRepository, _apiCall) = (linxPedidosVendaRepository, apiCall);

        public List<TEntity?> DeserializeResponse(List<Dictionary<string, string>> registros)
        {
            var list = new List<TEntity?>();

            for (int i = 0; i < registros.Count; i++)
            {
                try
                {
                    list.Add(new TEntity
                    {
                        lastupdateon = DateTime.Now,
                        portal = registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First(),
                        cnpj_emp = registros[i].Where(pair => pair.Key == "cnpj_emp").Select(pair => pair.Value).First(),
                        cod_pedido = registros[i].Where(pair => pair.Key == "cod_pedido").Select(pair => pair.Value).First(),
                        data_lancamento = registros[i].Where(pair => pair.Key == "data_lancamento").Select(pair => pair.Value).First(),
                        hora_lancamento = registros[i].Where(pair => pair.Key == "hora_lancamento").Select(pair => pair.Value).First(),
                        transacao = registros[i].Where(pair => pair.Key == "transacao").Select(pair => pair.Value).First(),
                        usuario = registros[i].Where(pair => pair.Key == "usuario").Select(pair => pair.Value).First(),
                        codigo_cliente = registros[i].Where(pair => pair.Key == "codigo_cliente").Select(pair => pair.Value).First(),
                        cod_produto = registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First(),
                        quantidade = registros[i].Where(pair => pair.Key == "quantidade").Select(pair => pair.Value).First(),
                        valor_unitario = registros[i].Where(pair => pair.Key == "valor_unitario").Select(pair => pair.Value).First(),
                        cod_vendedor = registros[i].Where(pair => pair.Key == "cod_vendedor").Select(pair => pair.Value).First(),
                        valor_frete = registros[i].Where(pair => pair.Key == "valor_frete").Select(pair => pair.Value).First(),
                        valor_total = registros[i].Where(pair => pair.Key == "valor_total").Select(pair => pair.Value).First(),
                        desconto_item = registros[i].Where(pair => pair.Key == "desconto_item").Select(pair => pair.Value).First(),
                        cod_plano_pagamento = registros[i].Where(pair => pair.Key == "cod_plano_pagamento").Select(pair => pair.Value).First(),
                        plano_pagamento = registros[i].Where(pair => pair.Key == "plano_pagamento").Select(pair => pair.Value).First(),
                        obs = registros[i].Where(pair => pair.Key == "obs").Select(pair => pair.Value).First(),
                        aprovado = registros[i].Where(pair => pair.Key == "aprovado").Select(pair => pair.Value).First(),
                        cancelado = registros[i].Where(pair => pair.Key == "cancelado").Select(pair => pair.Value).First(),
                        data_aprovacao = registros[i].Where(pair => pair.Key == "data_aprovacao").Select(pair => pair.Value).First(),
                        data_alteracao = registros[i].Where(pair => pair.Key == "data_alteracao").Select(pair => pair.Value).First(),
                        tipo_frete = registros[i].Where(pair => pair.Key == "tipo_frete").Select(pair => pair.Value).First(),
                        natureza_operacao = registros[i].Where(pair => pair.Key == "natureza_operacao").Select(pair => pair.Value).First(),
                        tabela_preco = registros[i].Where(pair => pair.Key == "tabela_preco").Select(pair => pair.Value).First(),
                        nome_tabela_preco = registros[i].Where(pair => pair.Key == "nome_tabela_preco").Select(pair => pair.Value).First(),
                        previsao_entrega = registros[i].Where(pair => pair.Key == "previsao_entrega").Select(pair => pair.Value).First(),
                        realizado_por = registros[i].Where(pair => pair.Key == "realizado_por").Select(pair => pair.Value).First(),
                        pontuacao_ser = registros[i].Where(pair => pair.Key == "pontuacao_ser").Select(pair => pair.Value).First(),
                        venda_externa = registros[i].Where(pair => pair.Key == "venda_externa").Select(pair => pair.Value).First(),
                        nf_gerada = registros[i].Where(pair => pair.Key == "nf_gerada").Select(pair => pair.Value).First(),
                        status = registros[i].Where(pair => pair.Key == "status").Select(pair => pair.Value).First(),
                        numero_projeto_officina = registros[i].Where(pair => pair.Key == "numero_projeto_officina").Select(pair => pair.Value).First(),
                        cod_natureza_operacao = registros[i].Where(pair => pair.Key == "cod_natureza_operacao").Select(pair => pair.Value).First(),
                        margem_contribuicao = registros[i].Where(pair => pair.Key == "margem_contribuicao").Select(pair => pair.Value).First(),
                        doc_origem = registros[i].Where(pair => pair.Key == "doc_origem").Select(pair => pair.Value).First(),
                        posicao_item = registros[i].Where(pair => pair.Key == "posicao_item").Select(pair => pair.Value).First(),
                        orcamento_origem = registros[i].Where(pair => pair.Key == "orcamento_origem").Select(pair => pair.Value).First(),
                        transacao_origem = registros[i].Where(pair => pair.Key == "transacao_origem").Select(pair => pair.Value).First(),
                        timestamp = registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First(),
                        desconto = registros[i].Where(pair => pair.Key == "desconto").Select(pair => pair.Value).First(),
                        transacao_ws = registros[i].Where(pair => pair.Key == "transacao_ws").Select(pair => pair.Value).First(),
                        empresa = registros[i].Where(pair => pair.Key == "empresa").Select(pair => pair.Value).First(),
                        transportador = registros[i].Where(pair => pair.Key == "transportador").Select(pair => pair.Value).First(),
                        deposito = registros[i].Where(pair => pair.Key == "deposito").Select(pair => pair.Value).First()
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = registros[i].Where(pair => pair.Key == "cod_pedido").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_pedido").Select(pair => pair.Value).First();
                    throw new Exception($"LinxPedidosVenda - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
                }
            }

            return list;
        }

        public async Task IntegraRegistrosAsync(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _linxPedidosVendaRepository.GetParametersAsync(tableName, database, "parameters_lastday");

                var cnpjs = await _linxPedidosVendaRepository.GetCompanysAsync(tableName, database);

                foreach (var cnpj in cnpjs)
                {
                    var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-14).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_company);
                    var response = await _apiCall.CallAPIAsync(tableName, body);
                    var registros = _apiCall.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<TEntity, LinxPedidosVenda>(TEntityToObject));
                            _linxPedidosVendaRepository.BulkInsertIntoTableRaw(list, tableName, database);
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
                PARAMETERS = _linxPedidosVendaRepository.GetParametersNotAsync(tableName, database, "parameters_lastday");

                var cnpjs = _linxPedidosVendaRepository.GetCompanysNotAsync(tableName, database);

                foreach (var cnpj in cnpjs)
                {
                    var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-14).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_company);
                    var response = _apiCall.CallAPINotAsync(tableName, body);
                    var registros = _apiCall.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<TEntity, LinxPedidosVenda>(TEntityToObject));
                            _linxPedidosVendaRepository.BulkInsertIntoTableRaw(list, tableName, database);
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
                PARAMETERS = await _linxPedidosVendaRepository.GetParametersAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[doc_origem]", $"{identificador}").Replace("[0]", "0").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                string response = await _apiCall.CallAPIAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    for (int i = 0; i < registro.Count(); i++)
                    {
                        await _linxPedidosVendaRepository.InsereRegistroIndividualAsync(registro[i], tableName, database);
                    }
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
                PARAMETERS = _linxPedidosVendaRepository.GetParametersNotAsync(tableName, database, "parameters_manual");

                var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[doc_origem]", $"{identificador}").Replace("[0]", "0").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj_emp);
                string response = _apiCall.CallAPINotAsync(tableName, body);
                var registros = _apiCall.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro.Count() > 0)
                {
                    for (int i = 0; i < registro.Count(); i++)
                    {
                        _linxPedidosVendaRepository.InsereRegistroIndividualNotAsync(registro[i], tableName, database);
                    }
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
                    cod_pedido = t1.cod_pedido,
                    data_lancamento = t1.data_lancamento,
                    hora_lancamento = t1.hora_lancamento,
                    transacao = t1.transacao,
                    usuario = t1.usuario,
                    codigo_cliente = t1.codigo_cliente,
                    cod_produto = t1.cod_produto,
                    quantidade = t1.quantidade,
                    valor_unitario = t1.valor_unitario,
                    cod_vendedor = t1.cod_vendedor,
                    valor_frete = t1.valor_frete,
                    valor_total = t1.valor_total,
                    desconto_item = t1.desconto_item,
                    cod_plano_pagamento = t1.cod_plano_pagamento,
                    plano_pagamento = t1.plano_pagamento,
                    obs = t1.obs,
                    aprovado = t1.aprovado,
                    cancelado = t1.cancelado,
                    data_aprovacao = t1.data_aprovacao,
                    data_alteracao = t1.data_alteracao,
                    tipo_frete = t1.tipo_frete,
                    natureza_operacao = t1.natureza_operacao,
                    tabela_preco = t1.tabela_preco,
                    nome_tabela_preco = t1.nome_tabela_preco,
                    previsao_entrega = t1.previsao_entrega,
                    realizado_por = t1.realizado_por,
                    pontuacao_ser = t1.pontuacao_ser,
                    venda_externa = t1.venda_externa,
                    nf_gerada = t1.nf_gerada,
                    status = t1.status,
                    numero_projeto_officina = t1.numero_projeto_officina,
                    cod_natureza_operacao = t1.cod_natureza_operacao,
                    margem_contribuicao = t1.margem_contribuicao,
                    doc_origem = t1.doc_origem,
                    posicao_item = t1.posicao_item,
                    orcamento_origem = t1.orcamento_origem,
                    transacao_origem = t1.transacao_origem,
                    timestamp = t1.timestamp,
                    desconto = t1.desconto,
                    transacao_ws = t1.transacao_ws,
                    empresa = t1.empresa,
                    transportador = t1.transportador,
                    deposito = t1.deposito
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxPedidosVenda - TEntityToObject - Erro ao converter registro: {t1.cod_pedido} para objeto - {ex.Message}");
            }
        }
    }
}
