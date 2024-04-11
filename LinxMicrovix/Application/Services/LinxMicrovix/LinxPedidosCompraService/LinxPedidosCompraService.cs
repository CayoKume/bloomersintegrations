using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;
using BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovix.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovix.Domain.Extensions;
using BloomersMicrovixIntegrations.LinxMicrovix.Infrastructure.Apis;
using System.Globalization;

namespace BloomersMicrovixIntegrations.Application.Services.LinxMicrovix
{
    public class LinxPedidosCompraService<TEntity> : ILinxPedidosCompraService<TEntity> where TEntity : LinxPedidosCompra, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveExport.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationExport.ToName();
        private readonly IAPICall _apiCall;
        private readonly ILinxPedidosCompraRepository _linxPedidosCompraRepository;

        public LinxPedidosCompraService(ILinxPedidosCompraRepository linxPedidosCompraRepository, IAPICall apiCall)
            => (_linxPedidosCompraRepository, _apiCall) = (linxPedidosCompraRepository, apiCall);

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
                        portal = registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First(),
                        cnpj_emp = registros[i].Where(pair => pair.Key == "cnpj_emp").Select(pair => pair.Value).First(),
                        cod_pedido = registros[i].Where(pair => pair.Key == "cod_pedido").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_pedido").Select(pair => pair.Value).First().Replace("-", ""),
                        data_pedido = registros[i].Where(pair => pair.Key == "data_pedido").Select(pair => pair.Value).First() == String.Empty ? new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar).ToString("yyyy-MM-dd") : registros[i].Where(pair => pair.Key == "data_pedido").Select(pair => pair.Value).First(),
                        transacao = registros[i].Where(pair => pair.Key == "transacao").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "transacao").Select(pair => pair.Value).First(),
                        usuario = registros[i].Where(pair => pair.Key == "usuario").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "usuario").Select(pair => pair.Value).First(),
                        codigo_fornecedor = registros[i].Where(pair => pair.Key == "codigo_fornecedor").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "codigo_fornecedor").Select(pair => pair.Value).First(),
                        cod_produto = registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_produto").Select(pair => pair.Value).First(),
                        quantidade = registros[i].Where(pair => pair.Key == "quantidade").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "quantidade").Select(pair => pair.Value).First(),
                        valor_unitario = registros[i].Where(pair => pair.Key == "valor_unitario").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "valor_unitario").Select(pair => pair.Value).First(),
                        cod_comprador = registros[i].Where(pair => pair.Key == "cod_comprador").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_comprador").Select(pair => pair.Value).First(),
                        valor_frete = registros[i].Where(pair => pair.Key == "valor_frete").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "valor_frete").Select(pair => pair.Value).First(),
                        valor_total = registros[i].Where(pair => pair.Key == "valor_total").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "valor_total").Select(pair => pair.Value).First(),
                        cod_plano_pagamento = registros[i].Where(pair => pair.Key == "cod_plano_pagamento").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_plano_pagamento").Select(pair => pair.Value).First(),
                        plano_pagamento = registros[i].Where(pair => pair.Key == "plano_pagamento").Select(pair => pair.Value).First(),
                        obs = registros[i].Where(pair => pair.Key == "obs").Select(pair => pair.Value).First(),
                        aprovado = registros[i].Where(pair => pair.Key == "aprovado").Select(pair => pair.Value).First(),
                        cancelado = registros[i].Where(pair => pair.Key == "cancelado").Select(pair => pair.Value).First(),
                        encerrado = registros[i].Where(pair => pair.Key == "encerrado").Select(pair => pair.Value).First(),
                        data_aprovacao = registros[i].Where(pair => pair.Key == "data_aprovacao").Select(pair => pair.Value).First() == String.Empty ? new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar).ToString("yyyy-MM-dd HH:mm:ss") : registros[i].Where(pair => pair.Key == "data_aprovacao").Select(pair => pair.Value).First(),
                        numero_ped_fornec = registros[i].Where(pair => pair.Key == "numero_ped_fornec").Select(pair => pair.Value).First(),
                        tipo_frete = registros[i].Where(pair => pair.Key == "tipo_frete").Select(pair => pair.Value).First(),
                        natureza_operacao = registros[i].Where(pair => pair.Key == "natureza_operacao").Select(pair => pair.Value).First(),
                        previsao_entrega = registros[i].Where(pair => pair.Key == "previsao_entrega").Select(pair => pair.Value).First() == String.Empty ? new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar).ToString("yyyy-MM-dd") : registros[i].Where(pair => pair.Key == "previsao_entrega").Select(pair => pair.Value).First(),
                        numero_projeto_officina = registros[i].Where(pair => pair.Key == "numero_projeto_officina").Select(pair => pair.Value).First(),
                        status_pedido = registros[i].Where(pair => pair.Key == "status_pedido").Select(pair => pair.Value).First(),
                        qtde_entregue = registros[i].Where(pair => pair.Key == "qtde_entregue").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "qtde_entregue").Select(pair => pair.Value).First(),
                        descricao_frete = registros[i].Where(pair => pair.Key == "descricao_frete").Select(pair => pair.Value).First(),
                        integrado_linx = registros[i].Where(pair => pair.Key == "integrado_linx").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "integrado_linx").Select(pair => pair.Value).First(),
                        nf_gerada = registros[i].Where(pair => pair.Key == "nf_gerada").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "nf_gerada").Select(pair => pair.Value).First(),
                        timestamp = registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First(),
                        empresa = registros[i].Where(pair => pair.Key == "empresa").Select(pair => pair.Value).First()
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = registros[i].Where(pair => pair.Key == "cod_pedido").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "cod_pedido").Select(pair => pair.Value).First();
                    throw new Exception($"LinxPedidosCompra - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
                }
            }
            return list;
        }

        public async Task IntegraRegistrosAsync(string tableName, string procName, string database)
        {
            try
            {
                var cnpjs = await _linxPedidosCompraRepository.GetCompanysAsync(tableName, database);

                foreach (var cnpj in cnpjs)
                {
                    PARAMETERS = await _linxPedidosCompraRepository.GetParametersAsync(tableName, database, "parameters_lastday");

                    var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_company);
                    var response = await _apiCall.CallAPIAsync(tableName, body);
                    var registros = _apiCall.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<TEntity, LinxPedidosCompra>(TEntityToObject));
                            _linxPedidosCompraRepository.BulkInsertIntoTableRaw(list, tableName, database);
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
                var cnpjs = _linxPedidosCompraRepository.GetCompanysNotAsync(tableName, database);

                foreach (var cnpj in cnpjs)
                {
                    PARAMETERS = _linxPedidosCompraRepository.GetParametersNotAsync(tableName, database, "parameters_lastday");

                    var body = _apiCall.BuildBodyRequest(PARAMETERS.Replace("[0]", "0").Replace("[data_inicio]", $"{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}").Replace("[data_fim]", $"{DateTime.Today.ToString("yyyy-MM-dd")}"), tableName, AUTENTIFICACAO, CHAVE, cnpj.doc_company);
                    var response = _apiCall.CallAPINotAsync(tableName, body);
                    var registros = _apiCall.DeserializeXML(response);

                    if (registros.Count() > 0)
                    {
                        var listResults = DeserializeResponse(registros);
                        if (listResults.Count() > 0)
                        {
                            var list = listResults.ConvertAll(new Converter<TEntity, LinxPedidosCompra>(TEntityToObject));
                            _linxPedidosCompraRepository.BulkInsertIntoTableRaw(list, tableName, database);
                        }
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
                    portal = t1.portal,
                    cnpj_emp = t1.cnpj_emp,
                    cod_pedido = t1.cod_pedido,
                    data_pedido = t1.data_pedido,
                    transacao = t1.transacao,
                    usuario = t1.usuario,
                    codigo_fornecedor = t1.codigo_fornecedor,
                    cod_produto = t1.cod_produto,
                    quantidade = t1.quantidade,
                    valor_unitario = t1.valor_unitario,
                    cod_comprador = t1.cod_comprador,
                    valor_frete = t1.valor_frete,
                    valor_total = t1.valor_total,
                    cod_plano_pagamento = t1.cod_plano_pagamento,
                    plano_pagamento = t1.plano_pagamento,
                    obs = t1.obs,
                    aprovado = t1.aprovado,
                    cancelado = t1.cancelado,
                    encerrado = t1.encerrado,
                    data_aprovacao = t1.data_aprovacao,
                    numero_ped_fornec = t1.numero_ped_fornec,
                    tipo_frete = t1.tipo_frete,
                    natureza_operacao = t1.natureza_operacao,
                    previsao_entrega = t1.previsao_entrega,
                    numero_projeto_officina = t1.numero_projeto_officina,
                    status_pedido = t1.status_pedido,
                    qtde_entregue = t1.qtde_entregue,
                    descricao_frete = t1.descricao_frete,
                    integrado_linx = t1.integrado_linx,
                    nf_gerada = t1.nf_gerada,
                    timestamp = t1.timestamp,
                    empresa = t1.empresa
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"LinxPedidosCompra - TEntityToObject - Erro ao converter registro: {t1.cod_pedido} para objeto - {ex.Message}");
            }
        }
    }
}
