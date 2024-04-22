using BloomersCarriersIntegrations.FlashCourier.Application.Services;
using BloomersCarriersIntegrations.FlashCourier.Infrastructure.Repositorys;
using BloomersCarriersIntegrations.TotalExpress.Application.Services;
using BloomersCarriersIntegrations.TotalExpress.Infrastructure.Repositorys;
using BloomersCommerceIntegrations.LinxCommerce.Application.Services;
using BloomersCommerceIntegrations.LinxCommerce.Domain.Entities;
using BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Repositorys;
using BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Repositorys.Base;
using BloomersGeneralIntegrations.AfterSale.Application.Services;
using BloomersGeneralIntegrations.AfterSale.Infrastructure.Repositorys;
using BloomersGeneralIntegrations.Dootax.Application.Services;
using BloomersGeneralIntegrations.Dootax.Infrastructure.Repositorys;
using BloomersGeneralIntegrations.Mobsim.Application.Services;
using BloomersGeneralIntegrations.Mobsim.Infrastructure.Repositorys;
using BloomersGeneralIntegrations.Movidesk.Application.Services;
using BloomersGeneralIntegrations.Movidesk.Infrastructure.Repositorys;
using BloomersGeneralIntegrations.Pagarme.Application.Services;
using BloomersGeneralIntegrations.Pagarme.Infrastructure.Repositorys;
using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using BloomersIntegrationsManager.Domain.Filters;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxCommerce;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxEcommerce;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxCommerce;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix;
using BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.Base;
using Hangfire;
using Hangfire.SqlServer;

namespace BloomersIntegrationsManager.Domain.Extensions
{
    public static class ServicesExtensions
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("Connection");

            builder.Services.AddScopedSQLServerConnection();
            builder.Services.AddScopedCarriersServices();
            builder.Services.AddScopedLinxCommerceServices();
            builder.Services.AddScopedLinxMicrovixCommerceServices();
            builder.Services.AddScopedLinxMicrovixERPWsSaidaServices();
            builder.Services.AddScopedLinxMicrovixERPWsEntradaServices();
            builder.Services.AddScopedGeneralServices();
            builder.Services.AddHangfireService(connectionString);

