using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Enums;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BloomersIntegrationsManager.UI.Controllers.LinxMicrovix
{
    [ApiController]
    [Route("NewBloomers/BloomersMicrovixJobs/LinxMicrovix")]
    public class LinxMicrovixERPController : Controller
    {
        private readonly ILinxClientesFornecService<LinxClientesFornec> _linxClientesFornecService;
        private readonly ILinxMovimentoCartoesService<LinxMovimentoCartoes> _linxMovimentoCartoesService;
        private readonly ILinxMovimentoService<LinxMovimento> _linxMovimentoService;
        private readonly ILinxPedidosCompraService<LinxPedidosCompra> _linxPedidosCompraService;
        private readonly ILinxPedidosVendaService<LinxPedidosVenda> _linxPedidosVendaService;
        private readonly ILinxProdutosService<LinxProdutos> _linxProdutosService;
        private readonly ILinxProdutosCamposAdicionaisService<LinxProdutosCamposAdicionais> _linxProdutosCamposAdicionaisService;
        private readonly ILinxProdutosDepositosService<LinxProdutosDepositos> _linxProdutosDepositosService;
        private readonly ILinxProdutosDetalhesService<LinxProdutosDetalhes> _linxProdutosDetalhes;
        private readonly ILinxProdutosInventarioService<LinxProdutosInventario> _linxProdutosInventarioService;
        private readonly ILinxProdutosPromocoesService<LinxProdutosPromocoes> _linxProdutosPromocoesService;
        private readonly ILinxProdutosTabelasService<LinxProdutosTabelas> _linxProdutosTabelasService;
        private readonly ILinxProdutosTabelasPrecosService<LinxProdutosTabelasPrecos> _linxProdutosTabelasPrecosService;
        private readonly ILinxXMLDocumentosService<LinxXMLDocumentos> _linxXMLDocumentosService;
        private readonly ILinxPlanosService<LinxPlanos> _linxPlanosService;
        private readonly ILinxGrupoLojasService<LinxGrupoLojas> _linxGrupoLojasService;
        private readonly ILinxMovimentoPlanosService<LinxMovimentoPlanos> _linxMovimentoPlanosService;

        public LinxMicrovixERPController
            (
                ILinxClientesFornecService<LinxClientesFornec> linxClientesFornecService,
                ILinxMovimentoCartoesService<LinxMovimentoCartoes> linxMovimentoCartoesService,
                ILinxMovimentoService<LinxMovimento> linxMovimentoService,
                ILinxPedidosCompraService<LinxPedidosCompra> linxPedidosCompraService,
                ILinxPedidosVendaService<LinxPedidosVenda> linxPedidosVendaService,
                ILinxProdutosService<LinxProdutos> linxProdutosService,
                ILinxProdutosCamposAdicionaisService<LinxProdutosCamposAdicionais> linxProdutosCamposAdicionaisService,
                ILinxProdutosDepositosService<LinxProdutosDepositos> linxProdutosDepositosService,
                ILinxProdutosDetalhesService<LinxProdutosDetalhes> linxProdutosDetalhes,
                ILinxProdutosInventarioService<LinxProdutosInventario> linxProdutosInventarioService,
                ILinxProdutosPromocoesService<LinxProdutosPromocoes> linxProdutosPromocoesService,
                ILinxProdutosTabelasService<LinxProdutosTabelas> linxProdutosTabelasService,
                ILinxProdutosTabelasPrecosService<LinxProdutosTabelasPrecos> linxProdutosTabelasPrecosService,
                ILinxXMLDocumentosService<LinxXMLDocumentos> linxXMLDocumentosService,
                ILinxPlanosService<LinxPlanos> linxPlanosService,
                ILinxGrupoLojasService<LinxGrupoLojas> linxGrupoLojasService,
                ILinxMovimentoPlanosService<LinxMovimentoPlanos> linxMovimentoPlanosService
            )
            =>
            (
                _linxClientesFornecService,
                _linxMovimentoCartoesService,
                _linxMovimentoService,
                _linxPedidosCompraService,
                _linxPedidosVendaService,
                _linxProdutosService,
                _linxProdutosCamposAdicionaisService,
                _linxProdutosDepositosService,
                _linxProdutosDetalhes,
                _linxProdutosInventarioService,
                _linxProdutosPromocoesService,
                _linxProdutosTabelasService,
                _linxProdutosTabelasPrecosService,
                _linxXMLDocumentosService,
                _linxPlanosService,
                _linxGrupoLojasService,
                _linxMovimentoPlanosService
            ) =
            (
                linxClientesFornecService,
                linxMovimentoCartoesService,
                linxMovimentoService,
                linxPedidosCompraService,
                linxPedidosVendaService,
                linxProdutosService,
                linxProdutosCamposAdicionaisService,
                linxProdutosDepositosService,
                linxProdutosDetalhes,
                linxProdutosInventarioService,
                linxProdutosPromocoesService,
                linxProdutosTabelasService,
                linxProdutosTabelasPrecosService,
                linxXMLDocumentosService,
                linxPlanosService,
                linxGrupoLojasService,
                linxMovimentoPlanosService
            );

        [HttpPost("LinxClientesFornec")]
        public async Task<ActionResult> IntegraClientesFornecIndividual([Required][FromQuery] string doc_client)
        {
            try
            {
                var result = await _linxClientesFornecService.IntegraRegistrosIndividualAsync(
                        "LinxClientesFornec",
                        "p_LinxClientesFornec_Sincronizacao",
                        LinxAPIAttributes.TypeEnum.Producao.ToName(),
                        doc_client
                    );

                if (result != true)
                    return BadRequest($"A API LinxClientesFornec não encontrou o cliente: {doc_client}.");
                else
                    return Ok($"Cliente: {doc_client} integrado com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar o cliente: {doc_client}. Erro: {ex.Message}");
            }
        }

        [HttpPost("LinxMovimentoCartoes")]
        public async Task<ActionResult> IntegraMovimentoCartoes()
        {
            try
            {
                await _linxMovimentoCartoesService.IntegraRegistrosAsync(
                        "LinxMovimentoCartoes",
                        "p_LinxMovimentoCartoes_Sincronizacao",
                        LinxAPIAttributes.TypeEnum.Producao.ToName()
                    );

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("LinxMovimento")]
        public async Task<ActionResult> IntegraMovimentoIndividual([Required][FromQuery] string document, [Required][FromQuery] string doc_company)
        {
            try
            {
                var result = await _linxMovimentoService.IntegraRegistrosIndividualAsync(
                        "LinxMovimento",
                        "p_LinxMovimento_trusted_unificado",
                        LinxAPIAttributes.TypeEnum.Producao.ToName(),
                        document,
                        doc_company
                    );

                if (result != true)
                    return BadRequest($"A API LinxMovimento não encontrou o documento: {document}, da empresa: {doc_company}.");
                else
                    return Ok($"Documento: {document}, da empresa: {doc_company} integrado com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar o documento: {document}, da empresa: {doc_company}. Erro: {ex.Message}");
            }
        }

        [HttpPost("LinxPedidosCompra")]
        public async Task<ActionResult> IntegraPedidosCompra()
        {
            try
            {
                await _linxPedidosCompraService.IntegraRegistrosAsync(
                        "LinxPedidosCompra",
                        "p_LinxPedidosCompra_Sincronizacao",
                        LinxAPIAttributes.TypeEnum.Producao.ToName()
                    );

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("LinxPedidosVenda")]
        public async Task<ActionResult> IntegraPedidosVendaIndividual([Required][FromQuery] string cod_order, [Required][FromQuery] string doc_company)
        {
            try
            {
                var result = await _linxPedidosVendaService.IntegraRegistrosIndividualAsync(
                        "LinxPedidosVenda",
                        "p_LinxPedidosVenda_trusted_unificado",
                        LinxAPIAttributes.TypeEnum.Producao.ToName(),
                        cod_order,
                        doc_company
                    );

                if (result != true)
                    return BadRequest($"A API LinxPedidosVenda não encontrou o pedido: {cod_order}, da empresa: {doc_company}.");
                else
                    return Ok($"Pedido: {cod_order}, da empresa: {doc_company} integrado com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar o pedido: {cod_order}, da empresa: {doc_company}. Erro: {ex.Message}");
            }
        }

        [HttpPost("LinxProdutos")]
        public async Task<ActionResult> IntegraProdutosIndividual([Required][FromQuery] string cod_product, [Required][FromQuery] string doc_company)
        {
            try
            {
                var result = await _linxProdutosService.IntegraRegistrosIndividualAsync(
                        "LinxProdutos",
                        "p_LinxProdutos_Sincronizacao",
                        LinxAPIAttributes.TypeEnum.Producao.ToName(),
                        cod_product,
                        doc_company
                    );

                if (result != true)
                    return BadRequest($"A API LinxProdutos não encontrou os dados do produto: {cod_product}, da empresa: {doc_company}.");
                else
                    return Ok($"Produto: {cod_product}, da empresa: {doc_company} integrados com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar o produto: {cod_product}, da empresa: {doc_company}. Erro: {ex.Message}");
            }
        }

        [HttpPost("LinxProdutosPromocoes")]
        public async Task<ActionResult> IntegraProdutosPromocoes()
        {
            try
            {
                await _linxProdutosPromocoesService.IntegraRegistrosAsync(
                        "LinxProdutosPromocoes",
                        "p_LinxProdutosPromocoes_Sincronizacao",
                        LinxAPIAttributes.TypeEnum.Producao.ToName()
                    );

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("LinxProdutosDepositos")]
        public async Task<ActionResult> IntegraProdutosDepositosIndividual([Required][FromQuery] string cod_deposit, [Required][FromQuery] string doc_company)
        {
            try
            {
                var result = await _linxProdutosDepositosService.IntegraRegistrosIndividualAsync(
                        "LinxProdutosDepositos",
                        "p_LinxProdutosDepositos_Sincronizacao",
                        LinxAPIAttributes.TypeEnum.Producao.ToName(),
                        cod_deposit,
                        doc_company
                    );

                if (result != true)
                    return BadRequest($"A API LinxProdutosDepositos não encontrou o deposito: {cod_deposit}, da empresa: {doc_company}.");
                else
                    return Ok($"Código Depósito: {cod_deposit}, da empresa: {doc_company} integrado com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar o código depósito: {cod_deposit}, da empresa: {doc_company}. Erro: {ex.Message}");
            }
        }

        [HttpPost("LinxProdutosDetalhes")]
        public async Task<ActionResult> IntegraProdutosDetalhesIndividual([Required][FromQuery] string cod_product, [Required][FromQuery] string doc_company)
        {
            try
            {

                //await _linxProdutosDetalhes.IntegraRegistrosAsync(
                //        "LinxProdutosDetalhes",
                //        "p_LinxProdutosDetalhes_Sincronizacao",
                //        LinxAPIAttributes.TypeEnum.Producao.ToName()
                //    );

                var result = await _linxProdutosDetalhes.IntegraRegistrosIndividualAsync(
                        "LinxProdutosDetalhes",
                        "p_LinxProdutosDetalhes_Sincronizacao",
                        LinxAPIAttributes.TypeEnum.Producao.ToName(),
                        cod_product,
                        doc_company
                    );

                if (result != true)
                    return BadRequest($"A API LinxProdutosDetalhes não encontrou os detalhes do produto: {cod_product}, da empresa: {doc_company}.");
                else
                    return Ok($"Detalhes do produto: {cod_product}, da empresa: {doc_company} integrados com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar os detalhes do produto: {cod_product}, da empresa: {doc_company}. Erro: {ex.Message}");
            }
        }

        [HttpPost("LinxProdutosInventario")]
        public async Task<ActionResult> IntegraProdutosInventarioIndividual([Required][FromQuery] string cod_deposit, [Required][FromQuery] string cod_product, [Required][FromQuery] string doc_company)
        {
            try
            {
                var result = await _linxProdutosInventarioService.IntegraRegistrosIndividualAsync(
                        "LinxProdutosInventario",
                        "p_LinxProdutosInventario_Sincronizacao",
                        LinxAPIAttributes.TypeEnum.Producao.ToName(),
                        cod_deposit,
                        cod_product,
                        doc_company
                    );

                if (result != true)
                    return BadRequest($"A API LinxProdutosInventario não encontrou o inventario do produto: {cod_product}, da empresa: {doc_company}.");
                else
                    return Ok($"Inventario do produto: {cod_product}, da empresa: {doc_company} integrados com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar o inventario do produto: {cod_product}, da empresa: {doc_company}. Erro: {ex.Message}");
            }
        }

        [HttpPost("LinxProdutosTabelas")]
        public async Task<ActionResult> IntegraProdutosTabelasIndividual([Required][FromQuery] string cod_table, [Required][FromQuery] string doc_company)
        {
            try
            {
                var result = await _linxProdutosTabelasService.IntegraRegistrosIndividualAsync(
                        "LinxProdutosTabelas",
                        "p_LinxProdutosTabelas_Sincronizacao",
                        LinxAPIAttributes.TypeEnum.Producao.ToName(),
                        cod_table,
                        doc_company
                    );

                if (result != true)
                    return BadRequest($"A API LinxProdutosTabelas não encontrou a tabela de preços: {cod_table}, da empresa: {doc_company}.");
                else
                    return Ok($"Tabelas de preços: {cod_table}, da empresa: {doc_company} integrados com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar o tabela de preços: {cod_table}, da empresa: {doc_company}. Erro: {ex.Message}");
            }
        }

        [HttpPost("LinxProdutosTabelasPrecos")]
        public async Task<ActionResult> IntegraProdutosTabelasPrecosIndividual([Required][FromQuery] string cod_product, [Required][FromQuery] string cod_table, [Required][FromQuery] string doc_company)
        {
            try
            {
                var result = await _linxProdutosTabelasPrecosService.IntegraRegistrosIndividualAsync(
                        "LinxProdutosTabelasPrecos",
                        "p_LinxProdutosTabelasPrecos_trusted",
                        LinxAPIAttributes.TypeEnum.Producao.ToName(),
                        cod_product,
                        cod_table,
                        doc_company
                    );

                if (result != true)
                    return BadRequest($"A API LinxProdutosTabelasPrecos não encontrou a tabela de preços do produto: {cod_product}, da empresa: {doc_company}.");
                else
                    return Ok($"Tabelas de preços do produto: {cod_product}, da empresa: {doc_company} integrados com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar o tabela de preços do produto: {cod_product}, da empresa: {doc_company}. Erro: {ex.Message}");
            }
        }

        [HttpPost("LinxProdutosCamposAdicionais")]
        public async Task<ActionResult> IntegraProdutosCamposAdicionaisIndividual([Required][FromQuery] string cod_product, [Required][FromQuery] string doc_company)
        {
            try
            {
                var result = await _linxProdutosCamposAdicionaisService.IntegraRegistrosIndividualAsync(
                        "LinxProdutosCamposAdicionais",
                        "p_LinxProdutosCamposAdicionais_Sincronizacao",
                        LinxAPIAttributes.TypeEnum.Producao.ToName(),
                        cod_product,
                        doc_company
                    );

                if (result != true)
                    return BadRequest($"A API LinxProdutosCamposAdicionais não encontrou os dados adicionais do produto: {cod_product}, da empresa: {doc_company}.");
                else
                    return Ok($"Campos adicionais do produto: {cod_product}, da empresa: {doc_company} integrados com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar os campos adicionais do produto: {cod_product}, da empresa: {doc_company}. Erro: {ex.Message}");
            }
        }

        [HttpPost("LinxXMLDocumentos")]
        public async Task<ActionResult> IntegraXMLDocumentosIndividual([Required][FromQuery] string document, [Required][FromQuery] string serie, [Required][FromQuery] string doc_company)
        {
            try
            {
                var result = await _linxXMLDocumentosService.IntegraRegistrosIndividualAsync(
                        "LinxXMLDocumentos",
                        "p_LinxXMLDocumentos_Sincronizacao",
                        LinxAPIAttributes.TypeEnum.Producao.ToName(),
                        document,
                        serie,
                        doc_company
                    );

                if (result != true)
                    return BadRequest($"A API LinxXMLDocumentos não encontrou os dados do documento: {document}, serie: {serie} da empresa: {doc_company}.");
                else
                    return Ok($"Nota fiscal do documento: {document}, serie: {serie} da empresa: {doc_company} integrados com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar o documento: {document}, serie: {serie} da empresa: {doc_company}. Erro: {ex.Message}");
            }
        }

        [HttpPost("LinxPlanos")]
        public async Task<ActionResult> IntegraPlanosIndividual([Required][FromQuery] string cod_plan)
        {
            try
            {
                var result = await _linxPlanosService.IntegraRegistrosIndividualAsync(
                        "LinxPlanos",
                        "p_LinxPlanos_Sincronizacao",
                        LinxAPIAttributes.TypeEnum.Producao.ToName(),
                        cod_plan
                    );

                if (result != true)
                    return BadRequest($"A API LinxPlanos não encontrou os dados do plano: {cod_plan}.");
                else
                    return Ok($"Plano: {cod_plan} integrado com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar o plano: {cod_plan}. Erro: {ex.Message}");
            }
        }

        [HttpPost("LinxGrupoLojas")]
        public async Task<ActionResult> IntegraGrupoLojasIndividual()
        {
            try
            {
                await _linxGrupoLojasService.IntegraRegistrosAsync(
                        "LinxGrupoLojas",
                        "p_LinxGrupoLojas_Sincronizacao",
                        LinxAPIAttributes.TypeEnum.Producao.ToName()
                    );
                return Ok($"Empresas integradas com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar as empresas. Erro: {ex.Message}");
            }
        }

        [HttpPost("LinxMovimentoPlanos")]
        public async Task<ActionResult> IntegraMovimentoPlanosIndividual([Required][FromQuery] string indentifier, [Required][FromQuery] string doc_company)
        {
            try
            {
                var result = await _linxMovimentoPlanosService.IntegraRegistrosIndividualAsync(
                        "LinxMovimentoPlanos",
                        "p_LinxMovimentoPlanos_Sincronizacao",
                        LinxAPIAttributes.TypeEnum.Producao.ToName(),
                        indentifier,
                        doc_company
                    );

                if (result != true)
                    return BadRequest($"A API LinxMovimentoPlanos não encontrou os dados do movimento: {indentifier}.");
                else
                    return Ok($"Dados do movimento: {indentifier} integrados com sucesso.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Nao foi possivel integrar os dados do movimento: {indentifier}. Erro: {ex.Message}");
            }
        }
    }
}
