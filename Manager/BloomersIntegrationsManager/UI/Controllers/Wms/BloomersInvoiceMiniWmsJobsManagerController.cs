using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BloomersIntegrationsManager.Controllers
{
    [ApiController]
    [Route("NewBloomers/BloomersInvoiceIntegrations/MiniWms")]
    public class BloomersInvoiceMiniWmsJobsManagerController : Controller
    {
        //private readonly IImprimeEtiquetasService _imprimeEtiquetasService;
        //private readonly IImprimeRomaneioService _imprimeRomaneioService;
        //private readonly IConferePedidoService _conferePedidoService;
        //private readonly IConfiguracoesGlobaisService _configuracoesGlobaisService;

        //public BloomersInvoiceMiniWmsJobsManagerController(IImprimeEtiquetasService imprimeEtiquetasService, IImprimeRomaneioService imprimeRomaneioService, IConferePedidoService conferePedidoService, IConfiguracoesGlobaisService configuracoesGlobaisService) =>
        //    (_imprimeEtiquetasService, _imprimeRomaneioService, _conferePedidoService, _configuracoesGlobaisService) = (imprimeEtiquetasService, imprimeRomaneioService, conferePedidoService, configuracoesGlobaisService);

        //[HttpGet("GetPedidoNaoConferido")]
        //public ActionResult<string> GetPedidoNaoConferido([Required][FromQuery] string cnpj_emp, [Required][FromQuery] string serie, [Required][FromQuery] string nr_pedido)
        //{
        //    try
        //    {
        //        var result = _conferePedidoService.GetPedidoNaoConferido(cnpj_emp, serie, nr_pedido);

        //        if (String.IsNullOrEmpty(result))
        //            return BadRequest($"Nao foi possivel encontrar o pedido: {nr_pedido}.");
        //        else
        //            return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel encontrar o pedido: {nr_pedido}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpGet("GetPedidosNaoConferidos")]
        //public async Task<ActionResult<string>> GetPedidosNaoConferidos([Required][FromQuery] string cnpj_emp, [Required][FromQuery] string serie, [Required][FromQuery] string data_inicial, [Required][FromQuery] string data_final)
        //{
        //    try
        //    {
        //        var result = await _conferePedidoService.GetPedidosNaoConferidos(cnpj_emp, serie, data_inicial, data_final);

        //        if (String.IsNullOrEmpty(result))
        //            return BadRequest($"Nao foi possivel encontrar os pedidos para o intervalo de datas determinado.");
        //        else
        //            return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel encontrar os pedidos para o intervalo de datas determinado. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("UpdateRetornoIT4_WMS_DOCUMENTO")]
        //public async Task<ActionResult> UpdateRetornoIT4_WMS_DOCUMENTO([FromBody] UpdateRetornoIT4_WMS_DOCUMENTORequest request)
        //{
        //    try
        //    {
        //        var result = await _conferePedidoService.UpdateRetornoIT4_WMS_DOCUMENTO(request.nr_pedido, request.volumes, JsonConvert.SerializeObject(request.itens));

        //        if (!result)
        //            return BadRequest($"Nao foi possivel atualizar o retorno do pedido na tabela.");
        //        else
        //            return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel atualizar o retorno do pedido na tabela. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPut("UpdateTransportadoraIT4_WMS_DOCUMENTO")]
        //public async Task<ActionResult> UpdateTransportadoraIT4_WMS_DOCUMENTO([Required][FromQuery] string nr_pedido, [Required][FromQuery] int cod_transportadora)
        //{
        //    try
        //    {
        //        var result = await _conferePedidoService.UpdateTransportadoraIT4_WMS_DOCUMENTO(nr_pedido, cod_transportadora);

        //        if (!result)
        //            return BadRequest($"Nao foi possivel atualizar a transportadora do pedido: {nr_pedido} para {cod_transportadora}.");
        //        else
        //            return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel atualizar a transportadora do pedido: {nr_pedido} para {cod_transportadora}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("EnviaZPLToAPI")]

        //public async Task<ActionResult<string>> EnviaZPLToAPI([FromBody] EnviaZPLToAPIRequest request)
        //{
        //    try
        //    {
        //        var result = await _imprimeEtiquetasService.EnviaZPLToAPI(request.zpl, request.nr_pedido, request.volumes);

        //        if (!result)
        //            return BadRequest($"Nao foi possivel gerar a etiqueta do pedido: {request.nr_pedido}.");
        //        else
        //            return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel gerar a etiqueta do pedido: {request.nr_pedido}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpGet("GetEtiquetaParaImprimir")]
        //public async Task<ActionResult<string>> GetEtiquetaParaImprimir([Required][FromQuery] string fileName)
        //{
        //    try
        //    {
        //        var result = await _imprimeEtiquetasService.GetEtiquetaParaImprimir(fileName);

        //        if (String.IsNullOrEmpty(result))
        //            return BadRequest($"Nao foi possivel encontrar a etiqueta: {fileName}.");
        //        else
        //            return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel encontrar o etiqueta: {fileName}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpGet("GetPedidoParaImprimir")]
        //public ActionResult<string> GetPedidoParaImprimir([Required][FromQuery] string cnpj_emp, [Required][FromQuery] string serie, [Required][FromQuery] string nr_pedido)
        //{
        //    try
        //    {
        //        var result = _imprimeEtiquetasService.GetPedidoParaImprimir(cnpj_emp, serie, nr_pedido);

        //        if (String.IsNullOrEmpty(result))
        //            return BadRequest($"Nao foi possivel encontrar o pedido: {nr_pedido}.");
        //        else
        //            return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel encontrar o pedido: {nr_pedido}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpGet("GetPedidosParaImprimir")]
        //public async Task<ActionResult<string>> GetPedidosParaImprimir([Required][FromQuery] string cnpj_emp, [Required][FromQuery] string serie, [Required][FromQuery] string data_inicial, [Required][FromQuery] string data_final)
        //{
        //    try
        //    {
        //        var result = await _imprimeEtiquetasService.GetPedidosParaImprimir(cnpj_emp, serie, data_inicial, data_final);

        //        if (String.IsNullOrEmpty(result))
        //            return BadRequest($"Nao foi possivel encontrar os pedidos para o intervalo de datas determinado.");
        //        else
        //            return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel encontrar os pedidos para o intervalo de datas determinado. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPut("UpdateNB_ETIQUETA_IMPRESSA_IT4_WMS_DOCUMENTO")]
        //public async Task<ActionResult> UpdateNB_ETIQUETA_IMPRESSA_IT4_WMS_DOCUMENTO([Required][FromQuery] string nr_pedido)
        //{
        //    try
        //    {
        //        var result = await _imprimeEtiquetasService.UpdateNB_ETIQUETA_IMPRESSA_IT4_WMS_DOCUMENTO(
        //                nr_pedido
        //            );

        //        if (!result)
        //            return BadRequest($"Nao foi possivel atualizar o retorno do pedido na tabela.");
        //        else
        //            return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel atualizar o retorno do pedido na tabela. Erro: {ex.Message}");
        //    }
        //}

        //[HttpGet("GetPedidoEnviado")]
        //public ActionResult<string> GetPedidoEnviado([Required][FromQuery] string cnpj_emp, [Required][FromQuery] string serie, [Required][FromQuery] string nr_pedido, [Required][FromQuery] string cod_transportadora)
        //{
        //    try
        //    {
        //        var result = _imprimeRomaneioService.GetPedidoEnviado(nr_pedido, serie, cnpj_emp, cod_transportadora);

        //        if (String.IsNullOrEmpty(result))
        //            return BadRequest($"Nao foi possivel encontrar o pedido: {nr_pedido}.");
        //        else
        //            return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel encontrar o pedido: {nr_pedido}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpGet("GetPedidosEnviados")]
        //public async Task<ActionResult<string>> GetPedidosEnviados([Required][FromQuery] string cod_transportadora, [Required][FromQuery] string cnpj_emp, [Required][FromQuery] string serie, [Required][FromQuery] string data_inicial, [Required][FromQuery] string data_final)
        //{
        //    try
        //    {
        //        var result = await _imprimeRomaneioService.GetPedidosEnviados(cod_transportadora, cnpj_emp, serie, data_inicial, data_final);

        //        if (String.IsNullOrEmpty(result))
        //            return BadRequest($"Nao foi possivel encontrar os pedidos para o intervalo de datas determinado.");
        //        else
        //            return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel encontrar os pedidos para o intervalo de datas determinado. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("ImprimeRomaneio")]
        //public async Task<ActionResult<string>> ImprimeRomaneio([FromBody] ImprimeRomaneioRequest request)
        //{
        //    try
        //    {
        //        var result = await _imprimeRomaneioService.ImprimePedido(JsonConvert.SerializeObject(request.serializePedidosList));

        //        if (!result)
        //            return BadRequest($"Nao foi possivel gerar o romaneio.");
        //        else
        //            return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel gerar o romaneio. Erro: {ex.Message}");
        //    }
        //}

        //[HttpGet("GetRomaneioParaImprimir")]
        //public async Task<ActionResult<string>> GetRomaneioParaImprimir([Required][FromQuery] string fileName)
        //{
        //    try
        //    {
        //        var result = await _imprimeRomaneioService.GetRomaneioParaImprimir(fileName);

        //        if (String.IsNullOrEmpty(result))
        //            return BadRequest($"Nao foi possivel encontrar a etiqueta: {fileName}.");
        //        else
        //            return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel encontrar o etiqueta: {fileName}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpGet("GetEmpresas")]
        //public async Task<ActionResult<string>> GetEmpresas()
        //{
        //    try
        //    {
        //        var result = await _configuracoesGlobaisService.GetEmpresas();

        //        if (String.IsNullOrEmpty(result))
        //            return BadRequest($"Nao foi possivel encontrar as empresas no banco de dados.");
        //        else
        //            return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel encontrar as empresas no banco de dados. Erro: {ex.Message}");
        //    }
        //}
    }
}
