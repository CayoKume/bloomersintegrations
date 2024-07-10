using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NewBloomersWebApplication.Domain.Entities.Attendance;

namespace NewBloomersWebApplication.UI.Pages
{
    partial class Attendance
    {
        private string? doc_company;
        private string? serie_order;

        private string? number;
        private string? name_client;
        private string? email_client;
        private string? fone_client;
        private string? inputObs;
        private string? atendente;

        private bool modalContato;
        private bool modalAtendente;
        private bool modalSucesso;
        private bool modalErro;

        private List<NewBloomersWebApplication.Domain.Entities.Attendance.Order> orders { get; set; } = new List<NewBloomersWebApplication.Domain.Entities.Attendance.Order>();
        private List<NewBloomersWebApplication.Domain.Entities.Attendance.ProductToContact> itens { get; set; } = new List<NewBloomersWebApplication.Domain.Entities.Attendance.ProductToContact>();
        private QuickGrid<NewBloomersWebApplication.Domain.Entities.Attendance.Order> myGrid;
        private PaginationState? pagination = new PaginationState { ItemsPerPage = 50 };
        private EventCallback<bool> OnClose { get; set; }

        protected override async Task OnInitializedAsync()
        {
            doc_company = await GetTextInLocalStorage("doc_company");
            serie_order = await GetTextInLocalStorage("serie_order");

            orders = await _attendanceService.GetOrdersToContact(serie_order, doc_company);

            foreach (var order in orders)
            {
                if (order.contacted.ToLower() == "contatado")
                {
                    order.buttonText = "Contatado";
                    order.buttonClass = "btn btn-success";
                }
                else
                {
                    order.buttonText = "Contatar";
                    order.buttonClass = "btn btn-primary";
                }
            }
        }

        private async Task<string> GetTextInLocalStorage(string key)
        {
            return await jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
        }

        private async Task EntrarEmContato(Order order)
        {
            try
            {
                number = order.number;
                name_client = order.client.reason_client;
                email_client = order.client.email_client;
                fone_client = order.client.fone_client;
                itens = order.itens;

                modalContato = true;
                await OnClose.InvokeAsync(true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task BtnFinalizar()
        {
            try
            {
                if (!String.IsNullOrEmpty(atendente))
                {
                    var result = await _attendanceService.UpdateDateContacted(number, atendente, inputObs);

                    if (result)
                        modalSucesso = true;
                    else
                        modalErro = true;
                }
                else
                    modalAtendente = true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
