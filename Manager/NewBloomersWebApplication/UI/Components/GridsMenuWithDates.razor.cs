using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using static NewBloomersWebApplication.Domain.Entities.AppContext;

namespace NewBloomersWebApplication.UI.Components
{
    public partial class GridsMenuWithDates
    {
        [Parameter]
        public EventCallback<string> OnClickEvent { get; set; }

        [Parameter]
        public EventCallback<Enter> OnKeyUpEvent { get; set; }

        [Parameter]
        public EventCallback<DateInterval> OnChangeEvent { get; set; }

        [Parameter]
        public string? transportadora { get; set; }

        public string? nr_pedido { get; set; }
        public DateTime dataInicial { get; set; } = DateTime.Now.Date;
        public DateTime dataFinal { get; set; } = DateTime.Now.Date;

        private async Task InvokeOnClickEvent()
        {
            var pedido = this.nr_pedido.Trim().ToUpper();
            this.nr_pedido = String.Empty;

            await OnClickEvent.InvokeAsync(pedido);
        }

        private async Task InvokeOnKeyUpEvent(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                var pedido = this.nr_pedido.Trim().ToUpper();
                this.nr_pedido = String.Empty;

                await OnKeyUpEvent.InvokeAsync(new Enter { e = e, orderNumber = pedido });
            }
        }

        private async Task InvokeOnChangeEvent()
        {
            await OnChangeEvent.InvokeAsync(new DateInterval { finalDate = this.dataFinal, initialDate = this.dataInicial, shippingCompany = this.transportadora });
        }
    }
}
