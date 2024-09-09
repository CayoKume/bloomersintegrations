using BloomersGeneralIntegrations.AfterSale.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BloomersIntegrationsManager.UI.Controllers.General
{
    [ApiController]
    [Route("NewBloomers/BloomersGeneralJobs/AfterSale")]
    public class AfterSaleController : Controller
    {
        private readonly IAfterSaleService _afterSaleService;

        public AfterSaleController(IAfterSaleService afterSaleService) =>
            (_afterSaleService) = (afterSaleService);

        [HttpPost("GetReversas")]
        public async Task<ActionResult<string>> GetReversas()
        {
            try
            {
                await _afterSaleService.GetReverses();
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
