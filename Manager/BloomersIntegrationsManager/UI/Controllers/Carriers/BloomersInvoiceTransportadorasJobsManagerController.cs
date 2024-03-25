using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BloomersIntegrationsManager.Controllers
{
    [ApiController]
    [Route("NewBloomers/BloomersInvoiceIntegrations/Transportadoras")]
    public class BloomersInvoiceTransportadorasJobsManagerController : Controller
    {
        //private readonly ITotalExpressService _totalExpressService;
        //private readonly IMandaeService _mandaeService;

        //public BloomersInvoiceTransportadorasJobsManagerController(ITotalExpressService totalExpressService, IMandaeService mandaeService) =>
            //(_totalExpressService, _mandaeService) = (totalExpressService, mandaeService);

        //[HttpPost("TotalExpressEnviaPedidos")]
        //public async Task<ActionResult<string>> TotalExpressEnviaPedidos()
        //{
        //    try
        //    {
        //        await _totalExpressService.EnviaPedidosTotal();
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("TotalExpressEnviaPedido")]
        //public async Task<ActionResult<string>> TotalExpressEnviaPedido([Required][FromQuery] string nr_pedido)
        //{
        //    try
        //    {
        //        var result = await _totalExpressService.EnviaPedidoTotal(nr_pedido);

        //        if (result != true)
        //            return BadRequest($"A API TotalExpress não conseguiu enviar o pedido: {nr_pedido}.");
        //        else
        //            return Ok($"Pedido: {nr_pedido} enviado com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel enviar o pedido: {nr_pedido}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("TotalExpressEnviaPedidoETUR")]
        //public async Task<ActionResult<string>> TotalExpressEnviaPedidoETUR([Required][FromQuery] string nr_pedido)
        //{
        //    try
        //    {
        //        var result = await _totalExpressService.EnviaPedidoTotalETUR(nr_pedido);

        //        if (result != true)
        //            return BadRequest($"A API TotalExpress não conseguiu enviar o pedido: {nr_pedido}.");
        //        else
        //            return Ok($"Pedido: {nr_pedido} enviado com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel enviar o pedido: {nr_pedido}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("TotalExpressAtualizaLogPedidoEnviado")]
        //public async Task<ActionResult<string>> TotalExpressAtualizaLogPedidoEnviado()
        //{
        //    try
        //    {
        //       await _totalExpressService.AtualizaLogPedidoEnviado();
        //       return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Erro: {ex.Message}");
        //    }
        //}

        

        #region Mandae
        //[HttpPost("MandaeEnviaPedido")] //Descontinuado
        //public async Task<ActionResult<string>> MandaeEnviaPedido([Required][FromQuery] string nr_pedido)
        //{
        //    try
        //    {
        //        var result = await _mandaeService.EnviaPedidoMandae(nr_pedido);

        //        if (result != true)
        //            return BadRequest($"A API Mandae não conseguiu enviar o pedido: {nr_pedido}.");
        //        else
        //            return Ok($"Pedido: {nr_pedido} enviado com sucesso.");

        //        //return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel enviar o pedido: {nr_pedido}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("MandaeInsertTrackingHistory")] //Endpoint para webhook da mandae (Descontinuado)
        //public async Task<OkObjectResult> MandaeInsertTrackingHistory(TrackingRequestModel model)
        //{
        //    try
        //    {
        //        var result = await _mandaeService.InsereHistoricoDeRastreio(model);
        //        return Ok(result);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //} 
        #endregion
    }
}
