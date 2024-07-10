using System.ComponentModel.DataAnnotations;

namespace BloomersIntegrationsManager.Domain.Entities.MiniWms
{
    public class UpdateDateContactedRequest
    {
        [Required(ErrorMessage = "O Campo Código Pedido é Obrigatório")]
        public string? number { get; set; }

        [Required(ErrorMessage = "O Campo Nome Atendente é Obrigatório")]
        public string? atendente { get; set; }

        public string? obs { get; set; }
    }
}
