using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using static NewBloomersWebApplication.Infrastructure.Domain.Entities.AppContext;
using Order = NewBloomersWebApplication.Infrastructure.Domain.Entities.Picking.Order;
using Product = NewBloomersWebApplication.Infrastructure.Domain.Entities.Picking.Product;

namespace NewBloomersWebApplication.UI.Pages
{
    public partial class Picking
    {
        private string? doc_company;
        private string? serie_order;

        private string? nr_pedido { get; set; }
        private string? inputValueProduto { get; set; }

        private int inputValueVolumes { get; set; } = 1;

        private bool modalDataInvalida { get; set; }
        private bool modalConfirmacao { get; set; }
        private bool modalPedidoJaConferido { get; set; }
        private bool modalSeparacao { get; set; }
        private bool modalQuantidadeExcedida { get; set; }
        private bool modalQuantidadeFaltante { get; set; }
        private bool resultado { get; set; }

        private QuickGrid<Order> myGrid;
        private Order? pedido { get; set; }
        private List<Product>? itens { get; set; }
        private List<Order>? pedidos { get; set; }
        private PaginationState? pagination = new PaginationState { ItemsPerPage = 50 };
        private EventCallback<bool> OnClose { get; set; }

        protected override async Task OnInitializedAsync()
        {
            doc_company = await GetTextInLocalStorage("doc_company");
            serie_order = await GetTextInLocalStorage("serie_order");

            pedidos = await _conferenciaService.GetUnpickedOrders(doc_company, serie_order, DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"));

            foreach (var pedido in pedidos)
            {
                if (pedido.retorno != null)
                {
                    pedido.buttonText = "Conferido";
                    pedido.buttonClass = "btn btn-success";
                }
                else
                {
                    pedido.buttonText = "Conferir";
                    pedido.buttonClass = "btn btn-primary";
                }
            }
        }

        private async Task ReloadGrid(DateInterval dateInterval)
        {
            if (dateInterval.initialDate <= dateInterval.finalDate)
            {
                modalDataInvalida = false;
                pedidos = await _conferenciaService.GetUnpickedOrders(doc_company, serie_order, dateInterval.initialDate.ToString("yyyy-MM-dd"), dateInterval.finalDate.ToString("yyyy-MM-dd"));

                foreach (var pedido in pedidos)
                {
                    if (pedido.retorno != null)
                    {
                        pedido.buttonText = "Conferido";
                        pedido.buttonClass = "btn btn-success";
                    }
                    else
                    {
                        pedido.buttonText = "Conferir";
                        pedido.buttonClass = "btn btn-primary";
                    }
                }
            }
            else
            {
                modalDataInvalida = true;
            }
        }

        private async Task ConferePedidoIndividualButton(string nr_pedido)
        {
            this.nr_pedido = nr_pedido.ToUpper().Trim();

            if (pedidos.Where(p => p.number == this.nr_pedido).Count() > 0)
            {
                pedido = pedidos.Where(p => p.number == this.nr_pedido).First();

                if (pedido.volumes == 0 || pedido.retorno == null)
                {
                    itens = pedido.itens.ToList();

                    modalSeparacao = true;
                    await OnClose.InvokeAsync(true);
                }
                else
                {
                    modalPedidoJaConferido = true;
                }
            }
            else
            {
                pedido = await _conferenciaService.GetUnpickedOrder(doc_company, serie_order, this.nr_pedido.Trim());
                pedidos.Add(pedido);
                if (pedido.retorno != null)
                {
                    pedido.buttonText = "Conferido";
                    pedido.buttonClass = "btn btn-success";
                }
                else
                {
                    pedido.buttonText = "Conferir";
                    pedido.buttonClass = "btn btn-primary";
                }

                await myGrid.RefreshDataAsync();

                if (pedido.volumes == 0 || pedido.retorno == null)
                {
                    itens = pedido.itens.ToList();

                    modalSeparacao = true;
                    await OnClose.InvokeAsync(true);
                }
                else
                {
                    modalPedidoJaConferido = true;
                    itens = pedido.itens.ToList();
                }
            }
        }

        private async Task ConferePedidoIndividualEnter(Enter evento)
        {
            if (evento.e.Code == "Enter" || evento.e.Code == "NumpadEnter")
            {
                this.nr_pedido = evento.orderNumber;

                if (pedidos.Where(p => p.number == this.nr_pedido).Count() > 0)
                {
                    pedido = pedidos.Where(p => p.number == this.nr_pedido).First();

                    if (pedido.volumes == 0 || pedido.retorno == null)
                    {
                        itens = pedido.itens.ToList();

                        modalSeparacao = true;
                        await OnClose.InvokeAsync(true);
                    }
                    else
                    {
                        modalPedidoJaConferido = true;
                    }
                }
                else
                {
                    pedido = await _conferenciaService.GetUnpickedOrder(doc_company, serie_order, this.nr_pedido.Trim());
                    pedidos.Add(pedido);
                    if (pedido.retorno != null)
                    {
                        pedido.buttonText = "Conferido";
                        pedido.buttonClass = "btn btn-success";
                    }
                    else
                    {
                        pedido.buttonText = "Conferir";
                        pedido.buttonClass = "btn btn-primary";
                    }

                    await myGrid.RefreshDataAsync();

                    if (pedido.volumes == 0 || pedido.retorno == null)
                    {
                        itens = pedido.itens.ToList();

                        modalSeparacao = true;
                        await OnClose.InvokeAsync(true);
                    }
                    else
                    {
                        modalPedidoJaConferido = true;
                        itens = pedido.itens.ToList();
                    }
                }
            }
        }

        private async Task ConferePedidos()
        {

        }

        private void BtnSim()
        {
            modalPedidoJaConferido = false;
            modalSeparacao = true;
        }

        private async Task BtnContinuar()
        {
            var item = pedidos.Where(p => p.number == this.nr_pedido).First().itens.Where(p => p.cod_product == Convert.ToInt32(inputValueProduto)).First();

            item.picked_quantity = item.picked_quantity + 1;
            Thread.Sleep(2 * 1000);
            inputValueProduto = "";

            modalQuantidadeExcedida = false;
            await OnClose.InvokeAsync(true);
        }

        private async Task BtnFinalizar()
        {
            var pedido = pedidos.Where(p => p.number == this.nr_pedido).First();

            if (pedido.itens.Where(p => p.picked_quantity < p.quantity_product).Count() > 0)
            {
                modalQuantidadeFaltante = true;
                await OnClose.InvokeAsync(true);
            }
            else
            {
                pedido.volumes = inputValueVolumes;
                var result = await _conferenciaService.UpdateRetorno(pedido);

                if (result)
                {
                    resultado = true;
                    modalConfirmacao = true;
                    await OnClose.InvokeAsync(true);
                }
                else
                {
                    resultado = false;
                    modalConfirmacao = true;
                    await OnClose.InvokeAsync(true);
                }

                modalSeparacao = true;
                await OnClose.InvokeAsync(true);
            }
        }

        private Task BtnConferir(Order pedido)
        {
            this.nr_pedido = pedido.number;
            itens = pedido.itens.ToList();

            modalSeparacao = true;
            return OnClose.InvokeAsync(true);
        }

        private async Task BtnImprimir(Order pedido)
        {
            try
            {
                var base64String = await _conferenciaService.PrintCoupon(doc_company, serie_order, pedido.number);
                await jsRuntime.InvokeVoidAsync("downloadFile", "application/pdf", base64String, pedido.number);
            }
            catch (Exception ex)
            {
                throw;
            }        
        }

        private async void Enter(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                var item = pedidos.Where(p => p.number == this.nr_pedido).First().itens.Where(p => p.cod_product == Convert.ToInt32(inputValueProduto)).First();
                
                if (item.picked_quantity >= item.quantity_product)
                {
                    modalQuantidadeExcedida = true;
                    await OnClose.InvokeAsync(true);
                }
                else
                {
                    item.picked_quantity = item.picked_quantity + 1;
                    Thread.Sleep(2 * 1000);
                    inputValueProduto = "";
                }
            }
        }

        private void RemoveQtde()
        {
            var pedido = pedidos.Where(p => p.number == this.nr_pedido).First();
            var item = pedido.itens.Where(p => p.cod_product == Convert.ToInt32(inputValueProduto)).First();
            item.picked_quantity = item.picked_quantity - 1;
            Thread.Sleep(2 * 1000);
            inputValueProduto = "";
        }

        private async Task<string> GetTextInLocalStorage(string key)
        {
            return await jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
        }
    }
}
