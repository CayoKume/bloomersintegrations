namespace NewBloomersWebApplication.UI.Layouts
{
    public partial class SideMenuLayout
    {
        private bool collapseNavMenu = true;
        private bool expandMenuExpedicao;
        private bool expandMenuFinanceiro;
        private bool expandMenuSolicitacoes;

        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }
    }
}
