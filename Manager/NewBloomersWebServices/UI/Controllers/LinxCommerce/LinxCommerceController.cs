using BloomersCommerceIntegrations.LinxCommerce.Application.Services;
using BloomersCommerceIntegrations.LinxCommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
        public async Task<ActionResult> IntegraPedido([Required][FromQuery] string nr_pedido)
        {
            try
            {
                var result = await _linxOrderService.IntegraRegistrosIndividual("LINX_COMMERCE", nr_pedido);

                if (result != true)
                    return BadRequest($"A API Pedido não conseguiu integrar o pedido: {nr_pedido}.");
                else
                    return Ok($"Pedido: {nr_pedido} integrado com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar o pedido: {nr_pedido} . Erro: {ex.Message}");
            }
        }

        [HttpPost("Pedidos")]
        public async Task<ActionResult> IntegraPedidos()
        {
            try
            {
                await _linxOrderService.IntegraRegistros("LINX_COMMERCE");
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
                await _linxProductService.IntegraRegistros("LINX_COMMERCE");
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
                await _linxSKUService.IntegraRegistros("LINX_COMMERCE");
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
