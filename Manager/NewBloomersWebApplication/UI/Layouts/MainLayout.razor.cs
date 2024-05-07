using Microsoft.JSInterop;

namespace NewBloomersWebApplication.UI.Layouts
{
    public partial class MainLayout
    {
        private string empresa;

        private bool collapseNavMenu = true;
        public bool expandUsuario;

        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        protected override async Task OnInitializedAsync()
        {
            empresa = await GetTextInLocalStorage("name_company");
        }

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }

        private async Task<string> GetTextInLocalStorage(string key)
        {
            return await jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
        }
    }
}
