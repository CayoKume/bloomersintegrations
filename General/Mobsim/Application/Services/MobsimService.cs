using BloomersGeneralIntegrations.Mobsim.Domain.Entities;
using BloomersGeneralIntegrations.Mobsim.Infrastructure.Apis;
using BloomersGeneralIntegrations.Mobsim.Infrastructure.Repositorys;

namespace BloomersGeneralIntegrations.Mobsim.Application.Services
{
    public class MobsimService : IMobsimService
    {
        private readonly IAPICall _apiCall;
        private readonly IMobsimRepository _mobsimRepository;

        public MobsimService(IMobsimRepository mobsimRepository, IAPICall apiCall) =>
            (_mobsimRepository, _apiCall) = (mobsimRepository, apiCall);

        public async Task SendMessageDeliveredOrder()
        {
            try
            {
                var pedidosEntregues = await _mobsimRepository.GetDeliveredOrders();

                Guid pedidosEntreguesAgrupados = Guid.NewGuid();
                var messagesDeliveredOrders = new List<Message>();
                var listBodyLog = new List<string>();

                foreach (var pedido in pedidosEntregues)
                {
                    var messageSent = await _mobsimRepository.HasLogInMobsimHistoric(pedido.Documento);
                    if (messageSent.Pedido is not null && messageSent.Faturado is true && messageSent.Enviado is true && messageSent.Entregue is false)
                    {
                        var cliente = await _mobsimRepository.GetClient(pedido.CodCliente);
                        if (cliente.CelularCliente == String.Empty)
                            continue;

                        var message = new Message
                        {
                            id = Guid.NewGuid().ToString(),
                            //to = "11999082056",
                            to = cliente.CelularCliente,
                            msg = $"Olá {cliente.NomeCliente}, tudo bem? Recebemos a informação que seu pedido ({pedido.Documento}) foi entregue. Aproveite para nos seguir nas redes sociais!"
                        };

                        listBodyLog.Add(pedido.Documento);
                        messagesDeliveredOrders.Add(message);
                    }
                }

                var body = new MobsimObject
                {
                    groupId = pedidosEntreguesAgrupados.ToString(),
                    messages = messagesDeliveredOrders
                };

                if (messagesDeliveredOrders.Any())
                {
                    await _apiCall.PostAsync(body);
                    pedidosEntreguesAgrupados = Guid.Empty;

                    foreach (var item in listBodyLog)
                    {
                        //criar processo, caso o pedido nao tenha registro na mobsimHistorico, criar com os status anteriores e dar o update em seguida
                        await _mobsimRepository.UpdateStatusMobsimHistoric(item, false, true);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SendMessageInvoicedOrder()
        {
            try
            {
                var pedidosFaturados = await _mobsimRepository.GetInvoicedOrders();

                var listBodyLog = new List<BodyLog>();
                Guid pedidosFaturadosAgrupados = Guid.NewGuid();
                var messagesInvoicedOrders = new List<Message>();

                foreach (var pedido in pedidosFaturados)
                {
                    var messageSent = await _mobsimRepository.HasLogInMobsimHistoric(pedido.Documento);
                    if (messageSent.Pedido is null)
                    {
                        var cliente = await _mobsimRepository.GetClient(pedido.CodCliente);
                        if (cliente.CelularCliente == String.Empty)
                            continue;

                        var message = new Message
                        {
                            id = Guid.NewGuid().ToString(),
                            to = cliente.CelularCliente,
                            msg = $"Olá {cliente.NomeCliente}, tudo bem? Seu pedido ({pedido.Documento}) acabou de ser Faturado. Logo enviaremos para você..."
                        };

                        var log = new BodyLog
                        {
                            id = new Guid(message.id),
                            pedido = pedido.Documento,
                            cliente = pedido.CodCliente
                        };

                        listBodyLog.Add(log);
                        messagesInvoicedOrders.Add(message);
                    }
                }

                var body = new MobsimObject
                {
                    groupId = pedidosFaturadosAgrupados.ToString(),
                    messages = messagesInvoicedOrders
                };

                if (messagesInvoicedOrders.Any())
                {
                    await _apiCall.PostAsync(body);
                    pedidosFaturadosAgrupados = Guid.Empty;

                    foreach (var item in listBodyLog)
                    {
                        await _mobsimRepository.InsertMobsimHistoric(item.id, item.pedido, item.cliente);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task SendMessageShippdedOrder()
        {
            try
            {
                var pedidosExpedidos = await _mobsimRepository.GetShippedOrders();

                Guid pedidosExpedidosAgrupados = Guid.NewGuid();
                var messagesShippedOrders = new List<Message>();
                var listBodyLog = new List<string>();

                foreach (var pedido in pedidosExpedidos)
                {
                    var messageSent = await _mobsimRepository.HasLogInMobsimHistoric(pedido.Documento);
                    if (messageSent.Pedido is not null && messageSent.Faturado is true && messageSent.Enviado is false && messageSent.Entregue is false)
                    {
                        var cliente = await _mobsimRepository.GetClient(pedido.CodCliente);
                        if (cliente.CelularCliente == String.Empty)
                            continue;

                        var txtMessage = $"Olá {cliente.NomeCliente}, tudo bem? Seu pedido ({pedido.Documento}) está a caminho. Qualquer dúvida: WhatsApp (11) 3627-5900";

                        if (pedido.Transportadora == "18035")
                            txtMessage = $"Olá {cliente.NomeCliente}, tudo bem? Seu pedido ({pedido.Documento}) está a caminho. Acompanhe aqui: https://www.flashcourier.com.br/rastreio/{pedido.Documento}";

                        else if (pedido.Transportadora == "7601")
                            txtMessage = $"Olá {cliente.NomeCliente}, tudo bem? Seu pedido ({pedido.Documento}) está a caminho. Acompanhe aqui: https://tracking.totalexpress.com.br/";

                        var message = new Message
                        {
                            id = Guid.NewGuid().ToString(),
                            //to = "11999082056",
                            to = cliente.CelularCliente,
                            msg = txtMessage
                        };

                        listBodyLog.Add(pedido.Documento);
                        messagesShippedOrders.Add(message);
                    }
                }

                var body = new MobsimObject
                {
                    groupId = pedidosExpedidosAgrupados.ToString(),
                    messages = messagesShippedOrders
                };

                if (messagesShippedOrders.Any())
                {
                    await _apiCall.PostAsync(body);
                    pedidosExpedidosAgrupados = Guid.Empty;

                    foreach (var item in listBodyLog)
                    {
                        //criar processo, caso o pedido nao tenha registro na mobsimHistorico, criar com os status anteriores e dar o update em seguida
                        await _mobsimRepository.UpdateStatusMobsimHistoric(item, true, false);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
