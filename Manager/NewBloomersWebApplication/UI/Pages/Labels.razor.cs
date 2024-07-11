using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.JSInterop;
using NewBloomersWebApplication.Domain.Entities.Labels;
using static NewBloomersWebApplication.Domain.Entities.AppContext;
using DateInterval = NewBloomersWebApplication.Domain.Entities.AppContext.DateInterval;

namespace NewBloomersWebApplication.UI.Pages
{
    public partial class Labels
    {
        private string? doc_company;
        private string? serie_order;

        private string? nr_pedido { get; set; }

        private bool modalDataInvalida { get; set; }
        private bool modalNotaFiscalPendente { get; set; }

        private IEnumerable<Order>? pedidos { get; set; }
        private PaginationState pagination = new PaginationState { ItemsPerPage = 50 };

        protected override async Task OnInitializedAsync()
        {
            doc_company = await GetTextInLocalStorage("doc_company");
            serie_order = await GetTextInLocalStorage("serie_order");

            pedidos = await _etiquetasService.GetOrders(doc_company, serie_order, DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"));

            foreach (var pedido in pedidos)
            {
                if (pedido.printed == "S")
                {
                    pedido.buttonText = "Impresso";
                    pedido.buttonClass = "btn btn-success";
                }
                else
                {
                    pedido.buttonText = "Imprimir";
                    pedido.buttonClass = "btn btn-primary";
                }

                if (pedido.present == "S")
                {
                    pedido.buttonDisabled = false;
                    pedido.buttonPresentText = "Imprimir";
                    pedido.buttonPresentClass = "btn btn-primary";
                }
                else
                {
                    pedido.buttonDisabled = true;
                    pedido.buttonPresentText = "Imprimir";
                    pedido.buttonPresentClass = "btn btn-secondary";
                }
            }
        }

        private async Task ReloadGrid(DateInterval dateInterval)
        {
            if (dateInterval.initialDate <= dateInterval.finalDate)
            {
                modalDataInvalida = false;
                pedidos = await _etiquetasService.GetOrders(doc_company, serie_order, dateInterval.initialDate.ToString("yyyy-MM-dd"), dateInterval.finalDate.ToString("yyyy-MM-dd"));

                foreach (var pedido in pedidos)
                {
                    if (pedido.printed == "S")
                    {
                        pedido.buttonText = "Impresso";
                        pedido.buttonClass = "btn btn-success";
                    }
                    else
                    {
                        pedido.buttonText = "Imprimir";
                        pedido.buttonClass = "btn btn-primary";
                    }

                    if (pedido.present == "S")
                    {
                        pedido.buttonDisabled = false;
                        pedido.buttonPresentText = "Imprimir";
                        pedido.buttonPresentClass = "btn btn-primary";
                    }
                    else
                    {
                        pedido.buttonDisabled = true;
                        pedido.buttonPresentText = "Imprimir";
                        pedido.buttonPresentClass = "btn btn-secondary";
                    }
                }
            }
            else
            {
                modalDataInvalida = true;
            }
        }

        private async Task ImprimeCompTroca(Order pedido)
        {
            try
            {
                var base64String = await _etiquetasService.PrintCoupon(doc_company, serie_order, pedido.number);
                await jsRuntime.InvokeVoidAsync("downloadFile", "application/pdf", base64String, pedido.number);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task ImprimeEtiqueta(Order pedido)
        {
            await _etiquetasService.PrintLabels(pedido);

            for (int i = 0; i < pedido.volumes + 1; i++)
            {
                var fileName = pedido.number + " - " + (i + 1) + ".pdf";
                var base64String = await _etiquetasService.GetLabelToPrint(fileName);

                await jsRuntime.InvokeVoidAsync("downloadFile", "application/pdf", base64String, fileName);
            }
        }

        private async Task ImprimeEtiquetaIndividualButton(string nr_pedido)
        {
            this.nr_pedido = nr_pedido;
            modalNotaFiscalPendente = false;
            var pedido = await _etiquetasService.PrintLabel(doc_company, serie_order, this.nr_pedido);

            if (pedido.invoice is not null)
            {
                for (int i = 0; i < pedido.volumes + 1; i++)
                {
                    var fileName = pedido.number + " - " + (i + 1) + ".pdf";
                    var base64String = await _etiquetasService.GetLabelToPrint(fileName);

                    await jsRuntime.InvokeVoidAsync("downloadFile", "application/pdf", base64String, fileName);
                }
                await _etiquetasService.UpdateFlagPrinted(nr_pedido);
            }
            else
            {
                modalNotaFiscalPendente = true;
            }
        }

        private async Task ImprimeEtiquetaIndividualEnter(Enter evento)
        {
            if (evento.e.Code == "Enter" || evento.e.Code == "NumpadEnter")
            {
                this.nr_pedido = evento.orderNumber;
                modalNotaFiscalPendente = false;
                var pedido = await _etiquetasService.PrintLabel(doc_company, serie_order, this.nr_pedido);

                if (pedido.invoice is not null)
                {
                    for (int i = 0; i < pedido.volumes + 1; i++)
                    {
                        var fileName = pedido.number + " - " + (i + 1) + ".pdf";
                        var base64String = await _etiquetasService.GetLabelToPrint(fileName);

                        await jsRuntime.InvokeVoidAsync("downloadFile", "application/pdf", base64String, fileName);
                    }
                    await _etiquetasService.UpdateFlagPrinted(this.nr_pedido);
                }
                else
                {
                    modalNotaFiscalPendente = true;
                }
            }
        }

        private async Task<string> GetTextInLocalStorage(string key)
        {
            return await jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
        }
    }
}
