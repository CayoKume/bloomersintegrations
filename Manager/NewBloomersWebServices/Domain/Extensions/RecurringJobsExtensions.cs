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
using BloomersWorkers.AuthorizeNFe.Application.Services;
using BloomersWorkers.ChangingOrder.Application.Services;
using BloomersWorkers.ChangingPassword.Application.Services;
using BloomersWorkers.InsertReverse.Application.Services;
using BloomersWorkers.InvoiceOrder.Application.Services;
using Hangfire;

namespace BloomersIntegrationsManager.Domain.Extensions
{
    public static class RecurringJobsExtensions
    {
        public static void AddRecurringJobs(string? serverName)
        {
            if (serverName == "CarriersServer")
            {
                FlashCourierRecurringJobs();
                TotalExpressRecurringJobs();
            }

            else if (serverName == "GeneralServer")
            {
                AfterSaleRecurringJobs();
                DootaxRecurringJobs();
                MobsimRecurringJobs();
            }
            
            else if (serverName == "WorkerServer")
            {
                //AuthorizeNFeRecurringJobs();
                //ChangingOrderRecurringJobs();
                //ChangingPasswordRecurringJobs();
                InvoicedBotsRecurringJobs();
            }

            else if (serverName == "LinxMicrovixServer")
            {
                LinxMicrovixB2CRecurringJobs();
                LinxMicrovixERPRecurringJobs();
            }

            else if (serverName == "LinxCommerceServer")
                LinxCommerceRecurringJobs();
        }

        private static void FlashCourierRecurringJobs()
        {
            RecurringJob.AddOrUpdate<IFlashCourierService>("FlashCourierEnviaPedidos", service => service.EnviaPedidosFlash(),
                Cron.MinuteInterval(3),
                queue: "carriersserver"
            );

            RecurringJob.AddOrUpdate<IFlashCourierService>("FlashCourierAtualizaLogPedido", service => service.AtualizaLogPedidoEnviado(),
                Cron.MinuteInterval(5),
                queue: "carriersserver"
            );
        }

        private static void TotalExpressRecurringJobs()
        {
            RecurringJob.AddOrUpdate<ITotalExpressService>("TotalExpressEnviaPedidos", service => service.SendOrders(),
                Cron.MinuteInterval(3),
                queue: "carriersserver"
            );

            RecurringJob.AddOrUpdate<ITotalExpressService>("TotalExpressAtualizaLogPedido", service => service.UpdateOrderSendLog(),
                Cron.MinuteInterval(5),
                queue: "carriersserver"
            );
        }

        //DESCONTINUADO MANDAE
        //private static void MandaeRecurringJobs()
        //{
        //    RecurringJob.AddOrUpdate<IMandaeService>("MandaeEnviaPedidos", service => service.EnviaPedidosMandae(),
        //        Cron.MinuteInterval(3),
        //        queue: "CarriersServer"
        //    );
        //} 

        private static void AfterSaleRecurringJobs()
        {
            RecurringJob.AddOrUpdate<IAfterSaleService>("GetAfterSaleReversas", service => service.GetReverses(),
                Cron.MinuteInterval(5),
                queue: "generalserver"
            );
        }

        private static void DootaxRecurringJobs()
        {
            RecurringJob.AddOrUpdate<IDootaxService>("DootaxEnviaXMLs", service => service.EnviaXML(),
                Cron.MinuteInterval(3),
                queue: "generalserver"
            );
        }

        private static void MobsimRecurringJobs()
        {
            RecurringJob.AddOrUpdate<IMobsimService>("EnviaMensagensPedidosFaturados", service => service.SendMessageInvoicedOrder(),
                Cron.MinuteInterval(3),
                queue: "generalserver"
            );

            RecurringJob.AddOrUpdate<IMobsimService>("EnviaMensagensPedidosExpedidos", service => service.SendMessageShippdedOrder(),
                Cron.MinuteInterval(3),
                queue: "generalserver"
            );

            RecurringJob.AddOrUpdate<IMobsimService>("EnviaMensagensPedidosEntregues", service => service.SendMessageDeliveredOrder(),
                Cron.MinuteInterval(3),
                queue: "generalserver"
            );
        }

        //NOT IMPLEMENTED
        //private static void PagarmeRecurringJobs()
        //{

