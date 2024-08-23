using BloomersCarriersIntegrations.TotalExpress.Infrastructure.Apis;
using BloomersCarriersIntegrations.TotalExpress.Infrastructure.Repositorys;
using static BloomersCarriersIntegrations.TotalExpress.Domain.Entities.TotalInfos;

namespace BloomersCarriersIntegrations.TotalExpress.Application.Services
{
    public class TotalExpressService : ITotalExpressService
    {
        private readonly IAPICall _apiCall;
        private readonly ITotalExpressRepository _totalExpressRepository;

        public TotalExpressService(ITotalExpressRepository totalExpressRepository, IAPICall apiCall) =>
            (_totalExpressRepository, _apiCall) = (totalExpressRepository, apiCall);

        public async Task<bool> SendOrders()
        {
            try
            {
                var orders = await _totalExpressRepository.GetInvoicedOrders();
                if (orders.Count() > 0)
                {
                    foreach (var order in orders)
                    {
                        var registros = _apiCall.BuildRegistro(order);
                        for (int i = 0; i < registros.Count(); i++)
                        {
                            var response = await _apiCall.PostAWB(order.number, registros[i]);
                            await _totalExpressRepository.GeraResponseLog(order.number, order.REMETENTEID, response);
                        }
                    }
                    return true;
                }

                return false;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> SendOrder(string order_number)
        {
            try
            {
                var order = await _totalExpressRepository.GetInvoicedOrder(order_number);
                if (order is not null)
                {
                    var registros = _apiCall.BuildRegistro(order);
                    for (int i = 0; i < registros.Count(); i++)
                    {
                        var response = await _apiCall.PostAWB(order.number, registros[i]);
                        await _totalExpressRepository.GeraResponseLog(order.number, order.REMETENTEID, response);
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

        public async Task<bool> SendOrderAsEtur(string order_number)
        {
            try
            {
                var order = await _totalExpressRepository.GetInvoicedOrderETUR(order_number);
                if (order is not null)
                {
                    var registros = _apiCall.BuildRegistro(order);
                    for (int i = 0; i < registros.Count(); i++)
                    {
                        var response = await _apiCall.PostAWB(order.number, registros[i]);
                        await _totalExpressRepository.GeraResponseLog(order.number, order.REMETENTEID, response);
                        await _totalExpressRepository.UpdatePedidoETUR(order_number);
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

        public async Task UpdateOrderSendLog()
        {
            try
            {
                var orders = await _totalExpressRepository.GetPedidosEnviados();

                foreach (var order in orders)
                {
                    var status = await _apiCall.GetAWB(order.remetenteid, order.pedido);

                    if (status is null)
                        continue;
                    else if (status.detalhes == null)
                        continue;

                    //PEGA PEDIDOS ENVIADOS E AINDA NÃO COLETADOS E ATUALIZA NB_DATA_COLETA, NB_DATA_ULTIMO_STATUS, NB_DESCRICAO_ULTIMO_STATUS
                    var statusColeta = (from a in status.detalhes.statusDeEncomenda select a).Where(a => a.status == "COLETA REALIZADA"); //coleta realizada

                    if (statusColeta.Count() > 0)
                        await _totalExpressRepository.Update_NB_DATA_COLETA(statusColeta.First().data, status.pedido); //data da coleta

                    //PEGA PEDIDOS COLETADOS E ATUALIZA NB_PREVISAO_REAL_ENTREGA, NB_DATA_ULTIMO_STATUS, NB_DESCRICAO_ULTIMO_STATUS
                    if (status.detalhes.dataPrev != null)
                    {
                        if (status.detalhes.dataPrev.PrevEntregaAtualizada != null && Convert.ToDateTime(status.detalhes.dataPrev.PrevEntregaAtualizada) != order.previsao_real_entrega)
                            await _totalExpressRepository.Update_NB_PREVISAO_REAL_ENTREGA(status.detalhes.dataPrev.PrevEntregaAtualizada, order.pedido); //previsão de entrega real
                        else if (status.detalhes.dataPrev.PrevEntrega != null)
                            await _totalExpressRepository.Update_NB_PREVISAO_REAL_ENTREGA(status.detalhes.dataPrev.PrevEntrega, order.pedido); //previsão de entrega
                    }

                    var lastStatus = status.detalhes.statusDeEncomenda.OrderByDescending(a => DateTime.Parse(a.data)).FirstOrDefault(); //last status

                    //PEGA PEDIDOS ENTREGUES E ATUALIZA NB_DATA_ENTREGA_REALIZADA, NB_DATA_ULTIMO_STATUS, NB_DESCRICAO_ULTIMO_STATUS
                    if (lastStatus.status.ToUpper().Contains("ENTREGA REALIZADA"))
                        await _totalExpressRepository.Update_NB_DATA_ENTREGA_REALIZADA(lastStatus.data, order.pedido); //entrega realizada

                    if ($"{lastStatus.statusid}-{lastStatus.status}" != order.descricao_ultimo_status)
                        await _totalExpressRepository.Update_NB_DATA_ULTIMO_STATUS(lastStatus.data, lastStatus.statusid, lastStatus.status, order.pedido); //ultimo status
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
