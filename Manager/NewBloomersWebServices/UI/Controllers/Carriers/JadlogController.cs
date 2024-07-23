using BloomersCarriersIntegrations.Jadlog.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace NewBloomersWebServices.UI.Controllers.Carriers
{
    [ApiController]
    [Route("NewBloomers/BloomersCarriersJobs/Jadlog")]
    public class JadlogController : Controller
    {
        private readonly IJadlogService _jadlogService;
        
        public JadlogController(IJadlogService jadlogService) =>
            (_jadlogService) = (jadlogService);

        [HttpPost("SendOrders")]
        public async Task<ActionResult<string>> SendOrdersJadlog()
        {
            try
            {
                var result = await _jadlogService.SendOrdersJadlog();

                if (result != true)
                    return BadRequest($"A API Jadlog não conseguiu enviar os pedidos.");
                else
                    return Ok($"Pedidos enviados com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Erro: {ex.Message}");
            }
        }

        [HttpPost("SendOrder")]
        public async Task<ActionResult<string>> SendOrderJadlog([Required][FromQuery] string nr_pedido)
        {
            try
            {
                var result = await _jadlogService.SendOrderJadlog(nr_pedido);

                if (result != true)
                    return BadRequest($"A API Jadlog não conseguiu enviar o pedido: {nr_pedido}.");
                else
                    return Ok($"Pedido: {nr_pedido} enviado com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel enviar o pedido: {nr_pedido}. Erro: {ex.Message}");
            }
        }

        [HttpPost("SendOrderAsEtur")]
        public async Task<ActionResult<string>> SendOrderJadlogAsEtur([Required][FromQuery] string nr_pedido)
        {
            throw new NotImplementedException();
            //try
            //{
            //    var result = await _jadlogService.se(nr_pedido);

            //    if (result != true)
            //        return BadRequest($"A API TotalExpress não conseguiu enviar o pedido: {nr_pedido}.");
            //    else
            //        return Ok($"Pedido: {nr_pedido} enviado com sucesso.");
            //}
            //catch (Exception ex)
            //{
            //    Response.StatusCode = 400;
            //    return Content($"Nao foi possivel enviar o pedido: {nr_pedido}. Erro: {ex.Message}");
            //}
        }

        [HttpPost("UpdateShippedOrdersLog")]
        public async Task<ActionResult<string>> UpdateShippedOrdersLog([Required][FromQuery] string nr_pedido)
        {
            throw new NotImplementedException();
            //try
            //{
            //    var result = await _jadlogService.se(nr_pedido);

            //    if (result != true)
            //        return BadRequest($"A API TotalExpress não conseguiu enviar o pedido: {nr_pedido}.");
            //    else
            //        return Ok($"Pedido: {nr_pedido} enviado com sucesso.");
            //}
            //catch (Exception ex)
            //{
            //    Response.StatusCode = 400;
            //    return Content($"Nao foi possivel enviar o pedido: {nr_pedido}. Erro: {ex.Message}");
            //}
        }
    }
}
