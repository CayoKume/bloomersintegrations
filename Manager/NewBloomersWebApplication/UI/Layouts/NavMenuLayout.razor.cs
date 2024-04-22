namespace BloomersWebAplication.Layout
{
    public partial class NavMenuLayout
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
