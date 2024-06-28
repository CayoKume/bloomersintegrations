using BloomersMiniWmsIntegrations.Application.Services;
using Microsoft.AspNetCore.Mvc;
using BloomersIntegrationsManager.Domain.Entities.MiniWms;

namespace NewBloomersWebServices.UI.Controllers.Wms
{
    [ApiController]
    [Route("NewBloomers/BloomersInvoiceIntegrations/MiniWms")]
    public class CancellationRequestController : Controller
    {
        private readonly ICancellationRequestService _cancellationRequestService;

        public CancellationRequestController(ICancellationRequestService cancellationRequestService) =>
            (_cancellationRequestService) = (cancellationRequestService);

        [HttpPost("CreateCancellationRequest")]
        public async Task<ActionResult> CreateCancellationRequest([FromBody] OrderToCancellationRequest request)
        {
            try
            {
                await _cancellationRequestService.CreateCancellationRequest(System.Text.Json.JsonSerializer.Serialize(request.serializeOrder));

                return Ok();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel gerar o cupom de troca. Erro: {ex.Message}");
            }
        }

        [HttpGet("GetReasons")]
        public async Task<ActionResult<string>> GetReasons()
        {
            try
            {
                var result = await _cancellationRequestService.GetReasons();

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
    }
}
