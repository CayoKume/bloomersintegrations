using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace NewBloomersWebApplication.UI.Components
{
    public partial class Pagination
    {
        [Parameter]
        public PaginationState? pagination { get; set; }
    }
}
