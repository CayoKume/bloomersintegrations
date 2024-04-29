using Microsoft.AspNetCore.Components;

namespace NewBloomersWebApplication.UI.Components
{
    public partial class AlertModal
    {
        [Parameter]
        public string? Title { get; set; }
        [Parameter]
        public string? Text { get; set; }
        [Parameter]
        public EventCallback OnClickEvent { get; set; }
        [Parameter]
        public EventCallback<bool> OnCloseEvent { get; set; }

        private Task InvokeOnCloseEvent()
        {
            return OnCloseEvent.InvokeAsync(false);
        }

        private Task InvokeOnClickEvent()
        {
            return OnClickEvent.InvokeAsync();
        }
    }
}
