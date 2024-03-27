using BloomersCarriersIntegrations.FlashCourier.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BloomersIntegrationsManager.UI.Controllers.BloomersCarriersJobs
{
    [ApiController]
    [Route("NewBloomers/BloomersCarriersJobs/FlashCourier")]
    public class FlashCourierController : Controller
    {
        private readonly IFlashCourierService _flashCourierService;

        public FlashCourierController(IFlashCourierService flashCourierService) =>
            (_flashCourierService) = (flashCourierService);

        [HttpPost("EnviaPedidos")]
        public async Task<ActionResult<string>> FlashCourierEnviaPedidos()
        {
            try
            {
                await _flashCourierService.EnviaPedidosFlash();
                return Ok();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Erro: {ex.Message}");
            }
        }

        [HttpPost("EnviaPedido")]
        public async Task<ActionResult<string>> FlashCourierEnviaPedido([Required][FromQuery] string nr_pedido)
        {
            try
            {
                var result = await _flashCourierService.EnviaPedidoFlash(nr_pedido);

                if (result != true)
                    return BadRequest($"A API FlashCourier não conseguiu enviar o pedido: {nr_pedido}.");
                else
                    return Ok($"Pedido: {nr_pedido} enviado com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel enviar o pedido: {nr_pedido}. Erro: {ex.Message}");
            }
        }

        [HttpPost("AtualizaLogPedidoEnviado")]
        public async Task<ActionResult<string>> FlashCourierAtualizaLogPedidoEnviado()
        {
            try
            {
                await _flashCourierService.AtualizaLogPedidoEnviado();

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
