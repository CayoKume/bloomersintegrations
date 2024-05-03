using BloomersIntegrationsManager.Domain.Entities.MiniWms;
using BloomersMiniWmsIntegrations.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace NewBloomersWebServices.UI.Controllers.Wms
{
    [ApiController]
    [Route("NewBloomers/BloomersInvoiceIntegrations/MiniWms")]
    public class DeliveryListController : Controller
    {
        private readonly IDeliveryListService _deliveryListService;

        public DeliveryListController(IDeliveryListService deliveryListService) =>
            (_deliveryListService) = (deliveryListService);

        [HttpGet("GetOrderShipped")]
        public async Task<ActionResult<string>> GetOrderShipped([Required][FromQuery] string cnpj_emp, [Required][FromQuery] string serie, [Required][FromQuery] string nr_pedido, [Required][FromQuery] string cod_transportadora)
        {
            try
            {
                var result = await _deliveryListService.GetOrderShipped(nr_pedido, serie, cnpj_emp, cod_transportadora);

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

        [HttpGet("GetOrdersShipped")]
        public async Task<ActionResult<string>> GetOrdersShipped([Required][FromQuery] string cod_transportadora, [Required][FromQuery] string cnpj_emp, [Required][FromQuery] string serie, [Required][FromQuery] string data_inicial, [Required][FromQuery] string data_final)
        {
            try
            {
                var result = await _deliveryListService.GetOrdersShipped(cod_transportadora, cnpj_emp, serie, data_inicial, data_final);

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

        [HttpPost("PrintDeliveryList")]
        public async Task<ActionResult<string>> PrintDeliveryList([FromBody] PrintOrderRequest request)
        {
            try
            {
                var result = await _deliveryListService.PrintOrder(JsonConvert.SerializeObject(request.serializePedidosList));

                if (!result)
                    return BadRequest($"Nao foi possivel gerar o romaneio.");
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel gerar o romaneio. Erro: {ex.Message}");
            }
        }

        [HttpGet("GetDeliveryListToPrint")]
        public async Task<ActionResult<string>> GetDeliveryListToPrint([Required][FromQuery] string fileName)
        {
            try
            {
                var result = await _deliveryListService.GetDeliveryListToPrint(fileName);

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
    }
}
