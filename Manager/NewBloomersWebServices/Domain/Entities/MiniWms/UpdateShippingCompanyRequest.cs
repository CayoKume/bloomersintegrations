using System.ComponentModel.DataAnnotations;

namespace BloomersIntegrationsManager.Domain.Entities.MiniWms
{
    public class UpdateShippingCompanyRequest
    {
        [Required(ErrorMessage = "O Campo Código Pedido é Obrigatório")]
        public string? orderNumber { get; set; }

        [Required(ErrorMessage = "O Campo Volumes é Obrigatório")]
        public int cod_shippingCompany { get; set; }
    }
}
