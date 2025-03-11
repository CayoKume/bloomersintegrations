using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxCommerce;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxEcommerce;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BloomersIntegrationsManager.UI.Controllers.LinxMicrovix
{
    [ApiController]
    [Route("NewBloomers/BloomersMicrovixJobs/LinxEcommerce")]
    public class LinxMicrovixB2CController : Controller
    {
        private readonly IB2CConsultaClientesServices<B2CConsultaClientes> _b2CConsultaClientesService;
        private readonly IB2CConsultaNFeService<B2CConsultaNFe> _b2CConsultaNFeService;
        private readonly IB2CConsultaNFeSituacaoService<B2CConsultaNFeSituacao> _b2CConsultaNFeSituacaoService;
        private readonly IB2CConsultaPedidosService<B2CConsultaPedidos> _b2CConsultaPedidosService;
        private readonly IB2CConsultaPedidosItensService<B2CConsultaPedidosItens> _b2CConsultaPedidosItensService;
        private readonly IB2CConsultaPedidosStatusService<B2CConsultaPedidosStatus> _b2CConsultaPedidosStatusService;
        private readonly IB2CConsultaStatusService<B2CConsultaStatus> _b2CConsultaStatusService;

        public LinxMicrovixB2CController
            (
                 IB2CConsultaClientesServices<B2CConsultaClientes> b2CConsultaClientesService,
                 IB2CConsultaNFeService<B2CConsultaNFe> b2CConsultaNFeService,
                 IB2CConsultaNFeSituacaoService<B2CConsultaNFeSituacao> b2CConsultaNFeSituacaoService,
                 IB2CConsultaPedidosService<B2CConsultaPedidos> b2CConsultaPedidosService,
                 IB2CConsultaPedidosItensService<B2CConsultaPedidosItens> b2CConsultaPedidosItensService,
                 IB2CConsultaPedidosStatusService<B2CConsultaPedidosStatus> b2CConsultaPedidosStatusService,
                 IB2CConsultaStatusService<B2CConsultaStatus> b2CConsultaStatusService
            ) 
            =>
            (
                 _b2CConsultaClientesService,
                 _b2CConsultaNFeService,
                 _b2CConsultaNFeSituacaoService,
                 _b2CConsultaPedidosService,
                 _b2CConsultaPedidosItensService,
                 _b2CConsultaPedidosStatusService,
                 _b2CConsultaStatusService
            ) 
            =
            (
                 b2CConsultaClientesService,
                 b2CConsultaNFeService,
                 b2CConsultaNFeSituacaoService,
                 b2CConsultaPedidosService,
                 b2CConsultaPedidosItensService,
                 b2CConsultaPedidosStatusService,
                 b2CConsultaStatusService
            );

        [HttpPost("B2CConsultaClientes")]
        public async Task<ActionResult> IntegraClienteIndividual([Required][FromQuery] string doc_client)
        {
            try
            {
                await _b2CConsultaClientesService.IntegraRegistrosAsync(
                        "B2CConsultaClientes",
                        "p_B2CConsultaClientes_Sincronizacao",
                        LinxAPIAttributes.TypeEnum.Producao.ToName()
                    );

                var result = await _b2CConsultaClientesService.IntegraRegistrosIndividualAsync(
                        "B2CConsultaClientes",
                        "p_B2CConsultaClientes_Sincronizacao",
                        LinxAPIAttributes.TypeEnum.Producao.ToName(),
                        doc_client
                    );

                if (result != true)
                    return BadRequest($"A API B2CConsultaClientes não encontrou o cliente: {doc_client}.");
                else
                    return Ok($"Cliente: {doc_client} integrado com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar o cliente: {doc_client}. Erro: {ex.Message}");
            }
        }

        [HttpPost("B2CConsultaNFe")]
        public async Task<ActionResult> IntegraNFeIndividual([Required][FromQuery] string order_id)
        {
            try
            {
                var result = await _b2CConsultaNFeService.IntegraRegistrosIndividualAsync(
                        "B2CConsultaNFe",
                        "p_B2CConsultaNFe_Sincronizacao",
                        LinxAPIAttributes.TypeEnum.Producao.ToName(),
                        order_id.ToString()
                    );

                if (result != true)
                    return BadRequest($"A API B2CConsultaNFe não encontrou a Nota Fiscal do pedido: {order_id}.");
                else
                    return Ok($"Nota Fiscal do pedido: {order_id} integrado com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar a Nota Fiscal do pedido: {order_id}. Erro: {ex.Message}");
            }
        }

        [HttpPost("B2CConsultaNFeSituacao")]
        public async Task<ActionResult> IntegraNFeSituacao()
        {
            try
            {
                await _b2CConsultaNFeSituacaoService.IntegraRegistrosAsync(
                        "B2CConsultaNFeSituacao",
                        "p_B2CConsultaNFeSituacao_Sincronizacao",
                        LinxAPIAttributes.TypeEnum.Producao.ToName()
                    );

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("B2CConsultaPedidosItens")]
        public async Task<ActionResult> IntegraPedidosItensIndividual([Required][FromQuery] string order_id)
        {
            try
            {
                var result = await _b2CConsultaPedidosItensService.IntegraRegistrosIndividualAsync(
                        "B2CConsultaPedidosItens",
                        "p_B2CConsultaPedidosItens_Sincronizacao",
                        LinxAPIAttributes.TypeEnum.Producao.ToName(),
                        order_id.ToString()
                    );

                if (result != true)
                    return BadRequest($"A API B2CConsultaPedidosItens não encontrou os itens do pedido: {order_id}.");
                else
                    return Ok($"Itens do pedido: {order_id} integrados com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar os itens do pedido: {order_id}. Erro: {ex.Message}");
            }
        }

        [HttpPost("B2CConsultaPedidos")]
        public async Task<ActionResult> IntegraPedidosIndividual([Required][FromQuery] string order_id)
        {
            try
            {
                await _b2CConsultaPedidosService.IntegraRegistrosAsync(
                        "B2CConsultaPedidos",
                        "p_B2CConsultaPedidos_Sincronizacao",
                        LinxAPIAttributes.TypeEnum.Producao.ToName()
                    );

                var result = await _b2CConsultaPedidosService.IntegraRegistrosIndividualAsync(
                        "B2CConsultaPedidos",
                        "p_B2CConsultaPedidos_Sincronizacao",
                        LinxAPIAttributes.TypeEnum.Producao.ToName(),
                        order_id.ToString()
                    );

                if (result != true)
                    return BadRequest($"A API B2CConsultaPedidos não encontrou o pedido: {order_id}.");
                else
                    return Ok($"O pedido: {order_id} foi integrado com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar o pedido: {order_id}. Erro: {ex.Message}");
            }
        }

        [HttpPost("B2CConsultaPedidosStatus")]
        public async Task<ActionResult> IntegraPedidosStatusIndividual([Required][FromQuery] string order_id)
        {
            try
            {
                var result = await _b2CConsultaPedidosStatusService.IntegraRegistrosIndividualAsync(
                        "B2CConsultaPedidosStatus",
                        "p_B2CConsultaPedidosStatus_Sincronizacao",
                        LinxAPIAttributes.TypeEnum.Producao.ToName(),
                        order_id.ToString()
                    );

                if (result != true)
                    return BadRequest($"A API B2CConsultaPedidosStatus não encontrou os status do pedido: {order_id}.");
                else
                    return Ok($"O pedido: {order_id} foi integrado com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar os status do pedido: {order_id}. Erro: {ex.Message}");
            }
        }

        [HttpPost("B2CConsultaStatus")]
        public async Task<ActionResult> IntegraStatus()
        {
            try
            {
                await _b2CConsultaStatusService.IntegraRegistrosAsync(
                        "B2CConsultaStatus",
                        "p_B2CConsultaStatus_Sincronizacao",
                        LinxAPIAttributes.TypeEnum.Producao.ToName()
                    );

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
