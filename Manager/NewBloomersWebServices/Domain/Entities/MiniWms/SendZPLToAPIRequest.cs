using System.ComponentModel.DataAnnotations;

namespace BloomersIntegrationsManager.Domain.Entities.MiniWms
{
    public class SendZPLToAPIRequest
    {
        [Required(ErrorMessage = "O Campo ZPL é Obrigatório")]
        public string? zpl { get; set; }

        [Required(ErrorMessage = "O Campo Código Pedido é Obrigatório")]
        public string? nr_pedido { get; set; }

        [Required(ErrorMessage = "O Campo Volumes é Obrigatório")]
        [MinLength(1)]
        [MaxLength(1)]
        public string? volumes { get; set; }
    }
}
