using BloomersCarriersIntegrations.FlashCourier.Application.Services;
using BloomersCarriersIntegrations.TotalExpress.Application.Services;
using BloomersCommerceIntegrations.LinxCommerce.Application.Services;
using BloomersCommerceIntegrations.LinxCommerce.Domain.Entities;
using BloomersCommerceIntegrations.LinxCommerce.Domain.Enums;
using BloomersCommerceIntegrations.LinxCommerce.Domain.Extensions;
using BloomersGeneralIntegrations.AfterSale.Application.Services;
using BloomersGeneralIntegrations.Dootax.Application.Services;
using BloomersGeneralIntegrations.Mobsim.Application.Services;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxCommerce;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxEcommerce;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;
using Hangfire;

namespace BloomersIntegrationsManager.Domain.Extensions
{
    public static class RecurringJobsExtensions
    {
        public static void AddRecurringJobs()
        {
            FlashCourierRecurringJobs();
            TotalExpressRecurringJobs();
            AfterSaleRecurringJobs();
            DootaxRecurringJobs();
            MobsimRecurringJobs();
            LinxCommerceRecurringJobs();
            LinxMicrovixB2CRecurringJobs();
            LinxMicrovixERPRecurringJobs();
        }

        private static void FlashCourierRecurringJobs()
        {
            RecurringJob.AddOrUpdate<IFlashCourierService>("FlashCourierEnviaPedidos", service => service.EnviaPedidosFlash(),
                Cron.MinuteInterval(3)
            );

            RecurringJob.AddOrUpdate<IFlashCourierService>("FlashCourierAtualizaLogPedido", service => service.AtualizaLogPedidoEnviado(),
                Cron.MinuteInterval(5)
            );
        }

        private static void TotalExpressRecurringJobs()
        {
            RecurringJob.AddOrUpdate<ITotalExpressService>("TotalExpressEnviaPedidos", service => service.EnviaPedidosTotal(),
                Cron.MinuteInterval(3)
            );

            RecurringJob.AddOrUpdate<ITotalExpressService>("TotalExpressAtualizaLogPedido", service => service.AtualizaLogPedidoEnviado(),
                Cron.MinuteInterval(5)
            );
        }

        //DESCONTINUADO MANDAE
        //private static void MandaeRecurringJobs()
        //{
        //    RecurringJob.AddOrUpdate<IMandaeService>("MandaeEnviaPedidos", service => service.EnviaPedidosMandae(),
        //        Cron.MinuteInterval(3)
        //    );
        //} 

        private static void AfterSaleRecurringJobs()
        {
            RecurringJob.AddOrUpdate<IAfterSaleService>("GetAfterSaleReversas", service => service.GetReverses(),
                Cron.MinuteInterval(5)
            );
        }
    
        private static void DootaxRecurringJobs()
        {
            RecurringJob.AddOrUpdate<IDootaxService>("DootaxEnviaXMLs", service => service.EnviaXML(),
                Cron.MinuteInterval(3)
            );
        }
    
        private static void MobsimRecurringJobs()
        {
            RecurringJob.AddOrUpdate<IMobsimService>("EnviaMensagensPedidosFaturados", service => service.EnviaMensagemPedidoFaturado(),
                Cron.MinuteInterval(3)
            );

            RecurringJob.AddOrUpdate<IMobsimService>("EnviaMensagensPedidosExpedidos", service => service.EnviaMensagemPedidoExpedido(),
                Cron.MinuteInterval(3)
            );

            RecurringJob.AddOrUpdate<IMobsimService>("EnviaMensagensPedidosEntregues", service => service.EnviaMensagemPedidoEntregue(),
                Cron.MinuteInterval(3)
            );
        }
    
        //NOT IMPLEMENTED
        //private static void PagarmeRecurringJobs()
        //{

        //}

