namespace BloomersCarriersIntegrations.FlashCourier.Application.Services
{
    public class FlashCourierService : IFlashCourierService
    {
        private readonly IFlashCourierRepository _flashCourierRepository;

        public FlashCourierService(IFlashCourierRepository flashCourierRepository) =>
            _flashCourierRepository = flashCourierRepository;

        public async Task AtualizaLogPedidoEnviado()
        {
            try
            {
                var logs = await _flashCourierRepository.GetPedidosEnviados();

                string[] numEncCli = logs.Select(a => a.Pedido).ToArray();
                var result = APICall.GetHAWB(numEncCli);

                if (result.statusRetorno == "00")
                {
                    foreach (var hawb in result.hawbs)
                    {
                        var currentStatusHawb = hawb.historico.Last().evento;
                        var currentHistoricoHawb = hawb.historico.LastOrDefault();
                        var currentColetaHawb = hawb.historico.Where(a => a.evento == "Postado - logistica iniciada").FirstOrDefault();
                        var currentOrder = logs.FirstOrDefault(a => a.Pedido == hawb.codigoCartao);

                        if (!String.IsNullOrEmpty(hawb.dtSla))
                            await _flashCourierRepository.Update_NB_PREVISAO_REAL_ENTREGA(hawb.dtSla, hawb.codigoCartao);

                        if (currentStatusHawb.ToUpper().Contains("ENTREGUE") || currentStatusHawb.ToUpper() == "COMPROVANTE REGISTRADO")
                            await _flashCourierRepository.Update_NB_DATA_ENTREGA_REALIZADA(currentHistoricoHawb.ocorrencia, hawb.codigoCartao);

                        if (currentColetaHawb is not null && currentColetaHawb.evento == "Postado - logistica iniciada")
                            await _flashCourierRepository.Update_NB_DATA_COLETA(currentColetaHawb.ocorrencia, hawb.codigoCartao);

                        if (!String.IsNullOrEmpty(currentHistoricoHawb.evento))
                            await _flashCourierRepository.Update_NB_DATA_ULTIMO_STATUS(currentHistoricoHawb.ocorrencia, currentHistoricoHawb.eventoId, currentHistoricoHawb.evento, hawb.codigoCartao);

                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> EnviaPedidoFlash(string order_number)
        {
            var pedido = _flashCourierRepository.GetPedidoFaturado(order_number);

            if (pedido is not null)
            {
                Thread.Sleep(1000);
                var postHAWB = APICall.PostHAWB(pedido).FirstOrDefault();
                if (postHAWB.type.ToUpper() == "SUCESS")
                {
                    var retorno = JsonSerializer.Serialize(postHAWB);
                    await _flashCourierRepository.GeraLogSucesso(pedido: pedido.nr_pedido, remetenteID: pedido.empresa.doc_empresa, retorno, statusFlash: "Enviado", chaveNFe: pedido.notaFiscal.chave_nfe_nf);
                }
                else
                {
                    var retorno = JsonSerializer.Serialize(postHAWB);
                    await _flashCourierRepository.GeraLogSucesso(pedido: pedido.nr_pedido, remetenteID: pedido.empresa.doc_empresa, retorno, statusFlash: "Erro_Flash", chaveNFe: pedido.notaFiscal.chave_nfe_nf);
                }
                
                return true;
            }
            else
                return false;
        }

        public async Task<bool> EnviaPedidosFlash()
        {
            try
            {
                var pedidos = await _flashCourierRepository.GetPedidosFaturados();

                if (pedidos.Count() > 0)
                {
                    foreach (var pedido in pedidos)
                    {
                        Thread.Sleep(1000);
                        var postHAWB = APICall.PostHAWB(pedido).FirstOrDefault();
                        if (postHAWB.type.ToUpper() == "SUCESS")
                        {
                            var retorno = JsonSerializer.Serialize(postHAWB);
                            await _flashCourierRepository.GeraLogSucesso(pedido: pedido.nr_pedido, remetenteID: pedido.empresa.doc_empresa, retorno, statusFlash: "Enviado", chaveNFe: pedido.notaFiscal.chave_nfe_nf);
                        }
                        else
                        {
                            var retorno = JsonSerializer.Serialize(postHAWB);
                            await _flashCourierRepository.GeraLogSucesso(pedido: pedido.nr_pedido, remetenteID: pedido.empresa.doc_empresa, retorno, statusFlash: "Erro_Flash", chaveNFe: pedido.notaFiscal.chave_nfe_nf);
                        }
                    }
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
