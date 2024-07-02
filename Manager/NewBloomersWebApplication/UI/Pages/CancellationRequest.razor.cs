using BloomersIntegrationsCore.Domain.Entities;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using NewBloomersWebApplication.Domain.Entities.CancellationRequest;

namespace NewBloomersWebApplication.UI.Pages
{
    partial class CancellationRequest
    {
        private string? orderNumber { get; set; }
        private string? display { get; set; } = "none";
        private string? inputValueRequester { get; set; }
        private int inputValueReason { get; set; } = 1;
        private bool modalOrderNumberEmpty { get; set; }
        private Dictionary<int, string> reasons { get; set; } = new Dictionary<int, string>();
        private NewBloomersWebApplication.Domain.Entities.CancellationRequest.Order order { get; set; } = new NewBloomersWebApplication.Domain.Entities.CancellationRequest.Order();
        private List<NewBloomersWebApplication.Domain.Entities.CancellationRequest.ProductToCancellation> productToCancellations { get; set; } = new List<NewBloomersWebApplication.Domain.Entities.CancellationRequest.ProductToCancellation>();

        private QuickGrid<NewBloomersWebApplication.Domain.Entities.CancellationRequest.ProductToCancellation> myGrid;
        private PaginationState? pagination = new PaginationState { ItemsPerPage = 50 };

        protected override async Task OnInitializedAsync()
        {
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
                        order = await _cancellationRequestService.GetOrderToCancel(orderNumber);
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
                order.reason = inputValueReason;
                order.requester = inputValueRequester;
                foreach (var item in productToCancellations)
                {
                    order.itens.RemoveAll(p => p.cod_product != item.cod_product);
                }
                await _cancellationRequestService.CreateCancellationRequest(order);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
