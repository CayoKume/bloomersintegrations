using BloomersIntegrationsCore.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using NewBloomersWebApplication.Domain.Entities.CancellationRequest;

namespace NewBloomersWebApplication.UI.Pages
{
    partial class CancellationRequest
    {
        private string? doc_company;
        private string? serie_order;

        private string? orderNumber { get; set; }
        private string? display { get; set; } = "none";
        private string? inputValueRequester { get; set; }
        private int inputValueReason { get; set; } = 1;

        private bool resultado { get; set; }
        private bool modalOrderNumberEmpty { get; set; }
        private bool modalListProducts { get; set; }
        private bool modalConfirmacao { get; set; }
        private bool modalRequester { get; set; }

        private Dictionary<int, string> reasons { get; set; } = new Dictionary<int, string>();
        private NewBloomersWebApplication.Domain.Entities.CancellationRequest.Order order { get; set; } = new NewBloomersWebApplication.Domain.Entities.CancellationRequest.Order();
        private List<NewBloomersWebApplication.Domain.Entities.CancellationRequest.ProductToCancellation> productToCancellations { get; set; } = new List<NewBloomersWebApplication.Domain.Entities.CancellationRequest.ProductToCancellation>();

        private QuickGrid<NewBloomersWebApplication.Domain.Entities.CancellationRequest.ProductToCancellation> myGrid;
        private PaginationState? pagination = new PaginationState { ItemsPerPage = 50 };
        private EventCallback<bool> OnClose { get; set; }

        protected override async Task OnInitializedAsync()
        {
            doc_company = await GetTextInLocalStorage("doc_company");
            serie_order = await GetTextInLocalStorage("serie_order");

            reasons = await _cancellationRequestService.GetReasons();
        }

        private async Task Enter(KeyboardEventArgs e)
        {
            try
            {
                if (e.Code == "Enter" || e.Code == "NumpadEnter")
                {
                    if (!System.String.IsNullOrEmpty(orderNumber))
                    {
                        order = await _cancellationRequestService.GetOrderToCancel(orderNumber, serie_order, doc_company);
                        Thread.Sleep(2 * 1000);
                        display = "block";
                    }
                    else
                        modalOrderNumberEmpty = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task SelectProduct(ProductToCancellation product)
        {
            try
            {
                productToCancellations.Add(product);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task solicitarCancelamento()
        {
            try
            {
                if (inputValueRequester is not null)
                {
                    if (orderNumber is not null)
                    {
                        if (productToCancellations.Count() > 0)
                        {
                            order.reason = inputValueReason;
                            order.requester = inputValueRequester;
                            foreach (var item in productToCancellations)
                            {
                                order.itens.RemoveAll(p => p.cod_product != item.cod_product);
                            }
                            var result = await _cancellationRequestService.CreateCancellationRequest(order);

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
                        }
                        else
                            modalListProducts = true;
                    }
                    else
                        modalOrderNumberEmpty = true;
                }
                else
                    modalRequester = true;
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
