using Microsoft.AspNetCore.Mvc;

namespace BloomersIntegrationsManager.Controllers
{
    [ApiController]
    [Route("NewBloomers/BloomersMicrovixIntegrations/Microvix")]
    public class BloomersMicrovixMicrovixJobsManagerController : Controller
    {
        //private readonly ILinxClientesFornecService<LinxClientesFornec> _linxClientesFornecService;
        //private readonly ILinxMovimentoCartoesService<LinxMovimentoCartoes> _linxMovimentoCartoesService;
        //private readonly ILinxMovimentoService<LinxMovimento> _linxMovimentoService;
        //private readonly ILinxPedidosCompraService<LinxPedidosCompra> _linxPedidosCompraService;
        //private readonly ILinxPedidosVendaService<LinxPedidosVenda> _linxPedidosVendaService;
        //private readonly ILinxProdutosService<LinxProdutos> _linxProdutosService;
        //private readonly ILinxProdutosCamposAdicionaisService<LinxProdutosCamposAdicionais> _linxProdutosCamposAdicionaisService;
        //private readonly ILinxProdutosDepositosService<LinxProdutosDepositos> _linxProdutosDepositosService;
        //private readonly ILinxProdutosDetalhesService<LinxProdutosDetalhes> _linxProdutosDetalhes;
        //private readonly ILinxProdutosInventarioService<LinxProdutosInventario> _linxProdutosInventarioService;
        //private readonly ILinxProdutosPromocoesService<LinxProdutosPromocoes> _linxProdutosPromocoesService;
        //private readonly ILinxProdutosTabelasService<LinxProdutosTabelas> _linxProdutosTabelasService;
        //private readonly ILinxProdutosTabelasPrecosService<LinxProdutosTabelasPrecos> _linxProdutosTabelasPrecosService;
        //private readonly ILinxXMLDocumentosService<LinxXMLDocumentos> _linxXMLDocumentosService;
        //private readonly ILinxPlanosService<LinxPlanos> _linxPlanosService;
        //private readonly ILinxGrupoLojasService<LinxGrupoLojas> _linxGrupoLojasService;
        //private readonly ILinxMovimentoPlanosService<LinxMovimentoPlanos> _linxMovimentoPlanosService;

        //public BloomersMicrovixMicrovixJobsManagerController
        //    (
        //        ILinxClientesFornecService<LinxClientesFornec> linxClientesFornecService,
        //        ILinxMovimentoCartoesService<LinxMovimentoCartoes> linxMovimentoCartoesService,
        //        ILinxMovimentoService<LinxMovimento> linxMovimentoService,
        //        ILinxPedidosCompraService<LinxPedidosCompra> linxPedidosCompraService,
        //        ILinxPedidosVendaService<LinxPedidosVenda> linxPedidosVendaService,
        //        ILinxProdutosService<LinxProdutos> linxProdutosService,
        //        ILinxProdutosCamposAdicionaisService<LinxProdutosCamposAdicionais> linxProdutosCamposAdicionaisService,
        //        ILinxProdutosDepositosService<LinxProdutosDepositos> linxProdutosDepositosService,
        //        ILinxProdutosDetalhesService<LinxProdutosDetalhes> linxProdutosDetalhes,
        //        ILinxProdutosInventarioService<LinxProdutosInventario> linxProdutosInventarioService,
        //        ILinxProdutosPromocoesService<LinxProdutosPromocoes> linxProdutosPromocoesService,
        //        ILinxProdutosTabelasService<LinxProdutosTabelas> linxProdutosTabelasService,
        //        ILinxProdutosTabelasPrecosService<LinxProdutosTabelasPrecos> linxProdutosTabelasPrecosService,
        //        ILinxXMLDocumentosService<LinxXMLDocumentos> linxXMLDocumentosService,
        //        ILinxPlanosService<LinxPlanos> linxPlanosService,
        //        ILinxGrupoLojasService<LinxGrupoLojas> linxGrupoLojasService,
        //        ILinxMovimentoPlanosService<LinxMovimentoPlanos> linxMovimentoPlanosService
        //    )
        //    =>
        //    (
        //        _linxClientesFornecService,
        //        _linxMovimentoCartoesService,
        //        _linxMovimentoService,
        //        _linxPedidosCompraService,
        //        _linxPedidosVendaService,
        //        _linxProdutosService,
        //        _linxProdutosCamposAdicionaisService,
        //        _linxProdutosDepositosService,
        //        _linxProdutosDetalhes,
        //        _linxProdutosInventarioService,
        //        _linxProdutosPromocoesService,
        //        _linxProdutosTabelasService,
        //        _linxProdutosTabelasPrecosService,
        //        _linxXMLDocumentosService,
        //        _linxPlanosService,
        //        _linxGrupoLojasService,
        //        _linxMovimentoPlanosService
        //    ) =
        //    (
        //        linxClientesFornecService,
        //        linxMovimentoCartoesService,
        //        linxMovimentoService,
        //        linxPedidosCompraService,
        //        linxPedidosVendaService,
        //        linxProdutosService,
        //        linxProdutosCamposAdicionaisService,
        //        linxProdutosDepositosService,
        //        linxProdutosDetalhes,
        //        linxProdutosInventarioService,
        //        linxProdutosPromocoesService,
        //        linxProdutosTabelasService,
        //        linxProdutosTabelasPrecosService,
        //        linxXMLDocumentosService,
        //        linxPlanosService,
        //        linxGrupoLojasService,
        //        linxMovimentoPlanosService
        //    );

