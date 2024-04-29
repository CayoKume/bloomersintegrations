using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Web;
using static NewBloomersWebApplication.Domain.Entities.AppContext;
using Company = NewBloomersWebApplication.Domain.Entities.AppContext.Company;
using Order = NewBloomersWebApplication.Domain.Entities.Picking.Order;
using Product = NewBloomersWebApplication.Domain.Entities.Picking.Product;

namespace NewBloomersWebApplication.UI.Pages
{
    public partial class Picking
    {
        private string? nr_pedido { get; set; }
        private string? inputValueProduto { get; set; }

        private int inputValueVolumes { get; set; } = 1;

        private bool modalDataInvalida { get; set; }
        private bool modalConfirmacao { get; set; }
        private bool modalPedidoJaConferido { get; set; }
        private bool modalSeparacao { get; set; }
        private bool resultado { get; set; }

        private QuickGrid<Order> myGrid;
        private Order? pedido { get; set; }
        private List<Product>? itens { get; set; }
        private List<Order>? pedidos { get; set; }
        private PaginationState? pagination = new PaginationState { ItemsPerPage = 50 };
        private EventCallback<bool> OnClose { get; set; }

        protected override async Task OnInitializedAsync()
        {
            pedidos = await _conferenciaService.GetUnpickedOrders(Company.doc_company, Company.serie_order, DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"));

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
                pedidos = await _conferenciaService.GetUnpickedOrders(Company.doc_company, Company.serie_order, dateInterval.initialDate.ToString("yyyy-MM-dd"), dateInterval.finalDate.ToString("yyyy-MM-dd"));

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
                pedido = await _conferenciaService.GetUnpickedOrder(Company.doc_company, Company.serie_order, this.nr_pedido.Trim());
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
                    pedido = await _conferenciaService.GetUnpickedOrder(Company.doc_company, Company.serie_order, this.nr_pedido.Trim());
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

        private void BtnSim()
        {
            modalPedidoJaConferido = false;
            modalSeparacao = true;
        }

        private async Task BtnFinalizar()
        {
            var pedido = pedidos.Where(p => p.number == this.nr_pedido).First();
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

        private Task BtnConferir(Order pedido)
        {
            this.nr_pedido = pedido.number;
            itens = pedido.itens.ToList();

            modalSeparacao = true;
            return OnClose.InvokeAsync(true);
        }

        private void Enter(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                var item = pedidos.Where(p => p.number == this.nr_pedido).First().itens.Where(p => p.cod_product == Convert.ToInt32(inputValueProduto)).First();
                item.quantityPicked = item.quantityPicked + 1;
                Thread.Sleep(2 * 1000);
                inputValueProduto = "";
            }
        }

        private void RemoveQtde()
        {
            var pedido = pedidos.Where(p => p.number == this.nr_pedido).First();
            var item = pedido.itens.Where(p => p.cod_product == Convert.ToInt32(inputValueProduto)).First();
            item.quantity_product = item.quantity_product - 1;
            Thread.Sleep(2 * 1000);
            inputValueProduto = "";
        }

    }
}
