using BloomersIntegrationsCore.Domain.Entities;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace NewBloomersWebApplication.UI.Pages
{
    partial class CancellationRequest
    {
        private string? orderNumber { get; set; }
        private string? inputValueReason { get; set; }
        private bool modalOrderNumberEmpty { get; set; }
        private Dictionary<int, string> reasons { get; set; } = new Dictionary<int, string>();

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
                        Thread.Sleep(2 * 1000);
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
    }
}
