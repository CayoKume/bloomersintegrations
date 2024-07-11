using System.ComponentModel.DataAnnotations;

namespace BloomersIntegrationsManager.Domain.Entities.MiniWms
{
    public class UpdateFlagPrintedRequest
    {
        [Required(ErrorMessage = "O Campo Código Pedido é Obrigatório")]
        public string? number { get; set; }
    }
}
