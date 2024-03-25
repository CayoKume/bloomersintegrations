using BloomersCarriersIntegrations.FlashCourier.Infrastructure.Apis;
using BloomersCarriersIntegrations.FlashCourier.Infrastructure.Repositorys;
using System.Text.Json;

namespace BloomersCarriersIntegrations.FlashCourier.Application.Services
{
    public class FlashCourierService : IFlashCourierService
    {
        private readonly IAPICall _apiCall;
        private readonly IFlashCourierRepository _flashCourierRepository;

        public FlashCourierService(IAPICall apiCall, IFlashCourierRepository flashCourierRepository) =>
            (_apiCall, _flashCourierRepository) = (apiCall, flashCourierRepository);

        public async Task AtualizaLogPedidoEnviado()
        {
            try
            {
                var logs = await _flashCourierRepository.GetShippedOrders();

                string[] numEncCli = logs.Select(a => a.orderNumber).ToArray();
                var result = await _apiCall.GetHAWB(numEncCli, logs.Select(a => a.doc_company).First());
                
                if (result.statusRetorno == "00")
                {
                    foreach (var hawb in result.hawbs)
                    {
                        var currentStatusHawb = hawb.historico.Last().evento;
                        var currentHistoricoHawb = hawb.historico.LastOrDefault();
                        var currentColetaHawb = hawb.historico.Where(a => a.evento.ToUpper() == "POSTADO - LOGISTICA INICIADA").FirstOrDefault();
                        var currentOrder = logs.FirstOrDefault(a => a.orderNumber == hawb.codigoCartao);

                        if (!String.IsNullOrEmpty(hawb.dtSla))
                            await _flashCourierRepository.UpdateRealDeliveryForecastDate(hawb.dtSla, hawb.codigoCartao);

                        if (currentStatusHawb.ToUpper().Contains("ENTREGUE") || currentStatusHawb.ToUpper() == "COMPROVANTE REGISTRADO")
                            await _flashCourierRepository.UpdateDeliveryMadeDate(currentHistoricoHawb.ocorrencia, hawb.codigoCartao);

                        if (currentColetaHawb is not null && currentColetaHawb.evento.ToUpper() == "POSTADO - LOGISTICA INICIADA")
                            await _flashCourierRepository.UpdateCollectionDate(currentColetaHawb.ocorrencia, hawb.codigoCartao);

                        if (!String.IsNullOrEmpty(currentHistoricoHawb.evento))
                            await _flashCourierRepository.UpdateLastStatusDate(currentHistoricoHawb.ocorrencia, currentHistoricoHawb.eventoId, currentHistoricoHawb.evento, hawb.codigoCartao);

                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> EnviaPedidoFlash(string order_number)
        {
            try
            {
                var order = await _flashCourierRepository.GetInvoicedOrder(order_number);

                if (order is not null)
                {
                    Thread.Sleep(1000);
                    var postHAWB = await _apiCall.PostHAWB(order);
                    if (postHAWB.First().type.ToUpper() == "SUCESS")
                    {
                        var retorno = JsonSerializer.Serialize(postHAWB);
                        await _flashCourierRepository.GenerateSucessLog(orderNumber: order.number, senderID: order.company.doc_company, retorno, statusFlash: "Enviado", keyNFe: order.invoice.key_nfe_nf);
                    }
                    else
                    {
                        var retorno = JsonSerializer.Serialize(postHAWB);
                        await _flashCourierRepository.GenerateSucessLog(orderNumber: order.number, senderID: order.company.doc_company, retorno, statusFlash: "Erro_Flash", keyNFe: order.invoice.key_nfe_nf);
                    }

                    return true;
                }
                else
                    return false;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> EnviaPedidosFlash()
        {
            try
            {
                var orders = await _flashCourierRepository.GetInvoicedOrders();

                if (orders.Count() > 0)
                {
                    foreach (var order in orders)
                    {
                        Thread.Sleep(1000);
                        //var postHAWB = APICall.PostHAWB(order).FirstOrDefault();
                        //if (postHAWB.type.ToUpper() == "SUCESS")
                        //{
                        //    var retorno = JsonSerializer.Serialize(postHAWB);
                        //    await _flashCourierRepository.GenerateSucessLog(orderNumber: order.number, senderID: order.company.doc_company, retorno, statusFlash: "Enviado", keyNFe: order.invoice.key_nfe_nf);
                        //}
                        //else
                        //{
                        //    var retorno = JsonSerializer.Serialize(postHAWB);
                        //    await _flashCourierRepository.GenerateSucessLog(orderNumber: order.number, senderID: order.company.doc_company, retorno, statusFlash: "Erro_Flash", keyNFe: order.invoice.key_nfe_nf);
                        //}
                    }
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                throw;
            }
        }
    }
}
