using BloomersIntegrationsManager.Domain.Entities.MiniWms;
using BloomersMiniWmsIntegrations.Application.Services;
using Microsoft.AspNetCore.Mvc;
using NewBloomersWebServices.Domain.Entities.MiniWms;
using System.ComponentModel.DataAnnotations;

namespace NewBloomersWebServices.UI.Controllers.Wms
{
    [ApiController]
    [Route("NewBloomers/BloomersInvoiceIntegrations/MiniWms")]
    public class LabelsController : Controller
    {
        private readonly ILabelsService _labelsService;

        public LabelsController(ILabelsService labelsService) =>
            (_labelsService) = (labelsService);

        [HttpPost("SendZPLToAPI")]
        public async Task<ActionResult<string>> SendZPLToAPI([FromBody] SendZPLToAPIRequest request)
        {
            try
            {
                var result = await _labelsService.SendZPLToAPI(request.zpl, request.nr_pedido, request.volumes);

                if (!result)
                    return BadRequest($"Nao foi possivel gerar a etiqueta do pedido: {request.nr_pedido}.");
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel gerar a etiqueta do pedido: {request.nr_pedido}. Erro: {ex.Message}");
            }
        }

        [HttpGet("GetLabelToPrint")]
        public async Task<ActionResult<string>> GetLabelToPrint([Required][FromQuery] string fileName)
        {
            try
            {
                var result = await _labelsService.GetLabelToPrint(fileName);

                if (String.IsNullOrEmpty(result))
                    return BadRequest($"Nao foi possivel encontrar a etiqueta: {fileName}.");
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel encontrar o etiqueta: {fileName}. Erro: {ex.Message}");
            }
        }

        [HttpGet("GetOrderToPrint")]
        public async Task<ActionResult<string>> GetOrderToPrint([Required][FromQuery] string cnpj_emp, [Required][FromQuery] string serie, [Required][FromQuery] string nr_pedido)
        {
            try
            {
                var result = await _labelsService.GetOrderToPrint(cnpj_emp, serie, nr_pedido);

                if (String.IsNullOrEmpty(result))
                    return BadRequest($"Nao foi possivel encontrar o pedido: {nr_pedido}.");
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel encontrar o pedido: {nr_pedido}. Erro: {ex.Message}");
            }
        }

        [HttpGet("GetOrdersToPrint")]
        public async Task<ActionResult<string>> GetOrdersToPrint([Required][FromQuery] string cnpj_emp, [Required][FromQuery] string serie, [Required][FromQuery] string data_inicial, [Required][FromQuery] string data_final)
        {
            try
            {
                var result = await _labelsService.GetOrdersToPrint(cnpj_emp, serie, data_inicial, data_final);

                if (String.IsNullOrEmpty(result))
                    return BadRequest($"Nao foi possivel encontrar os pedidos para o intervalo de datas determinado.");
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel encontrar os pedidos para o intervalo de datas determinado. Erro: {ex.Message}");
            }
        }

        [HttpGet("GetOrderToPresent")]
        public async Task<ActionResult<string>> GetOrderToPresent([Required][FromQuery] string cnpj_emp, [Required][FromQuery] string serie, [Required][FromQuery] string nr_pedido)
        {
            try
            {
                var result = await _labelsService.GetOrdersToPresent(cnpj_emp, serie, nr_pedido);

                if (result is null)
                    return BadRequest($"Nao foi possivel gerar o cupom de separação.");
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel gerar o cupom de separação. Erro: {ex.Message}");
            }
        }

        [HttpPut("UpdateFlagPrinted")]
        public async Task<ActionResult> UpdateFlagPrinted([Required][FromQuery] string nr_pedido)
        {
            try
            {
                var result = await _labelsService.UpdateFlagPrinted(nr_pedido);

                if (!result)
                    return BadRequest($"Nao foi possivel atualizar o retorno do pedido na tabela.");
                else
                    return Ok();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel atualizar o retorno do pedido na tabela. Erro: {ex.Message}");
            }
        }

        [HttpPost("PrintExchangeCupoun")]
        public async Task<ActionResult<string>> PrintExchangeCupoun([FromBody] PrintOrderToPresentRequest request)
        {
            try
            {
                var result = await _labelsService.PrintExchangeCupoun(System.Text.Json.JsonSerializer.Serialize(request.serializePedido));

                if (result is null)
                    return BadRequest($"Nao foi possivel gerar o cupom de troca.");
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel gerar o cupom de troca. Erro: {ex.Message}");
            }
        }
    }
}
