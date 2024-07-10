using BloomersIntegrationsManager.Domain.Entities.MiniWms;
using BloomersMiniWmsIntegrations.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace NewBloomersWebServices.UI.Controllers.Wms
{
    [ApiController]
    [Route("NewBloomers/BloomersInvoiceIntegrations/MiniWms")]
    public class ExecuteCancellationController : Controller
    {
        private readonly IExecuteCancellationService _executeCancellationService;

        public ExecuteCancellationController(IExecuteCancellationService executeCancellationService) =>
            (_executeCancellationService) = (executeCancellationService);

        [HttpPut("UpdateDateCanceled")]
        public async Task<ActionResult<bool>> UpdateDateCanceled([Required][FromBody] UpdateDateCanceledRequest request)
        {
            try
            {
                if (await _executeCancellationService.UpdateDateCanceled(request.number, request.suporte, request.obs, request.motivo))
                    return Ok(true);
                else
                    return BadRequest($"Nao foi possivel atualizar a data de cancelamento do pedido na tabela.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel atualizar a data de cancelamento do pedido na tabela. Erro: {ex.Message}");
            }
        }

        [HttpGet("GetReasonsToExecuteCancellation")]
        public async Task<ActionResult<string>> GetReasons()
        {
            try
            {
                var result = await _executeCancellationService.GetReasons();

                if (String.IsNullOrEmpty(result))
                    return BadRequest($"Nao foi possivel encontrar os motivos no banco de dados.");
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel encontrar os motivos no banco de dados. Erro: {ex.Message}");
            }
        }

        [HttpGet("GetOrdersToCancel")]
        public async Task<ActionResult<string>> GetOrdersToCancel([Required][FromQuery] string serie, [Required][FromQuery] string doc_company)
        {
            try
            {
                var result = await _executeCancellationService.GetOrdersToCancel(serie, doc_company);

                if (String.IsNullOrEmpty(result))
                    return BadRequest($"Nao foi possivel encontrar o pedido no banco de dados.");
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel encontrar o pedido no banco de dados. Erro: {ex.Message}");
            }
        }
    }
}
