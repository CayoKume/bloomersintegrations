using BloomersCarriersIntegrations.TotalExpress.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BloomersIntegrationsManager.UI.Controllers.Carriers
{
    [ApiController]
    [Route("NewBloomers/BloomersCarriersJobs/TotalExpress")]
    public class TotalExpressController : Controller
    {
        private readonly ITotalExpressService _totalExpressService;

        public TotalExpressController(ITotalExpressService totalExpressService) =>
            (_totalExpressService) = (totalExpressService);

        [HttpPost("TotalExpressSendOrders")]
        public async Task<ActionResult<string>> TotalExpressSendOrders()
        {
            try
            {
                await _totalExpressService.SendOrders();
                return Ok();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Erro: {ex.Message}");
            }
        }

        [HttpPost("TotalExpressSendOrder")]
        public async Task<ActionResult<string>> TotalExpressSendOrder([Required][FromQuery] string nr_pedido)
        {
            try
            {
                var result = await _totalExpressService.SendOrder(nr_pedido);

                if (result != true)
                    return BadRequest($"A API TotalExpress não conseguiu enviar o pedido: {nr_pedido}.");
                else
                    return Ok($"Pedido: {nr_pedido} enviado com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel enviar o pedido: {nr_pedido}. Erro: {ex.Message}");
            }
        }

        [HttpPost("TotalExpressSendOrderAsETUR")]
        public async Task<ActionResult<string>> TotalExpressSendOrderAsETUR([Required][FromQuery] string nr_pedido)
        {
            try
            {
                var result = await _totalExpressService.SendOrderAsEtur(nr_pedido);

                if (result != true)
                    return BadRequest($"A API TotalExpress não conseguiu enviar o pedido: {nr_pedido}.");
                else
                    return Ok($"Pedido: {nr_pedido} enviado com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel enviar o pedido: {nr_pedido}. Erro: {ex.Message}");
            }
        }

        [HttpPost("TotalExpressUpdateOrderSendLog")]
        public async Task<ActionResult<string>> TotalExpressUpdateOrderSendLog()
        {
            try
            {
                await _totalExpressService.UpdateOrderSendLog();
                return Ok();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Erro: {ex.Message}");
            }
        }
    }
}
