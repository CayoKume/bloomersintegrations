using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using NewBloomersWebApplication.Domain.Entities.Picking;
using static NewBloomersWebApplication.Domain.Entities.AppContext;

namespace NewBloomersWebApplication.UI.Pages
{
    public partial class ShippingCompanys
    {
        private string? doc_company;
        private string? serie_order;

        private bool modalConfirmationAlert { get; set; }
        private bool modalOrderNumberEmpty { get; set; }
        private bool modalUpdateIsSuccessful { get; set; }
        private bool modalUpdateIsFailed { get; set; }
        private string? inputValueShippCompanie { get; set; } = "1210";
        private string? orderNumber { get; set; }
        private string? shippingCompany { get; set; }
        private List<BloomersIntegrationsCore.Domain.Entities.ShippingCompany>? shippingCompanies { get; set; } = new List<BloomersIntegrationsCore.Domain.Entities.ShippingCompany>();

        protected override async Task OnInitializedAsync()
        {
            doc_company = await GetTextInLocalStorage("doc_company");
            serie_order = await GetTextInLocalStorage("serie_order");

            shippingCompanies = await _pickingService.GetShippingCompanys();
        }

        private async Task selecionarTransportadora()
        {
            try
            {
                if (!System.String.IsNullOrEmpty(orderNumber))
                {
                    var order = await _pickingService.GetUnpickedOrder(doc_company, serie_order, orderNumber);
                    shippingCompany = order.shippingCompany.cod_shippingCompany + " - " + order.shippingCompany.reason_shippingCompany;
                    modalConfirmationAlert = true;
                }
                else
                    modalOrderNumberEmpty = true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        private async Task UpdateShippingCompany()
        {
            modalConfirmationAlert = false;

            var shippingCompanie = shippingCompanies.First(e => e.cod_shippingCompany == inputValueShippCompanie);
            var result = await _pickingService.UpdateShippingCompany(orderNumber, Convert.ToInt32(shippingCompanie.cod_shippingCompany));

            if (result)
                modalUpdateIsSuccessful = true;
            else
                modalUpdateIsFailed = true;
        }

        private async Task Enter(KeyboardEventArgs e)
        {
            try
            {
                if (e.Code == "Enter" || e.Code == "NumpadEnter")
                {
                    if (!System.String.IsNullOrEmpty(orderNumber))
                    {
                        var order = await _pickingService.GetUnpickedOrder(doc_company, serie_order, orderNumber);
                        shippingCompany = order.shippingCompany.cod_shippingCompany + " - " + order.shippingCompany.reason_shippingCompany;
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

        private async Task<string> GetTextInLocalStorage(string key)
        {
            return await jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
        }
    }
}
