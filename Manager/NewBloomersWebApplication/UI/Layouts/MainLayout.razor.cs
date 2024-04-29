using static NewBloomersWebApplication.Domain.Entities.AppContext;

namespace NewBloomersWebApplication.UI.Layouts
{
    public partial class MainLayout
    {
        public string empresa = Company.reason_company;

        private bool collapseNavMenu = true;
        public bool expandUsuario;

        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }
    }
}