        //[HttpPost("LinxClientesFornec")]
        //public async Task<ActionResult> IntegraClientesFornecIndividual([FromBody] LinxClientesFornecRequest request)
        //{
        //    try
        //    {
        //        var result = await _linxClientesFornecService.IntegraRegistrosIndividual(
        //                "LinxClientesFornec",
        //                "p_LinxClientesFornec_Sincronizacao",
        //                LinxAPIAttributes.TypeEnum.Producao.ToName(),
        //                request.doc_cliente
        //            );

        //        if (result != true)
        //            return BadRequest($"A API LinxClientesFornec não encontrou o cliente: {request.doc_cliente}.");
        //        else
        //            return Ok($"Cliente: {request.doc_cliente} integrado com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel integrar o cliente: {request.doc_cliente}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("LinxMovimentoCartoes")]
        //public async Task<ActionResult> IntegraMovimentoCartoes()
        //{
        //    try
        //    {
        //        await _linxMovimentoCartoesService.IntegraRegistros(
        //                "LinxMovimentoCartoes",
        //                "p_LinxMovimentoCartoes_Sincronizacao",
        //                LinxAPIAttributes.TypeEnum.Producao.ToName()
        //            );

        //        return Ok();
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}

        //[HttpPost("LinxMovimento")]
        //public async Task<ActionResult> IntegraMovimentoIndividual([FromBody] LinxMovimentoRequest request)
        //{
        //    try
        //    {
        //        var result = await _linxMovimentoService.IntegraRegistrosIndividual(
        //                "LinxMovimento",
        //                "p_LinxMovimento_trusted_unificado",
        //                LinxAPIAttributes.TypeEnum.Producao.ToName(),
        //                request.documento,
        //                request.cnpj_emp
        //            );

        //        if (result != true)
        //            return BadRequest($"A API LinxMovimento não encontrou o documento: {request.documento}, da empresa: {request.cnpj_emp}.");
        //        else
        //            return Ok($"Documento: {request.documento}, da empresa: {request.cnpj_emp} integrado com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel integrar o documento: {request.documento}, da empresa: {request.cnpj_emp}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("LinxPedidosCompra")]
        //public async Task<ActionResult> IntegraPedidosCompra()
        //{
        //    try
        //    {
        //        await _linxPedidosCompraService.IntegraRegistros(
        //                "LinxPedidosCompra",
        //                "p_LinxPedidosCompra_Sincronizacao",
        //                LinxAPIAttributes.TypeEnum.Producao.ToName()
        //            );

