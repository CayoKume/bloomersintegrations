using BloomersWebAplication.Models.Base.AppContext;

namespace BloomersWebAplication.Layout
{
    public partial class MainLayout
    {
        private string empresa = Empresa.razao_empresa;

        private bool collapseNavMenu = true;
        private bool expandUsuario;

        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }
    }
}
