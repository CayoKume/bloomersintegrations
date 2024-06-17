using Microsoft.JSInterop;
using NewBloomersWebApplication.Domain.Entities.Home;

namespace NewBloomersWebApplication.UI.Pages
{
    public partial class Home
    {
        private string doc_company { get; set; }
        private string name_company { get; set; }
        private List<Order>? orders { get; set; } = new List<Order>();

        protected override async Task OnInitializedAsync()
        {
            doc_company = await GetText("doc_company");
            name_company = await GetText("name_company");
            orders = await _homeService.GetPickupOrders(doc_company);
        }

        private async Task<string> GetText(string key)
        {
            return await jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
        }
    }
}