        //        return Ok();
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}

        //[HttpPost("LinxPedidosVenda")]
        //public async Task<ActionResult> IntegraPedidosVendaIndividual([FromBody] LinxPedidosVendaRequest request)
        //{
        //    try
        //    {
        //        var result = await _linxPedidosVendaService.IntegraRegistrosIndividual(
        //                "LinxPedidosVenda",
        //                "p_LinxPedidosVenda_trusted_unificado",
        //                LinxAPIAttributes.TypeEnum.Producao.ToName(),
        //                request.cod_pedido,
        //                request.cnpj_emp
        //            );

        //        if (result != true)
        //            return BadRequest($"A API LinxPedidosVenda não encontrou o pedido: {request.cod_pedido}, da empresa: {request.cnpj_emp}.");
        //        else
        //            return Ok($"Pedido: {request.cod_pedido}, da empresa: {request.cnpj_emp} integrado com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel integrar o pedido: {request.cod_pedido}, da empresa: {request.cnpj_emp}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("LinxProdutos")]
        //public async Task<ActionResult> IntegraProdutosIndividual([FromBody] LinxProdutosRequest request)
        //{
        //    try
        //    {
        //        var result = await _linxProdutosService.IntegraRegistrosIndividual(
        //                "LinxProdutos",
        //                "p_LinxProdutos_Sincronizacao",
        //                LinxAPIAttributes.TypeEnum.Producao.ToName(),
        //                request.cod_produto,
        //                request.cnpj_emp
        //            );

        //        if (result != true)
        //            return BadRequest($"A API LinxProdutos não encontrou os dados do produto: {request.cod_produto}, da empresa: {request.cnpj_emp}.");
        //        else
        //            return Ok($"Produto: {request.cod_produto}, da empresa: {request.cnpj_emp} integrados com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel integrar o produto: {request.cod_produto}, da empresa: {request.cnpj_emp}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("LinxProdutosPromocoes")]
        //public async Task<ActionResult> IntegraProdutosPromocoes()
        //{
        //    try
        //    {
        //        await _linxProdutosPromocoesService.IntegraRegistros(
        //                "LinxProdutosPromocoes",
        //                "p_LinxProdutosPromocoes_Sincronizacao",
        //                LinxAPIAttributes.TypeEnum.Producao.ToName()
        //            );

        //        return Ok();
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}

        //[HttpPost("LinxProdutosDepositos")]
        //public async Task<ActionResult> IntegraProdutosDepositosIndividual([FromBody] LinxProdutosDepositosRequest request)
        //{
        //    try
        //    {
        //        var result = await _linxProdutosDepositosService.IntegraRegistrosIndividual(
        //                "LinxProdutosDepositos",
        //                "p_LinxProdutosDepositos_Sincronizacao",
        //                LinxAPIAttributes.TypeEnum.Producao.ToName(),
        //                request.cod_deposito,
        //                request.cnpj_emp
        //            );

        //        if (result != true)
        //            return BadRequest($"A API LinxProdutosDepositos não encontrou o deposito: {request.cod_deposito}, da empresa: {request.cnpj_emp}.");
        //        else
        //            return Ok($"Código Depósito: {request.cod_deposito}, da empresa: {request.cnpj_emp} integrado com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel integrar o código depósito: {request.cod_deposito}, da empresa: {request.cnpj_emp}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("LinxProdutosDetalhes")]
        //public async Task<ActionResult> IntegraProdutosDetalhesIndividual([FromBody] LinxProdutosRequest request)
        //{
        //    try
        //    {
        //        var result = await _linxProdutosDetalhes.IntegraRegistrosIndividual(
        //                "LinxProdutosDetalhes",
        //                "p_LinxProdutosDetalhes_Sincronizacao",
        //                LinxAPIAttributes.TypeEnum.Producao.ToName(),
        //                request.cod_produto,
        //                request.cnpj_emp
        //            );

        //        if (result != true)
        //            return BadRequest($"A API LinxProdutosDetalhes não encontrou os detalhes do produto: {request.cod_produto}, da empresa: {request.cnpj_emp}.");
        //        else
        //            return Ok($"Detalhes do produto: {request.cod_produto}, da empresa: {request.cnpj_emp} integrados com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel integrar os detalhes do produto: {request.cod_produto}, da empresa: {request.cnpj_emp}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("LinxProdutosInventario")]
        //public async Task<ActionResult> IntegraProdutosInventarioIndividual([FromBody] LinxProdutosInventarioRequest request)
        //{
        //    try
        //    {
        //        var result = await _linxProdutosInventarioService.IntegraRegistrosIndividual(
        //                "LinxProdutosInventario",
        //                "p_LinxProdutosInventario_Sincronizacao",
        //                LinxAPIAttributes.TypeEnum.Producao.ToName(),
        //                request.cod_deposito,
        //                request.cod_produto,
        //                request.cnpj_emp
        //            );

        //        if (result != true)
        //            return BadRequest($"A API LinxProdutosInventario não encontrou o inventario do produto: {request.cod_produto}, da empresa: {request.cnpj_emp}.");
        //        else
        //            return Ok($"Inventario do produto: {request.cod_produto}, da empresa: {request.cnpj_emp} integrados com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel integrar o inventario do produto: {request.cod_produto}, da empresa: {request.cnpj_emp}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("LinxProdutosTabelas")]
        //public async Task<ActionResult> IntegraProdutosTabelasIndividual([FromBody] LinxProdutosTabelasRequest request)
        //{
        //    try
        //    {
        //        var result = await _linxProdutosTabelasService.IntegraRegistrosIndividual(
        //                "LinxProdutosTabelas",
        //                "p_LinxProdutosTabelas_Sincronizacao",
        //                LinxAPIAttributes.TypeEnum.Producao.ToName(),
        //                request.cod_tabela,
        //                request.cnpj_emp
        //            );

        //        if (result != true)
        //            return BadRequest($"A API LinxProdutosTabelas não encontrou a tabela de preços: {request.cod_tabela}, da empresa: {request.cnpj_emp}.");
        //        else
        //            return Ok($"Tabelas de preços: {request.cod_tabela}, da empresa: {request.cnpj_emp} integrados com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel integrar o tabela de preços: {request.cod_tabela}, da empresa: {request.cnpj_emp}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("LinxProdutosTabelasPrecos")]
        //public async Task<ActionResult> IntegraProdutosTabelasPrecosIndividual([FromBody] LinxProdutosTabelasPrecosRequest request)
        //{
        //    try
        //    {
        //        var result = await _linxProdutosTabelasPrecosService.IntegraRegistrosIndividual(
        //                "LinxProdutosTabelasPrecos",
        //                "p_LinxProdutosTabelasPrecos_trusted",
        //                LinxAPIAttributes.TypeEnum.Producao.ToName(),
        //                request.cod_produto,
        //                request.cod_tabela,
        //                request.cnpj_emp
        //            );

        //        if (result != true)
        //            return BadRequest($"A API LinxProdutosTabelasPrecos não encontrou a tabela de preços do produto: {request.cod_produto}, da empresa: {request.cnpj_emp}.");
        //        else
        //            return Ok($"Tabelas de preços do produto: {request.cod_produto}, da empresa: {request.cnpj_emp} integrados com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel integrar o tabela de preços do produto: {request.cod_produto}, da empresa: {request.cnpj_emp}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("LinxProdutosCamposAdicionais")]
        //public async Task<ActionResult> IntegraProdutosCamposAdicionaisIndividual([FromBody] LinxProdutosRequest request)
        //{
        //    try
        //    {
        //        var result = await _linxProdutosCamposAdicionaisService.IntegraRegistrosIndividual(
        //                "LinxProdutosCamposAdicionais",
        //                "p_LinxProdutosCamposAdicionais_Sincronizacao",
        //                LinxAPIAttributes.TypeEnum.Producao.ToName(),
        //                request.cod_produto,
        //                request.cnpj_emp
        //            );

        //        if (result != true)
        //            return BadRequest($"A API LinxProdutosCamposAdicionais não encontrou os dados adicionais do produto: {request.cod_produto}, da empresa: {request.cnpj_emp}.");
        //        else
        //            return Ok($"Campos adicionais do produto: {request.cod_produto}, da empresa: {request.cnpj_emp} integrados com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel integrar os campos adicionais do produto: {request.cod_produto}, da empresa: {request.cnpj_emp}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("LinxXMLDocumentos")]
        //public async Task<ActionResult> IntegraXMLDocumentosIndividual([FromBody] LinxXMLDocumentosRequest request)
        //{
        //    try
        //    {
        //        var result = await _linxXMLDocumentosService.IntegraRegistrosIndividual(
        //                "LinxXMLDocumentos",
        //                "p_LinxXMLDocumentos_Sincronizacao",
        //                LinxAPIAttributes.TypeEnum.Producao.ToName(),
        //                request.documento,
        //                request.serie,
        //                request.cnpj_emp
        //            );

        //        if (result != true)
        //            return BadRequest($"A API LinxXMLDocumentos não encontrou os dados do documento: {request.documento}, serie: {request.serie} da empresa: {request.cnpj_emp}.");
        //        else
        //            return Ok($"Nota fiscal do documento: {request.documento}, serie: {request.serie} da empresa: {request.cnpj_emp} integrados com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel integrar o documento: {request.documento}, serie: {request.serie} da empresa: {request.cnpj_emp}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("LinxPlanos")]
        //public async Task<ActionResult> IntegraPlanosIndividual([FromBody] LinxPlanosRequest request)
        //{
        //    try
        //    {
        //        var result = await _linxPlanosService.IntegraRegistrosIndividual(
        //                "LinxPlanos",
        //                "p_LinxPlanos_Sincronizacao",
        //                LinxAPIAttributes.TypeEnum.Producao.ToName(),
        //                request.cod_plano
        //            );

        //        if (result != true)
        //            return BadRequest($"A API LinxPlanos não encontrou os dados do plano: {request.cod_plano}.");
        //        else
        //            return Ok($"Plano: {request.cod_plano} integrado com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel integrar o plano: {request.cod_plano}. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("LinxGrupoLojas")]
        //public async Task<ActionResult> IntegraGrupoLojasIndividual() 
        //{
        //    try
        //    {
        //        await _linxGrupoLojasService.IntegraRegistros(
        //                "LinxGrupoLojas",
        //                "p_LinxGrupoLojas_Sincronizacao",
        //                LinxAPIAttributes.TypeEnum.Producao.ToName()
        //            );
        //        return Ok($"Empresas integradas com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel integrar as empresas. Erro: {ex.Message}");
        //    }
        //}

        //[HttpPost("LinxMovimentoPlanos")]
        //public async Task<ActionResult> IntegraMovimentoPlanosIndividual([FromBody] LinxMovimentoPlanosRequest request)
        //{
        //    try
        //    {
        //        await _linxMovimentoPlanosService.IntegraRegistros(
        //                "LinxMovimentoPlanos",
        //                "p_LinxMovimentoPlanos_Sincronizacao",
        //                LinxAPIAttributes.TypeEnum.Producao.ToName()
        //            );
                
        //        var result = await _linxMovimentoPlanosService.IntegraRegistrosIndividual(
        //                "LinxMovimentoPlanos",
        //                "p_LinxMovimentoPlanos_Sincronizacao",
        //                LinxAPIAttributes.TypeEnum.Producao.ToName(),
        //                request.indentificador,
        //                request.cnpj_emp
        //            );

        //        if (result != true)
        //            return BadRequest($"A API LinxMovimentoPlanos não encontrou os dados do movimento: {request.indentificador}.");
        //        else
        //            return Ok($"Dados do movimento: {request.indentificador} integrados com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content($"Nao foi possivel integrar os dados do movimento: {request.indentificador}. Erro: {ex.Message}");
        //    }
        //}
    }
}