        private static void LinxCommerceRecurringJobs()
        {
            RecurringJob.AddOrUpdate<IOrderService<SearchOrderResponse.Root>>("LinxCommerceOrders", service => service.IntegraRegistros(
                "Order",
                "p_Order_trusted",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(5)
            );

            RecurringJob.AddOrUpdate<ISKUService<SKUs>>("LinxCommerceSKUs", service => service.IntegraRegistros(
                "SkuBase",
                "p_SkuBase_trusted",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(5)
            );

            RecurringJob.AddOrUpdate<IProductService<SearchProductResponse.Root>>("LinxCommerceProdutos", service => service.IntegraRegistros(
                "Product",
                "p_Product_trusted",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(5)
            );
        }
    
        private static void LinxMicrovixB2CRecurringJobs()
        {
            RecurringJob.AddOrUpdate<IB2CConsultaClientesServices<B2CConsultaClientes>>("B2CConsultaClientes", service => service.IntegraRegistros(
            "B2CConsultaClientes",
                "p_B2CConsultaClientes_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(3)
            );

            RecurringJob.AddOrUpdate<IB2CConsultaNFeService<B2CConsultaNFe>>("B2CConsultaNFe", service => service.IntegraRegistros(
                "B2CConsultaNFe",
                "p_B2CConsultaNFe_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(3)
            );

            RecurringJob.AddOrUpdate<IB2CConsultaNFeSituacaoService<B2CConsultaNFeSituacao>>("B2CConsultaNFeSituacao", service => service.IntegraRegistros(
                "B2CConsultaNFeSituacao",
                "p_B2CConsultaNFeSituacao_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.Daily
            );

            RecurringJob.AddOrUpdate<IB2CConsultaPedidosItensService<B2CConsultaPedidosItens>>("B2CConsultaPedidosItens", service => service.IntegraRegistros(
                "B2CConsultaPedidosItens",
                "p_B2CConsultaPedidosItens_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(3)
            );

            RecurringJob.AddOrUpdate<IB2CConsultaPedidosService<B2CConsultaPedidos>>("B2CConsultaPedidos", service => service.IntegraRegistros(
                "B2CConsultaPedidos",
                "p_B2CConsultaPedidos_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(3)
            );

            RecurringJob.AddOrUpdate<IB2CConsultaPedidosStatusService<B2CConsultaPedidosStatus>>("B2CConsultaPedidosStatus", service => service.IntegraRegistros(
                "B2CConsultaPedidosStatus",
                "p_B2CConsultaPedidosStatus_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(10)
            );

            RecurringJob.AddOrUpdate<IB2CConsultaStatusService<B2CConsultaStatus>>("B2CConsultaStatus", service => service.IntegraRegistros(
                "B2CConsultaStatus",
                "p_B2CConsultaStatus_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.Daily
            );
        }

        private static void LinxMicrovixERPRecurringJobs()
        {
            RecurringJob.AddOrUpdate<ILinxClientesFornecService<LinxClientesFornec>>("LinxClientesFornec", service => service.IntegraRegistros(
                "LinxClientesFornec",
                "p_LinxClientesFornec_trusted_unificado",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(3)
            );

            RecurringJob.AddOrUpdate<ILinxMovimentoCartoesService<LinxMovimentoCartoes>>("LinxMovimentoCartoes", service => service.IntegraRegistros(
                "LinxMovimentoCartoes",
                "p_LinxMovimentoCartoes_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(10)
            );

            RecurringJob.AddOrUpdate<ILinxMovimentoService<LinxMovimento>>("LinxMovimento", service => service.IntegraRegistrosSync(
                "LinxMovimento",
                "p_LinxMovimento_trusted_unificado",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(5)
            );

            RecurringJob.AddOrUpdate<ILinxPedidosCompraService<LinxPedidosCompra>>("LinxPedidosCompra", service => service.IntegraRegistros(
                "LinxPedidosCompra",
                "p_LinxPedidosCompra_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(10)
            );

            RecurringJob.AddOrUpdate<ILinxPedidosVendaService<LinxPedidosVenda>>("LinxPedidosVenda", service => service.IntegraRegistrosSync(
                "LinxPedidosVenda",
                "p_LinxPedidosVenda_trusted_unificado",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(5)
            );

            RecurringJob.AddOrUpdate<ILinxProdutosService<LinxProdutos>>("LinxProdutos", service => service.IntegraRegistros(
                "LinxProdutos",
                "p_LinxProdutos_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(3)
            );

            RecurringJob.AddOrUpdate<ILinxProdutosPromocoesService<LinxProdutosPromocoes>>("LinxProdutosPromocoes", service => service.IntegraRegistros(
                "LinxProdutosPromocoes",
                "p_LinxProdutosPromocoes_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.Hourly
            );

            RecurringJob.AddOrUpdate<ILinxProdutosDepositosService<LinxProdutosDepositos>>("LinxProdutosDepositos", service => service.IntegraRegistros(
                "LinxProdutosDepositos",
                "p_LinxProdutosDepositos_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.Daily
            );

            RecurringJob.AddOrUpdate<ILinxProdutosDetalhesService<LinxProdutosDetalhes>>("LinxProdutosDetalhes", service => service.IntegraRegistros(
                "LinxProdutosDetalhes",
                "p_LinxProdutosDetalhes_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.Hourly
            );

            RecurringJob.AddOrUpdate<ILinxProdutosInventarioService<LinxProdutosInventario>>("LinxProdutosInventario", service => service.IntegraRegistros(
                "LinxProdutosInventario",
                "p_LinxProdutosInventario_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.Hourly
            );

            RecurringJob.AddOrUpdate<ILinxProdutosTabelasService<LinxProdutosTabelas>>("LinxProdutosTabelas", service => service.IntegraRegistros(
                "LinxProdutosTabelas",
                "p_LinxProdutosTabelas_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.Daily
            );

            RecurringJob.AddOrUpdate<ILinxProdutosTabelasPrecosService<LinxProdutosTabelasPrecos>>("LinxProdutosTabelasPrecos", service => service.IntegraRegistrosSync(
                "LinxProdutosTabelasPrecos",
                "p_LinxProdutosTabelasPrecos_trusted",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.Hourly
            );

            RecurringJob.AddOrUpdate<ILinxProdutosCamposAdicionaisService<LinxProdutosCamposAdicionais>>("LinxProdutosCamposAdicionais", service => service.IntegraRegistros(
                "LinxProdutosCamposAdicionais",
                "p_LinxProdutosCamposAdicionais_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.Daily
            );

            RecurringJob.AddOrUpdate<ILinxXMLDocumentosService<LinxXMLDocumentos>>("LinxXMLDocumentos", service => service.IntegraRegistrosSync(
                "LinxXMLDocumentos",
                "p_LinxXMLDocumentos_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(5)
            );
        }
    }
}
