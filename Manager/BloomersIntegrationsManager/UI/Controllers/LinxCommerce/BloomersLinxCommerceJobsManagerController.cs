using BloomersCommerceIntegrations.LinxCommerce.Application.Services;
using BloomersCommerceIntegrations.LinxCommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BloomersIntegrationsManager.Controllers
{
    [ApiController]
    [Route("NewBloomers/BloomersLinxCommerceIntegrations/LinxCommerce")]
    public class BloomersLinxCommerceJobsManagerController : Controller
    {
        private readonly IOrderService<SearchOrderResponse.Root> _linxOrderService;
        //private readonly ILinxPersonService<GetPersonResponse.Root> _linxPersonService;
        //private readonly ILinxSKUService<SKU> _linxSKUService;
        //private readonly ILinxProductService<SearchProductResponse.Root> _linxProductService;

        public BloomersLinxCommerceJobsManagerController(
            IOrderService<SearchOrderResponse.Root> linxOrderService
            //ILinxPersonService<GetPersonResponse.Root> linxPersonService,
            //ILinxSKUService<SKU> linxSKUService,
            //ILinxProductService<SearchProductResponse.Root> linxProductService
        ) =>
            (_linxOrderService/*, _linxPersonService, _linxSKUService, _linxProductService*/) =
            (linxOrderService/*, linxPersonService, linxSKUService, linxProductService*/);

        [HttpPost("Pedidos")]
        public async Task<ActionResult> IntegraPedidos()
        {
            try
            {
                await _linxOrderService.IntegraRegistros("Order", "p_Order_trusted", "LINX_COMMERCE");

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

        //[HttpPost("Clientes")]
        //public async Task<ActionResult> IntegraClientes()
        //{
        //    try
        //    {
        //        await _linxPersonService.IntegraRegistros("Person", "p_Person_trusted", "LINX_COMMERCE");

        //        //if (result != true)
        //        //    return BadRequest($"A API Pedidos não encontrou o pedido: .");
        //        //else
        //        return Ok($"Cliente:  integrado com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel integrar o cliente: . Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("SKUs")]
        //public async Task<ActionResult> IntegraSKUsBase()
        //{
        //    try
        //    {
        //        await _linxSKUService.IntegraRegistros("SkuBase", "p_SkuBase_trusted", "LINX_COMMERCE");

        //        //if (result != true)
        //        //    return BadRequest($"A API Pedidos não encontrou o pedido: .");
        //        //else
        //        return Ok($"Cliente:  integrado com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel integrar o cliente: . Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("Produtos")]
        //public async Task<ActionResult> IntegraProdutos()
        //{
        //    try
        //    {
        //        await _linxProductService.IntegraRegistros("Product", "p_Product_trusted", "LINX_COMMERCE");

        //        //if (result != true)
        //        //    return BadRequest($"A API Pedidos não encontrou o pedido: .");
        //        //else
        //        return Ok($"Cliente:  integrado com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel integrar o cliente: . Erro: {ex.Message}");
        //    }
        //}
    }
}
