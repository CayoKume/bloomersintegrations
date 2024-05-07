using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace NewBloomersWebApplication.UI.Components
{
    public partial class ConfirmationModalWithFocusById
    {
        [Parameter]
        public string? title { get; set; }
        [Parameter]
        public string? size { get; set; }
        [Parameter]
        public string? closeButtonText { get; set; }
        [Parameter]
        public string? clickButtonText { get; set; }
        [Parameter]
        public bool opened { get; set; }
        [Parameter]
        public RenderFragment? BodyContent { get; set; }
        [Parameter]
        public EventCallback OnClickEvent { get; set; }
        [Parameter]
        public EventCallback OnCloseEvent { get; set; }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await jsRuntime.InvokeVoidAsync("focusById", "inputSeparaProdutos");
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task InvokeOnClickEvent()
        {
            opened = false;
            await OnClickEvent.InvokeAsync();
        }

        private async Task InvokeOnCloseEvent()
        {
            opened = false;
            await OnCloseEvent.InvokeAsync();
        }
    }
}
