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

        private void selecionarEmpresa()
        {
            try
            {
                var empresa = empresas.Where(e => e.cod_company == inputValueEmpresas).First();
                Company.cod_company = Convert.ToInt32(empresa.cod_company);
                Company.reason_company = empresa.name_company;
                Company.doc_company = empresa.doc_company;

                if (empresa.name_company.ToUpper().Contains("MISHA"))
                    Company.serie_order = "MI-";
                else if (empresa.name_company.ToUpper().Contains("OPEN ERA"))
                    Company.serie_order = "OA-";

                NavigationManager.NavigateTo("Home");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
