using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.JSInterop;
using static NewBloomersWebApplication.Domain.Entities.AppContext;
using NewBloomersWebApplication.Domain.Entities.DeliveryList;

namespace NewBloomersWebApplication.UI.Pages
{
    public partial class DeliveryList
    {
        private string? doc_company;
        private string? serie_order;

        private string? inputValueTransportadoras { get; set; } = "7601";

        private bool modalDataInvalida { get; set; }

        private List<Order>? pedidos { get; set; }
        private List<Order>? pedidosRemovidos { get; set; } = new List<Order>();
        private PaginationState pagination = new PaginationState { ItemsPerPage = 50 };

        protected override async Task OnInitializedAsync()
        {
            doc_company = await GetTextInLocalStorage("doc_company");
            serie_order = await GetTextInLocalStorage("serie_order");

            pedidos = await _romaneioService.GetOrdersShipped(inputValueTransportadoras, doc_company, serie_order, DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"));
        }

        private async Task ReloadGrid(DateInterval dateInterval)
        {
            if (dateInterval.initialDate <= dateInterval.finalDate)
            {
                modalDataInvalida = false;
                pedidos = await _romaneioService.GetOrdersShipped(inputValueTransportadoras, doc_company, serie_order, dateInterval.initialDate.ToString("yyyy-MM-dd"), dateInterval.finalDate.ToString("yyyy-MM-dd"));
            }
            else
            {
                modalDataInvalida = true;
            }
        }

        private async Task AdicionaPedidoEnter(Enter evento)
        {
            try
            {
                if (evento.e.Code == "Enter" || evento.e.Code == "NumpadEnter")
                {
                    var _pedido = await _romaneioService.GetOrderShipped(evento.orderNumber, serie_order, doc_company, inputValueTransportadoras);
                    pedidos.Add(_pedido);
                    Thread.Sleep(1 * 1000);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task AdicionaPedidoButton(string nr_pedido)
        {
            var _pedido = await _romaneioService.GetOrderShipped(nr_pedido, serie_order, doc_company, inputValueTransportadoras);
            pedidos.Add(_pedido);
            Thread.Sleep(1 * 1000);
        }

        private void RemovePedido()
        {
            foreach (var pedido in pedidosRemovidos)
            {
                pedidos.Remove(pedido);
            }
        }

        private void RemovePedidoFromList(Order pedido)
        {
            pedidosRemovidos.Add(pedidos.Where(p => p.number == pedido.number).First());
        }

        private void LimpaGrid()
        {
            pedidos.Clear();
        }

        private async Task ImprimeRomaneio()
        {
            try
            {
                await _romaneioService.PrintOrder(pedidos);

                var fileName = $@"deliverylists{pedidos.First().company.doc_company.Substring(pedidos.First().company.doc_company.Length - 3)} - {DateTime.Now.Date.ToString("yyyy-MM-dd")}.pdf";
                var base64String = await _romaneioService.GetDeliveryListToPrint(fileName);

                await jsRuntime.InvokeVoidAsync("downloadFile", "application/pdf", base64String, fileName);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<string> GetTextInLocalStorage(string key)
        {
            return await jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
        }
    }
}
