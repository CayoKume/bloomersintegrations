using Microsoft.AspNetCore.Mvc;

namespace BloomersIntegrationsManager.Controllers
{
    [ApiController]
    [Route("NewBloomers/BloomersMicrovixIntegrations/Ecommerce")]
    public class BloomersMicrovixEcommerceJobsManagerController : Controller
    {
        //private readonly IB2CConsultaClientesServices<B2CConsultaClientes> _b2CConsultaClientesService;
        //private readonly IB2CConsultaNFeService<B2CConsultaNFe> _b2CConsultaNFeService;
        //private readonly IB2CConsultaNFeSituacaoService<B2CConsultaNFeSituacao> _b2CConsultaNFeSituacaoService;
        //private readonly IB2CConsultaPedidosService<B2CConsultaPedidos> _b2CConsultaPedidosService;
        //private readonly IB2CConsultaPedidosItensService<B2CConsultaPedidosItens> _b2CConsultaPedidosItensService;
        //private readonly IB2CConsultaPedidosStatusService<B2CConsultaPedidosStatus> _b2CConsultaPedidosStatusService;
        //private readonly IB2CConsultaStatusService<B2CConsultaStatus> _b2CConsultaStatusService;

        //public BloomersMicrovixEcommerceJobsManagerController
        //    (IB2CConsultaClientesServices<B2CConsultaClientes> b2CConsultaClientesService,
        //     IB2CConsultaNFeService<B2CConsultaNFe> b2CConsultaNFeService,
        //     IB2CConsultaNFeSituacaoService<B2CConsultaNFeSituacao> b2CConsultaNFeSituacaoService,
        //     IB2CConsultaPedidosService<B2CConsultaPedidos> b2CConsultaPedidosService,
        //     IB2CConsultaPedidosItensService<B2CConsultaPedidosItens> b2CConsultaPedidosItensService,
        //     IB2CConsultaPedidosStatusService<B2CConsultaPedidosStatus> b2CConsultaPedidosStatusService,
        //     IB2CConsultaStatusService<B2CConsultaStatus> b2CConsultaStatusService
        //    ) =>
        //    (_b2CConsultaClientesService,
        //     _b2CConsultaNFeService,
        //     _b2CConsultaNFeSituacaoService,
        //     _b2CConsultaPedidosService,
        //     _b2CConsultaPedidosItensService,
        //     _b2CConsultaPedidosStatusService,
        //     _b2CConsultaStatusService
        //    ) =
        //    (b2CConsultaClientesService,
        //     b2CConsultaNFeService,
        //     b2CConsultaNFeSituacaoService,
        //     b2CConsultaPedidosService,
        //     b2CConsultaPedidosItensService,
        //     b2CConsultaPedidosStatusService,
        //     b2CConsultaStatusService
        //    );
        
        //[HttpPost("B2CConsultaClientes")]
        //public async Task<ActionResult> IntegraClienteIndividual([FromBody] B2CConsultaClientesRequest request)
        //{
        //    try
        //    {
        //        var result = await _b2CConsultaClientesService.IntegraRegistrosIndividual(
        //                "B2CConsultaClientes",
        //                "p_B2CConsultaClientes_Sincronizacao",
        //                LinxAPIAttributes.TypeEnum.Producao.ToName(),
        //                request.doc_cliente
        //            );

        //        if (result != true)
        //            return BadRequest($"A API B2CConsultaClientes não encontrou o cliente: {request.doc_cliente}.");
        //        else
        //            return Ok($"Cliente: {request.doc_cliente} integrado com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel integrar o cliente: {request.doc_cliente}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("B2CConsultaNFe")]
        //public async Task<ActionResult> IntegraNFeIndividual([FromBody] B2CConsultaNFeRequest request)
        //{
        //    try
        //    {
        //        var result = await _b2CConsultaNFeService.IntegraRegistrosIndividual(
        //                "B2CConsultaNFe",
        //                "p_B2CConsultaNFe_Sincronizacao",
        //                LinxAPIAttributes.TypeEnum.Producao.ToName(),
        //                request.id_pedido.ToString()
        //            );

