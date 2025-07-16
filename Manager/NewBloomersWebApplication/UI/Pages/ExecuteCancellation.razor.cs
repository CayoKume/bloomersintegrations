using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.JSInterop;
using NewBloomersWebApplication.Infrastructure.Domain.Entities.ExecuteCancellation;

namespace NewBloomersWebApplication.UI.Pages
{
    partial class ExecuteCancellation
    {
        private string? doc_company;
        private string? serie_order;

        private string? inputObs { get; set; }
        private string? inputValueRequester { get; set; }
        private int inputValueReason { get; set; } = 1;

        private bool modalSuporte { get; set; }
        private bool modalPedido { get; set; }
        private bool modalSucesso { get; set; }

        private Dictionary<int, string> reasons { get; set; } = new Dictionary<int, string>();
        private Order order { get; set; }
        private List<Order> orders { get; set; } = new List<Order>();
        private List<ProductToExecuteCancellation> productToCancellations { get; set; } = new List<ProductToExecuteCancellation>();
        private PaginationState? pagination = new PaginationState { ItemsPerPage = 50 };
        private QuickGrid<Order> myGrid;

        protected override async Task OnInitializedAsync()
        {
            doc_company = await GetTextInLocalStorage("doc_company");
            serie_order = await GetTextInLocalStorage("serie_order");

            reasons = await _executeCancellationService.GetReasons();
            orders = await _executeCancellationService.GetOrdersToCancel(serie_order, doc_company);

            foreach (var order in orders)
            {
                if (order.canceled.ToLower() == "cancelado")
                {
                    order.buttonText = "Cancelado";
                    order.buttonClass = "btn btn-success";
                }
                else
                {
                    order.buttonText = "Cancelar";
                    order.buttonClass = "btn btn-primary";
                }
            }
        }

        private async Task<string> GetTextInLocalStorage(string key)
        {
            return await jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
        }

        private void SelectOrder(Order order)
        {
            this.order = order;
        }

        private async Task CancelarPedido()
        {
            try
            {
                if (!String.IsNullOrEmpty(inputValueRequester))
                {
                    if (this.order is not null)
                    {
                        var result = await _executeCancellationService.UpdateDateCanceled(this.order.number, inputValueRequester, inputObs, inputValueReason);

                        if (result)
                            modalSucesso = true;
                        else
                            modalSucesso = false;
                    }
                    else
                        modalPedido = true;
                }
                else
                    modalSuporte = true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
