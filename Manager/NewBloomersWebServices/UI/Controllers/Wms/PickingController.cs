using BloomersIntegrationsManager.Domain.Entities.MiniWms;
using BloomersMiniWmsIntegrations.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace NewBloomersWebServices.UI.Controllers.Wms
{
    [ApiController]
    [Route("NewBloomers/BloomersInvoiceIntegrations/MiniWms")]
    public class PickingController : Controller
    {
        private readonly IPickingService _pickingService;

        public PickingController(IPickingService pickingService) =>
            (_pickingService) = (pickingService);

        [HttpGet("GetShippingCompanys")]
        public async Task<ActionResult<string>> GetShippingCompanys()
        {
            try
            {
                var result = await _pickingService.GetShippingCompanys();

                if (String.IsNullOrEmpty(result))
                    return BadRequest($"Nao foi possivel obter as transportadoras da tabela LinxClientesFornec.");
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel obter as transportadoras da tabela LinxClientesFornec. Erro: {ex.Message}");
            }
        }

        [HttpGet("GetUnpickedOrder")]
        public async Task<ActionResult<string>> GetUnpickedOrder([Required][FromQuery] string cnpj_emp, [Required][FromQuery] string serie, [Required][FromQuery] string nr_pedido)
        {
            try
            {
                var result = await _pickingService.GetUnpickedOrder(cnpj_emp, serie, nr_pedido);

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

        [HttpGet("GetUnpickedOrders")]
        public async Task<ActionResult<string>> GetUnpickedOrders([Required][FromQuery] string cnpj_emp, [Required][FromQuery] string serie, [Required][FromQuery] string data_inicial, [Required][FromQuery] string data_final)
        {
            try
            {
                var result = await _pickingService.GetUnpickedOrders(cnpj_emp, serie, data_inicial, data_final);

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


        [HttpGet("GetUnpickedOrderToPrint")]
        public async Task<ActionResult<string>> GetUnpickedOrderToPrint([Required][FromQuery] string cnpj_emp, [Required][FromQuery] string serie, [Required][FromQuery] string nr_pedido)
        {
            try
            {
                var result = await _pickingService.GetUnpickedOrderToPrint(cnpj_emp, serie, nr_pedido);

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

        [HttpGet("GetUnpickedOrdersToPrint")]
        public async Task<ActionResult<string>> GetUnpickedOrdersToPrint([Required][FromQuery] string cnpj_emp, [Required][FromQuery] string serie, [Required][FromQuery] string data_inicial, [Required][FromQuery] string data_final)
        {
            try
            {
                var result = await _pickingService.GetUnpickedOrdersToPrint(cnpj_emp, serie, data_inicial, data_final);

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

        [HttpPost("UpdateRetorno")]
        public async Task<ActionResult> UpdateRetorno([FromBody] UpdateRetornoRequest request)
        {
            try
            {
                var result = await _pickingService.UpdateRetorno(request.nr_pedido, request.volumes, JsonConvert.SerializeObject(request.itens));

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

        [HttpPost("UpdateShippingCompany")]
        public async Task<ActionResult> UpdateShippingCompany([FromBody] UpdateShippingCompanyRequest request)
        {
            try
            {
                var result = await _pickingService.UpdateShippingCompany(request.orderNumber, request.cod_shippingCompany);

                if (!result)
                    return BadRequest($"Nao foi possivel atualizar a transportadora do pedido: {request.orderNumber} para {request.cod_shippingCompany}.");
                else
                    return Ok();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel atualizar a transportadora do pedido: {request.orderNumber} para {request.cod_shippingCompany}. Erro: {ex.Message}");
            }
        }

        [HttpPost("PrintOrderToCupoun")]
        public async Task<ActionResult<string>> PrintOrderToCupoun([FromBody] PrintOrderRequest request)
        {
            try
            {
                var result = await _pickingService.PrintOrderToCupoun(JsonConvert.SerializeObject(request.serializePedido));

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
    }
}