        //        if (result != true)
        //            return BadRequest($"A API B2CConsultaNFe não encontrou a Nota Fiscal do pedido: {request.id_pedido}.");
        //        else
        //            return Ok($"Nota Fiscal do pedido: {request.id_pedido} integrado com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel integrar a Nota Fiscal do pedido: {request.id_pedido}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("B2CConsultaNFeSituacao")]
        //public async Task<ActionResult> IntegraNFeSituacao()
        //{
        //    try
        //    {
        //        await _b2CConsultaNFeSituacaoService.IntegraRegistros(
        //                "B2CConsultaNFeSituacao",
        //                "p_B2CConsultaNFeSituacao_Sincronizacao",
        //                LinxAPIAttributes.TypeEnum.Producao.ToName()
        //            );

        //        return Ok();
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}

        //[HttpPost("B2CConsultaPedidosItens")]
        //public async Task<ActionResult> IntegraPedidosItensIndividual([FromBody] B2CConsultaPedidosItensRequest request)
        //{
        //    try
        //    {
        //        var result = await _b2CConsultaPedidosItensService.IntegraRegistrosIndividual(
        //                "B2CConsultaPedidosItens",
        //                "p_B2CConsultaPedidosItens_Sincronizacao",
        //                LinxAPIAttributes.TypeEnum.Producao.ToName(),
        //                request.id_pedido.ToString()
        //            );

        //        if (result != true)
        //            return BadRequest($"A API B2CConsultaPedidosItens não encontrou os itens do pedido: {request.id_pedido}.");
        //        else
        //            return Ok($"Itens do pedido: {request.id_pedido} integrados com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel integrar os itens do pedido: {request.id_pedido}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("B2CConsultaPedidos")]
        //public async Task<ActionResult> IntegraPedidosIndividual([FromBody] B2CConsultaPedidosRequest request)
        //{
        //    try
        //    {
        //        var result = await _b2CConsultaPedidosService.IntegraRegistrosIndividual(
        //                "B2CConsultaPedidos",
        //                "p_B2CConsultaPedidos_Sincronizacao",
        //                LinxAPIAttributes.TypeEnum.Producao.ToName(),
        //                request.id_pedido.ToString()
        //            );

        //        if (result != true)
        //            return BadRequest($"A API B2CConsultaPedidos não encontrou o pedido: {request.id_pedido}.");
        //        else
        //            return Ok($"O pedido: {request.id_pedido} foi integrado com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel integrar o pedido: {request.id_pedido}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("B2CConsultaPedidosStatus")]
        //public async Task<ActionResult> IntegraPedidosStatusIndividual([FromBody] B2CConsultaPedidosRequest request)
        //{
        //    try
        //    {
        //        var result = await _b2CConsultaPedidosStatusService.IntegraRegistrosIndividual(
        //                "B2CConsultaPedidosStatus",
        //                "p_B2CConsultaPedidosStatus_Sincronizacao",
        //                LinxAPIAttributes.TypeEnum.Producao.ToName(),
        //                request.id_pedido.ToString()
        //            );

        //        if (result != true)
        //            return BadRequest($"A API B2CConsultaPedidosStatus não encontrou os status do pedido: {request.id_pedido}.");
        //        else
        //            return Ok($"O pedido: {request.id_pedido} foi integrado com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel integrar os status do pedido: {request.id_pedido}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("B2CConsultaStatus")]
        //public async Task<ActionResult> IntegraStatus()
        //{
        //    try
        //    {
        //        await _b2CConsultaStatusService.IntegraRegistros(
        //                "B2CConsultaStatus",
        //                "p_B2CConsultaStatus_Sincronizacao",
        //                LinxAPIAttributes.TypeEnum.Producao.ToName()
        //            );

        //        return Ok();
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}
    }
}
