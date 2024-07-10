using System.ComponentModel.DataAnnotations;

namespace BloomersIntegrationsManager.Domain.Entities.MiniWms
{
    public class UpdateDateCanceledRequest
    {
        [Required(ErrorMessage = "O Campo Código Pedido é Obrigatório")]
        public string? number { get; set; }

        [Required(ErrorMessage = "O Campo Nome Suporte é Obrigatório")]
        public string? suporte { get; set; }

        [Required(ErrorMessage = "O Campo Id Motivo e é Obrigatório")]
        public int motivo { get; set; }

        public string? obs { get; set; }
    }
}