            return builder;
        }

        public static IServiceCollection AddScopedLinxCommerceServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(ILinxCommerceRepositoryBase<>), typeof(LinxCommerceRepositoryBase<>));
            services.AddScoped<BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Apis.IAPICall, BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Apis.APICall>();
            services.AddHttpClient("LinxCommerceAPI", client =>
            {
                client.BaseAddress = new Uri("https://misha.layer.core.dcg.com.br");
                client.Timeout = new TimeSpan(0, 2, 0);
            });

            services.AddScoped<IOrderService<SearchOrderResponse.Root>, OrderService<SearchOrderResponse.Root>>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<IProductService<SearchProductResponse.Root>, ProductService<SearchProductResponse.Root>>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<ISKUService<SearchSKUResponse.Root>, SKUService<SearchSKUResponse.Root>>();
            services.AddScoped<ISKURepository, SKURepository>();

            return services;
        }

        public static IServiceCollection AddScopedLinxMicrovixCommerceServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(ILinxMicrovixRepositoryBase<>), typeof(LinxMicrovixRepositoryBase<>));
            services.AddScoped<BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Apis.IAPICall, BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Apis.APICall>();

            services.AddScoped<IB2CConsultaClientesServices<B2CConsultaClientes>, B2CConsultaClientesService<B2CConsultaClientes>>();
            services.AddScoped<IB2CConsultaClientesRepository, B2CConsultaClientesRepository>();

            services.AddScoped<IB2CConsultaNFeService<B2CConsultaNFe>, B2CConsultaNFeService<B2CConsultaNFe>>();
            services.AddScoped<IB2CConsultaNFeRepository, B2CConsultaNFeRepository>();

            services.AddScoped<IB2CConsultaNFeSituacaoService<B2CConsultaNFeSituacao>, B2CConsultaNFeSituacaoService<B2CConsultaNFeSituacao>>();
            services.AddScoped<IB2CConsultaNFeSituacaoRepository, B2CConsultaNFeSituacaoRepository>();

            services.AddScoped<IB2CConsultaPedidosItensService<B2CConsultaPedidosItens>, B2CConsultaPedidosItensService<B2CConsultaPedidosItens>>();
            services.AddScoped<IB2CConsultaPedidosItensRepository, B2CConsultaPedidosItensRepository>();

            services.AddScoped<IB2CConsultaPedidosService<B2CConsultaPedidos>, B2CConsultaPedidosService<B2CConsultaPedidos>>();
            services.AddScoped<IB2CConsultaPedidosRepository, B2CConsultaPedidosRepository>();

            services.AddScoped<IB2CConsultaPedidosStatusService<B2CConsultaPedidosStatus>, B2CConsultaPedidosStatusService<B2CConsultaPedidosStatus>>();
            services.AddScoped<IB2CConsultaPedidosStatusRepository, B2CConsultaPedidosStatusRepository>();

            services.AddScoped<IB2CConsultaStatusService<B2CConsultaStatus>, B2CConsultaStatusService<B2CConsultaStatus>>();
            services.AddScoped<IB2CConsultaStatusRepository, B2CConsultaStatusRepository>();

            return services;
        }

        public static IServiceCollection AddScopedLinxMicrovixERPWsSaidaServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(ILinxMicrovixRepositoryBase<>), typeof(LinxMicrovixRepositoryBase<>));
            services.AddScoped<BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Apis.IAPICall, BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Apis.APICall>();

            services.AddScoped<ILinxClientesFornecService<LinxClientesFornec>, LinxClientesFornecService<LinxClientesFornec>>();
            services.AddScoped<ILinxClientesFornecRepository, LinxClientesFornecRepository>();

            services.AddScoped<ILinxMovimentoService<LinxMovimento>, LinxMovimentoService<LinxMovimento>>();
            services.AddScoped<ILinxMovimentoRepository, LinxMovimentoRepository>();

            services.AddScoped<ILinxMovimentoCartoesService<LinxMovimentoCartoes>, LinxMovimentoCartoesService<LinxMovimentoCartoes>>();
            services.AddScoped<ILinxMovimentoCartoesRepository, LinxMovimentoCartoesRepository>();

            services.AddScoped<ILinxXMLDocumentosService<LinxXMLDocumentos>, LinxXMLDocumentosService<LinxXMLDocumentos>>();
            services.AddScoped<ILinxXMLDocumentosRepository, LinxXMLDocumentosRepository>();

            services.AddScoped<ILinxPedidosCompraService<LinxPedidosCompra>, LinxPedidosCompraService<LinxPedidosCompra>>();
            services.AddScoped<ILinxPedidosCompraRepository, LinxPedidosCompraRepository>();

            services.AddScoped<ILinxPedidosVendaService<LinxPedidosVenda>, LinxPedidosVendaService<LinxPedidosVenda>>();
            services.AddScoped<ILinxPedidosVendaRepository, LinxPedidosVendaRepository>();

            services.AddScoped<ILinxProdutosService<LinxProdutos>, LinxProdutosService<LinxProdutos>>();
            services.AddScoped<ILinxProdutosRepository, LinxProdutosRepository>();

            services.AddScoped<ILinxProdutosDepositosService<LinxProdutosDepositos>, LinxProdutosDepositosService<LinxProdutosDepositos>>();
            services.AddScoped<ILinxProdutosDepositosRepository, LinxProdutosDepositosRepository>();

            services.AddScoped<ILinxProdutosInventarioService<LinxProdutosInventario>, LinxProdutosInventarioService<LinxProdutosInventario>>();
            services.AddScoped<ILinxProdutosInventarioRepository, LinxProdutosInventarioRepository>();

            services.AddScoped<ILinxProdutosDetalhesService<LinxProdutosDetalhes>, LinxProdutosDetalhesService<LinxProdutosDetalhes>>();
            services.AddScoped<ILinxProdutosDetalhesRepository, LinxProdutosDetalhesRepository>();

            services.AddScoped<ILinxProdutosPromocoesService<LinxProdutosPromocoes>, LinxProdutosPromocoesService<LinxProdutosPromocoes>>();
            services.AddScoped<ILinxProdutosPromocoesRepository, LinxProdutosPromocoesRepository>();

            services.AddScoped<ILinxProdutosTabelasService<LinxProdutosTabelas>, LinxProdutosTabelasService<LinxProdutosTabelas>>();
            services.AddScoped<ILinxProdutosTabelasRepository, LinxProdutosTabelasRepository>();

            services.AddScoped<ILinxProdutosTabelasPrecosService<LinxProdutosTabelasPrecos>, LinxProdutosTabelasPrecosService<LinxProdutosTabelasPrecos>>();
            services.AddScoped<ILinxProdutosTabelasPrecosRepository, LinxProdutosTabelasPrecosRepository>();

            services.AddScoped<ILinxProdutosCamposAdicionaisService<LinxProdutosCamposAdicionais>, LinxProdutosCamposAdicionaisService<LinxProdutosCamposAdicionais>>();
            services.AddScoped<ILinxProdutosCamposAdicionaisRepository, LinxProdutosCamposAdicionaisRepository>();

            services.AddScoped<ILinxPlanosService<LinxPlanos>, LinxPlanosService<LinxPlanos>>();
            services.AddScoped<ILinxPlanosRepository, LinxPlanosRepository>();

            services.AddScoped<ILinxGrupoLojasService<LinxGrupoLojas>, LinxGrupoLojasService<LinxGrupoLojas>>();
            services.AddScoped<ILinxGrupoLojasRepository, LinxGrupoLojasRepository>();

            services.AddScoped<ILinxMovimentoPlanosService<LinxMovimentoPlanos>, LinxMovimentoPlanosService<LinxMovimentoPlanos>>();
            services.AddScoped<ILinxMovimentoPlanosRepository, LinxMovimentoPlanosRepository>();

            return services;
        }

        public static IServiceCollection AddScopedLinxMicrovixERPWsEntradaServices(this IServiceCollection services)
        {
            services.AddScoped<BloomersMicrovixIntegrations.LinxMicrovixWsEntrada.Infrastructure.Apis.IAPICall, BloomersMicrovixIntegrations.LinxMicrovixWsEntrada.Infrastructure.Apis.APICall>();
            services.AddHttpClient("LinxMicrovixWsEntradaAPI", client =>
            {
                client.BaseAddress = new Uri("https://webapi.microvix.com.br/1.0/importador.svc");
                client.Timeout = new TimeSpan(0, 2, 0);
            });
            return services;
        }

        public static IServiceCollection AddScopedWmsServices(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddScopedCarriersServices(this IServiceCollection services)
        {
            services.AddScoped<BloomersCarriersIntegrations.FlashCourier.Infrastructure.Apis.IAPICall, BloomersCarriersIntegrations.FlashCourier.Infrastructure.Apis.APICall>();
            services.AddHttpClient("FlashCourierAPI", client =>
            {
                //HOMOLOG
                //https://homolog.flashpegasus.com.br/FlashPegasus/rest

                client.BaseAddress = new Uri("https://webservice.flashpegasus.com.br/FlashPegasus/rest");
                client.Timeout = new TimeSpan(0, 2, 0);
            });

            services.AddScoped<IFlashCourierService, FlashCourierService>();
            services.AddScoped<IFlashCourierRepository, FlashCourierRepository>();

            services.AddScoped<BloomersCarriersIntegrations.TotalExpress.Infrastructure.Apis.IAPICall, BloomersCarriersIntegrations.TotalExpress.Infrastructure.Apis.APICall>();
            services.AddHttpClient("TotalExpressAPI", client =>
            {
                client.BaseAddress = new Uri("https://apis.totalexpress.com.br/");
                client.Timeout = new TimeSpan(0, 2, 0);
            });
            services.AddHttpClient("TotalExpressEdiAPI", client =>
            {
                client.BaseAddress = new Uri("https://edi.totalexpress.com.br/");
                client.Timeout = new TimeSpan(0, 2, 0);
            });
            services.AddScoped<ITotalExpressService, TotalExpressService>();
            services.AddScoped<ITotalExpressRepository, TotalExpressRepository>();

            return services;
        }

        public static IServiceCollection AddScopedGeneralServices(this IServiceCollection services)
        {
            services.AddScoped<BloomersGeneralIntegrations.AfterSale.Infrastructure.Apis.IAPICall, BloomersGeneralIntegrations.AfterSale.Infrastructure.Apis.APICall>();
            services.AddHttpClient("AfterSaleAPI", client =>
            {
                client.BaseAddress = new Uri("https://api.send4.com.br/v3/api/reverses?");
                client.Timeout = new TimeSpan(0, 2, 0);
            });

            services.AddScoped<IAfterSaleService, AfterSaleService>();
            services.AddScoped<IAfterSaleRepository, AfterSaleRepository>();

            services.AddScoped<BloomersGeneralIntegrations.Dootax.Infrastructure.Apis.IAPICall, BloomersGeneralIntegrations.Dootax.Infrastructure.Apis.APICall>();
            services.AddHttpClient("DootaxAPI", client =>
            {
                //HOMOLOG
                //https://hom.app.dootax.com.br

                client.BaseAddress = new Uri("https://app.dootax.com.br");
                client.Timeout = new TimeSpan(0, 2, 0);
            });

            services.AddScoped<IDootaxService, DootaxService>();
            services.AddScoped<IDootaxRepository, DootaxRepository>();

            services.AddScoped<BloomersGeneralIntegrations.Mobsim.Infrastructure.Apis.IAPICall, BloomersGeneralIntegrations.Mobsim.Infrastructure.Apis.APICall>();
            services.AddHttpClient("MobsimAPI", client =>
            {
                client.BaseAddress = new Uri("https://mobsim-api.com.br");
                client.Timeout = new TimeSpan(0, 2, 0);
            });

            services.AddScoped<IMobsimService, MobsimService>();
            services.AddScoped<IMobsimRepository, MobsimRepository>();

            services.AddScoped<BloomersGeneralIntegrations.Pagarme.Infrastructure.Apis.IAPICall, BloomersGeneralIntegrations.Pagarme.Infrastructure.Apis.APICall>();
            services.AddHttpClient("PagarmeAPI", client =>
            {
                client.BaseAddress = new Uri("https://api.pagar.me/core/v5/payables?created_since=");
                client.Timeout = new TimeSpan(0, 2, 0);
            });

            services.AddScoped<IPagarmeService, PagarmeService>();
            services.AddScoped<IPagarmeRepository, PagarmeRepository>();

            services.AddScoped<BloomersGeneralIntegrations.Movidesk.Infrastructure.Apis.IAPICall, BloomersGeneralIntegrations.Movidesk.Infrastructure.Apis.APICall>();
            services.AddHttpClient("MovideskAPI", client =>
            {
                client.BaseAddress = new Uri("https://api.movidesk.com");
                client.Timeout = new TimeSpan(0, 2, 0);
            });

            services.AddScoped<IMovideskService, MovideskService>();
            services.AddScoped<IMovideskRepository, MovideskRepository>();

            return services;
        }

        public static IServiceCollection AddScopedSQLServerConnection(this IServiceCollection services)
        {
            services.AddScoped<ISQLServerConnection, SQLServerConnection>();

            return services;
        }

        public static IServiceCollection AddHangfireService(this IServiceCollection services, string connectionString)
        {
            services.AddHangfire(configuration => configuration
                .UseFilter(new AutomaticRetryAttribute { Attempts = 0 })
                .UseFilter(new WorkflowJobFailureAttribute())
                .UseFilter(new DisableConcurrentExecutionWithParametersAttribute())

                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            services.AddHangfireServer(options =>
            {
                options.WorkerCount = 50;
            });

            return services;
        }
    }
}
