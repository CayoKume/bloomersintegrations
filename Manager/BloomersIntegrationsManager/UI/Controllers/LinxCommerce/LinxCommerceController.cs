using BloomersCommerceIntegrations.LinxCommerce.Application.Services;
using BloomersCommerceIntegrations.LinxCommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BloomersIntegrationsManager.UI.Controllers.LinxCommerce
{
    [ApiController]
    [Route("NewBloomers/BloomersEcommerceJobs/LinxCommerce")]
    public class LinxCommerceController : Controller
    {
        private readonly IOrderService<SearchOrderResponse.Root> _linxOrderService;
        private readonly ISKUService<SearchSKUResponse.Root> _linxSKUService;
        private readonly IProductService<SearchProductResponse.Root> _linxProductService;

        public LinxCommerceController(
            IOrderService<SearchOrderResponse.Root> linxOrderService,
            ISKUService<SearchSKUResponse.Root> linxSKUService,
            IProductService<SearchProductResponse.Root> linxProductService
        ) =>
            (_linxOrderService, _linxSKUService, _linxProductService) =
            (linxOrderService, linxSKUService, linxProductService);

        [HttpPost("Pedidos")]
        public async Task<ActionResult> IntegraPedidos()
        {
            try
            {
                await _linxOrderService.IntegraRegistros("HOMOLOG_LINX_COMMERCE");

                //if (result != true)
                //    return BadRequest($"A API Pedidos não encontrou o pedido: .");
                //else
                return Ok($"Pedido:  integrado com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar o pedido: . Erro: {ex.Message}");
            }
        }

        [HttpPost("Produtos")]
        public async Task<ActionResult> IntegraProdutos()
        {
            try
            {
                await _linxProductService.IntegraRegistros("HOMOLOG_LINX_COMMERCE");

                //if (result != true)
                //    return BadRequest($"A API Pedidos não encontrou o pedido: .");
                //else
                return Ok($"Cliente:  integrado com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar o cliente: . Erro: {ex.Message}");
            }
        }

        [HttpPost("SKUs")]
        public async Task<ActionResult> IntegraSKUsBase()
        {
            try
            {
                await _linxSKUService.IntegraRegistros("HOMOLOG_LINX_COMMERCE");

                //if (result != true)
                //    return BadRequest($"A API Pedidos não encontrou o pedido: .");
                //else
                return Ok($"Cliente:  integrado com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar o cliente: . Erro: {ex.Message}");
            }
        }
    }
}
