using BloomersCommerceIntegrations.LinxCommerce.Application.Services;
using BloomersCommerceIntegrations.LinxCommerce.Domain.Entities;
using BloomersIntegrationsManager.Domain.Entities.Request;
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

        [HttpPost("Pedido")]
        public async Task<ActionResult> IntegraPedido([FromBody] LinxCommerceOrderRequest request)
        {
            try
            {
                await _linxOrderService.IntegraRegistrosIndividual("HOMOLOG_LINX_COMMERCE", request.orderNumber);

                //if (result != true)
                //    return BadRequest($"A API Pedido não conseguiu integrar o pedido: {request.orderNumber}.");
                //else
                    return Ok($"Pedido: {request.orderNumber} integrado com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar o pedido: {request.orderNumber} . Erro: {ex.Message}");
            }
        }

        [HttpPost("Pedidos")]
        public async Task<ActionResult> IntegraPedidos()
        {
            try
            {
                await _linxOrderService.IntegraRegistros("HOMOLOG_LINX_COMMERCE");
                return Ok($"Pedidos integrados com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar os pedidos. Erro: {ex.Message}");
            }
        }

        [HttpPost("Produtos")]
        public async Task<ActionResult> IntegraProdutos()
        {
            try
            {
                await _linxProductService.IntegraRegistros("HOMOLOG_LINX_COMMERCE");
                return Ok($"Produtos integrados com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar os produtos. Erro: {ex.Message}");
            }
        }

        [HttpPost("SKUs")]
        public async Task<ActionResult> IntegraSKUsBase()
        {
            try
            {
                await _linxSKUService.IntegraRegistros("HOMOLOG_LINX_COMMERCE");
                return Ok($"SKUs integrados com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar os SKUs. Erro: {ex.Message}");
            }
        }
    }
}
