using System.ComponentModel.DataAnnotations;

namespace BloomersIntegrationsManager.Domain.Entities.Request
{
    public class LinxCommerceOrderRequest
    {
        [Required(ErrorMessage = "O Campo Código Pedido é Obrigatório")]
        public string? orderNumber { get; set; }
    }
}
