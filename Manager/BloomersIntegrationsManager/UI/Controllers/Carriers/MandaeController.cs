using Microsoft.AspNetCore.Mvc;

namespace BloomersIntegrationsManager.UI.Controllers.Carriers
{
    //[ApiController]
    //[Route("NewBloomers/BloomersCarriersJobs/Mandae")]
    public class MandaeController : Controller
    {
        //private readonly IMandaeService _mandaeService;

        //public MandaeController(IMandaeService mandaeService) =>
        //    (_mandaeService) = (mandaeService);

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

    }
}
