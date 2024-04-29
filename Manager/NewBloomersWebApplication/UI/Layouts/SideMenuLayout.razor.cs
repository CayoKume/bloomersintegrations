namespace NewBloomersWebApplication.UI.Layouts
{
    public partial class SideMenuLayout
    {
        private bool collapseNavMenu = true;
        private bool expandMenu;

        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }
    }
}
