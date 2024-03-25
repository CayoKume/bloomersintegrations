using Microsoft.AspNetCore.Mvc;

namespace BloomersIntegrationsManager.Controllers
{
    public class BloomersGeneralJobsManagerController : Controller
    {
        //private readonly IMobsimService _mobsimService;
        //private readonly IDootaxService _dootaxService;
        //private readonly IAfterSaleService _afterSaleService;
        //private readonly IPagarmeService _pagarmeService;

        //public BloomersGeneralJobsManagerController(IMobsimService mobsimService, IDootaxService dootaxService, IAfterSaleService afterSaleService, IPagarmeService pagarmeService) =>
        //    (_mobsimService, _dootaxService, _afterSaleService, _pagarmeService) = (mobsimService, dootaxService, afterSaleService, pagarmeService);

        //[HttpPost("EnviaMensagensPedidoFaturado")]
        //public async Task<ActionResult> EnviaMensagensPedidoFaturado()
        //{
        //    try
        //    {
        //        await _mobsimService.EnviaMensagemPedidoFaturado();

        //        return Ok($"Mesagens dos pedidos faturados enviadas com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel enviar as mensagens dos pedidos faturados. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("EnviaMensagensPedidoExpedido")]
        //public async Task<ActionResult> EnviaMensagensPedidoExpedido()
        //{
        //    try
        //    {
        //        await _mobsimService.EnviaMensagemPedidoExpedido();

        //        return Ok($"Mesagens dos pedidos expedidos enviadas com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel enviar as mensagens dos pedidos expedidos. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("EnviaMensagensPedidoEntregue")]
        //public async Task<ActionResult> EnviaMensagensPedidoEntregue()
        //{
        //    try
        //    {
        //        await _mobsimService.EnviaMensagemPedidoEntregue();

        //        return Ok($"Mesagens dos pedidos entregues enviadas com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel enviar as mensagens dos pedidos entregues. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("EnviaXMLDootax")]
        //public async Task<ActionResult> EnviaXMLDootax()
        //{
        //    try
        //    {
        //        await _dootaxService.EnviaXML();

        //        return Ok($"XMLs dos pedidos faturados enviadas com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel enviar os xmls dos pedidos faturados. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("GetAfterSaleReversas")]
        //public async Task<ActionResult> GetAfterSaleReversas()
        //{
        //    try
        //    {
        //        await _afterSaleService.GetReverses();

        //        return Ok($"Reversas dos pedidos inseridoas com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel obter as reversas dos pedidos. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("ObterRecebiveisPagarme")]
        //public async Task<ActionResult> ObterRecebiveisPagarme()
        //{
        //    try
        //    {
        //        await _pagarmeService.GetRecebiveis();

        //        return Ok($"Recebíveis dos pedidos inseridoas com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel obter os recebíveis dos pedidos. Erro: {ex.Message}");
        //    }
        //}
    }
}