        //}

        private static void LinxCommerceRecurringJobs()
        {
            RecurringJob.AddOrUpdate<IOrderService<SearchOrderResponse.Root>>("LinxCommerceOrders", service => service.IntegraRegistros(
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(5), 
                queue: "linxcommerceserver"
            );

            RecurringJob.AddOrUpdate<ISKUService<SKUs>>("LinxCommerceSKUs", service => service.IntegraRegistros(
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(5),
                queue: "linxcommerceserver"
            );

            RecurringJob.AddOrUpdate<IProductService<SearchProductResponse.Root>>("LinxCommerceProdutos", service => service.IntegraRegistros(
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(5),
                queue: "linxcommerceserver"
            );
        }

        private static void LinxMicrovixB2CRecurringJobs()
        {
            RecurringJob.AddOrUpdate<IB2CConsultaClientesServices<B2CConsultaClientes>>("B2CConsultaClientes", service => service.IntegraRegistrosAsync(
            "B2CConsultaClientes",
                "p_B2CConsultaClientes_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(3),
                queue: "linxmicrovixserver"
            );

            RecurringJob.AddOrUpdate<IB2CConsultaNFeService<B2CConsultaNFe>>("B2CConsultaNFe", service => service.IntegraRegistrosAsync(
                "B2CConsultaNFe",
                "p_B2CConsultaNFe_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(3),
                queue: "linxmicrovixserver"
            );

            RecurringJob.AddOrUpdate<IB2CConsultaNFeSituacaoService<B2CConsultaNFeSituacao>>("B2CConsultaNFeSituacao", service => service.IntegraRegistrosAsync(
                "B2CConsultaNFeSituacao",
                "p_B2CConsultaNFeSituacao_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.Daily,
                queue: "linxmicrovixserver"
            );

            RecurringJob.AddOrUpdate<IB2CConsultaPedidosItensService<B2CConsultaPedidosItens>>("B2CConsultaPedidosItens", service => service.IntegraRegistrosAsync(
                "B2CConsultaPedidosItens",
                "p_B2CConsultaPedidosItens_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(3),
                queue: "linxmicrovixserver"
            );

            RecurringJob.AddOrUpdate<IB2CConsultaPedidosService<B2CConsultaPedidos>>("B2CConsultaPedidos", service => service.IntegraRegistrosAsync(
                "B2CConsultaPedidos",
                "p_B2CConsultaPedidos_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(3),
                queue: "linxmicrovixserver"
            );

            RecurringJob.AddOrUpdate<IB2CConsultaPedidosStatusService<B2CConsultaPedidosStatus>>("B2CConsultaPedidosStatus", service => service.IntegraRegistrosAsync(
                "B2CConsultaPedidosStatus",
                "p_B2CConsultaPedidosStatus_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(10),
                queue: "linxmicrovixserver"
            );

            RecurringJob.AddOrUpdate<IB2CConsultaStatusService<B2CConsultaStatus>>("B2CConsultaStatus", service => service.IntegraRegistrosAsync(
                "B2CConsultaStatus",
                "p_B2CConsultaStatus_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.Daily,
                queue: "linxmicrovixserver"
            );
        }

        private static void LinxMicrovixERPRecurringJobs()
        {
            RecurringJob.AddOrUpdate<ILinxClientesFornecService<LinxClientesFornec>>("LinxClientesFornec", service => service.IntegraRegistrosAsync(
                "LinxClientesFornec",
                "p_LinxClientesFornec_trusted_unificado",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(3),
                queue: "linxmicrovixserver"
            );

            RecurringJob.AddOrUpdate<ILinxMovimentoCartoesService<LinxMovimentoCartoes>>("LinxMovimentoCartoes", service => service.IntegraRegistrosAsync(
                "LinxMovimentoCartoes",
                "p_LinxMovimentoCartoes_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(10),
                queue: "linxmicrovixserver"
            );

            RecurringJob.AddOrUpdate<ILinxMovimentoService<LinxMovimento>>("LinxMovimento", service => service.IntegraRegistrosNotAsync(
                "LinxMovimento",
                "p_LinxMovimento_trusted_unificado",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(5),
                queue: "linxmicrovixserver"
            );

            RecurringJob.AddOrUpdate<ILinxPedidosCompraService<LinxPedidosCompra>>("LinxPedidosCompra", service => service.IntegraRegistrosAsync(
                "LinxPedidosCompra",
                "p_LinxPedidosCompra_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(10),
                queue: "linxmicrovixserver"
            );

            RecurringJob.AddOrUpdate<ILinxPedidosVendaService<LinxPedidosVenda>>("LinxPedidosVenda", service => service.IntegraRegistrosNotAsync(
                "LinxPedidosVenda",
                "p_LinxPedidosVenda_trusted_unificado",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(5),
                queue: "linxmicrovixserver"
            );

            RecurringJob.AddOrUpdate<ILinxProdutosService<LinxProdutos>>("LinxProdutos", service => service.IntegraRegistrosAsync(
                "LinxProdutos",
                "p_LinxProdutos_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(3),
                queue: "linxmicrovixserver"
            );

            RecurringJob.AddOrUpdate<ILinxProdutosPromocoesService<LinxProdutosPromocoes>>("LinxProdutosPromocoes", service => service.IntegraRegistrosAsync(
                "LinxProdutosPromocoes",
                "p_LinxProdutosPromocoes_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.Hourly,
                queue: "linxmicrovixserver"
            );

            RecurringJob.AddOrUpdate<ILinxProdutosDepositosService<LinxProdutosDepositos>>("LinxProdutosDepositos", service => service.IntegraRegistrosAsync(
                "LinxProdutosDepositos",
                "p_LinxProdutosDepositos_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.Daily,
                queue: "linxmicrovixserver"
            );

            RecurringJob.AddOrUpdate<ILinxProdutosDetalhesService<LinxProdutosDetalhes>>("LinxProdutosDetalhes", service => service.IntegraRegistrosAsync(
                "LinxProdutosDetalhes",
                "p_LinxProdutosDetalhes_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.Hourly,
                queue: "linxmicrovixserver"
            );

            RecurringJob.AddOrUpdate<ILinxProdutosInventarioService<LinxProdutosInventario>>("LinxProdutosInventario", service => service.IntegraRegistrosAsync(
                "LinxProdutosInventario",
                "p_LinxProdutosInventario_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.Hourly,
                queue: "linxmicrovixserver"
            );

            RecurringJob.AddOrUpdate<ILinxProdutosTabelasService<LinxProdutosTabelas>>("LinxProdutosTabelas", service => service.IntegraRegistrosAsync(
                "LinxProdutosTabelas",
                "p_LinxProdutosTabelas_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.Daily,
                queue: "linxmicrovixserver"
            );

            RecurringJob.AddOrUpdate<ILinxProdutosTabelasPrecosService<LinxProdutosTabelasPrecos>>("LinxProdutosTabelasPrecos", service => service.IntegraRegistrosNotAsync(
                "LinxProdutosTabelasPrecos",
                "p_LinxProdutosTabelasPrecos_trusted",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.Hourly,
                queue: "linxmicrovixserver"
            );

            RecurringJob.AddOrUpdate<ILinxProdutosCamposAdicionaisService<LinxProdutosCamposAdicionais>>("LinxProdutosCamposAdicionais", service => service.IntegraRegistrosAsync(
                "LinxProdutosCamposAdicionais",
                "p_LinxProdutosCamposAdicionais_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.Daily,
                queue: "linxmicrovixserver"
            );

            RecurringJob.AddOrUpdate<ILinxXMLDocumentosService<LinxXMLDocumentos>>("LinxXMLDocumentos", service => service.IntegraRegistrosAsync(
                "LinxXMLDocumentos",
                "p_LinxXMLDocumentos_Sincronizacao",
                LinxAPIAttributes.TypeEnum.Producao.ToName()),
                Cron.MinuteInterval(5),
                queue: "linxmicrovixserver"
            );
        }
        
        private static void AuthorizeNFeRecurringJobs()
        {
            RecurringJob.AddOrUpdate<IAuthorizeNFeService>("AuthorizeNFe", service => service.AuthorizeNFes("AuthorizeNFe"),
                Cron.MinuteInterval(3),
                queue: "workersserver"
            );
        }

        private static void ChangingOrderRecurringJobs()
        {
            RecurringJob.AddOrUpdate<IChangingOrderService>("ChangingOrder", service => service.ChangingOrder("ChangingOrder"),
                Cron.MinuteInterval(3),
                queue: "workersserver"
            );
        }

        private static void ChangingPasswordRecurringJobs()
        {
            RecurringJob.AddOrUpdate<IChangingPasswordService>("ChangingPassword", service => service.ChangePassword(),
                Cron.Daily,
                queue: "workersserver"
            );
        }

        //private static void InsertReverseRecurringJobs()
        //{
        //    RecurringJob.AddOrUpdate<IInsertReverseService>("IInsertReverse", service => service.InsereReversa(),
        //       Cron.MinuteInterval(3),
        //       queue: "workersserver"
        //   );
        //}

        private static void InvoicedBotsRecurringJobs()
        {
            RecurringJob.AddOrUpdate<IInvoiceOrderService>("Gabot 1 e 2", service => service.InvoiceOrder("Gabot 1 e 2"),
                Cron.MinuteInterval(1),
                queue: "workersserver"
            );

            RecurringJob.AddOrUpdate<IInvoiceOrderService>("Gabot 3 e 4", service => service.InvoiceOrder("Gabot 3 e 4"),
                Cron.MinuteInterval(1),
                queue: "workersserver"
            );

            RecurringJob.AddOrUpdate<IInvoiceOrderService>("Gabot 5 e 6", service => service.InvoiceOrder("Gabot 5 e 6"),
                Cron.MinuteInterval(1),
                queue: "workersserver"
            );

            RecurringJob.AddOrUpdate<IInvoiceOrderService>("Gabot 7 e 8", service => service.InvoiceOrder("Gabot 7 e 8"),
                Cron.MinuteInterval(1),
                queue: "workersserver"
            );

            RecurringJob.AddOrUpdate<IInvoiceOrderService>("Gabot 9 e 0", service => service.InvoiceOrder("Gabot 9 e 0"),
                Cron.MinuteInterval(1),
                queue: "workersserver"
            );

            RecurringJob.AddOrUpdate<IInvoiceOrderService>("Vanabot 0", service => service.InvoiceOrder("Vanabot 0"),
                Cron.MinuteInterval(1),
                queue: "workersserver"
            );

            RecurringJob.AddOrUpdate<IInvoiceOrderService>("Vanabot 1", service => service.InvoiceOrder("Vanabot 1"),
                Cron.MinuteInterval(1),
                queue: "workersserver"
            );

            RecurringJob.AddOrUpdate<IInvoiceOrderService>("Vanabot 2", service => service.InvoiceOrder("Vanabot 2"),
                Cron.MinuteInterval(1),
                queue: "workersserver"
            );

            RecurringJob.AddOrUpdate<IInvoiceOrderService>("Vanabot 3", service => service.InvoiceOrder("Vanabot 3"),
                Cron.MinuteInterval(1),
                queue: "workersserver"
            );

            RecurringJob.AddOrUpdate<IInvoiceOrderService>("Vanabot 4", service => service.InvoiceOrder("Vanabot 4"),
                Cron.MinuteInterval(1),
                queue: "workersserver"
            );

            RecurringJob.AddOrUpdate<IInvoiceOrderService>("Vanabot 5", service => service.InvoiceOrder("Vanabot 5"),
                Cron.MinuteInterval(1),
                queue: "workersserver"
            );

            RecurringJob.AddOrUpdate<IInvoiceOrderService>("Vanabot 6", service => service.InvoiceOrder("Vanabot 6"),
                Cron.MinuteInterval(1),
                queue: "workersserver"
            );

            RecurringJob.AddOrUpdate<IInvoiceOrderService>("Vanabot 7", service => service.InvoiceOrder("Vanabot 7"),
                Cron.MinuteInterval(1),
                queue: "workersserver"
            );

            RecurringJob.AddOrUpdate<IInvoiceOrderService>("Vanabot 8", service => service.InvoiceOrder("Vanabot 8"),
                Cron.MinuteInterval(1),
                queue: "workersserver"
            );

            RecurringJob.AddOrUpdate<IInvoiceOrderService>("Vanabot 9", service => service.InvoiceOrder("Vanabot 9"),
                Cron.MinuteInterval(1),
                queue: "workersserver"
            );
        }

    }
}
