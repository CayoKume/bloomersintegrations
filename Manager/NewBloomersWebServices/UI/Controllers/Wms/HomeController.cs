using BloomersMiniWmsIntegrations.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace NewBloomersWebServices.UI.Controllers.Wms
{
    [ApiController]
    [Route("NewBloomers/BloomersInvoiceIntegrations/MiniWms")]
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService) =>
            (_homeService) = (homeService);

        [HttpGet("GetPickupOrders")]
        public async Task<ActionResult<string>> GetPickupOrders([Required][FromQuery] string cnpj_emp)
        {
            try
            {
                var result = await _homeService.GetPickupOrders(cnpj_emp);

                if (String.IsNullOrEmpty(result))
                    return BadRequest($"Nao foi possivel encontrar as empresas no banco de dados.");
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel encontrar as empresas no banco de dados. Erro: {ex.Message}");
            }
        }
    }
}
