using Microsoft.JSInterop;
using static NewBloomersWebApplication.Domain.Entities.AppContext;

namespace NewBloomersWebApplication.UI.Pages
{
    public partial class Companys
    {
        private string? inputValueEmpresas { get; set; } = "1";
        private List<BloomersIntegrationsCore.Domain.Entities.Company>? empresas { get; set; } = new List<BloomersIntegrationsCore.Domain.Entities.Company>();

        protected override async Task OnInitializedAsync()
        {
            empresas = await _empresasService.GetCompanies();
        }

        private async Task selecionarEmpresa()
        {
            try
            {
                var empresa = empresas.Where(e => e.cod_company == inputValueEmpresas).First();
                await SaveTextInLocalStorage("cod_company", empresa.cod_company);
                await SaveTextInLocalStorage("name_company", empresa.name_company);
                await SaveTextInLocalStorage("doc_company", empresa.doc_company);

                if (empresa.name_company.ToUpper().Contains("MISHA"))
                    await SaveTextInLocalStorage("serie_order", "MI-");

                else if (empresa.name_company.ToUpper().Contains("OPEN ERA"))
                    await SaveTextInLocalStorage("serie_order", "OA-");

                NavigationManager.NavigateTo("Home");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task SaveTextInLocalStorage(string key, string value)
        {
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
        }
    }
}
