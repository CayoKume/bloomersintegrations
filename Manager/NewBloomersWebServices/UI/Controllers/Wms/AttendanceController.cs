using BloomersIntegrationsManager.Domain.Entities.MiniWms;
using BloomersMiniWmsIntegrations.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace NewBloomersWebServices.UI.Controllers.Wms
{
    [ApiController]
    [Route("NewBloomers/BloomersInvoiceIntegrations/MiniWms")]
    public class AttendanceController : Controller
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService) =>
            (_attendanceService) = (attendanceService);

        [HttpGet("GetOrdersToContact")]
        public async Task<ActionResult<string>> GetOrdersToContact([Required][FromQuery] string serie, [Required][FromQuery] string doc_company)
        {
            try
            {
                var result = await _attendanceService.GetOrdersToContact(serie, doc_company);

                if (String.IsNullOrEmpty(result))
                    return BadRequest($"Nao foi possivel encontrar os pedidos no banco de dados.");
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel encontrar os pedidos no banco de dados. Erro: {ex.Message}");
            }
        }

        [HttpPut("UpdateDateContacted")]
        public async Task<ActionResult<bool>> UpdateDateContacted([Required][FromBody] UpdateDateContactedRequest request)
        {
            try
            {
                if(await _attendanceService.UpdateDateContacted(request.number, request.atendente, request.obs))
                    return Ok(true);
                else
                    return BadRequest($"Nao foi possivel atualizar a data de contato do pedido na tabela.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel atualizar a data de contato do pedido na tabela. Erro: {ex.Message}");
            }
        }
    }
}
