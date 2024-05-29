using BloomersMiniWmsIntegrations.Domain.Entities.Picking;
using System.ComponentModel.DataAnnotations;

namespace BloomersIntegrationsManager.Domain.Entities.MiniWms
{
    public class UpdateRetornoRequest
    {
        private List<Product> _itens = new List<Product>();

        [Required(ErrorMessage = "O Campo Código Pedido é Obrigatório")]
        public string? nr_pedido { get; set; }

        [Required(ErrorMessage = "O Campo Volumes é Obrigatório")]
        public int volumes { get; set; }

        [Required(ErrorMessage = "A Lista de Pedidos é Obrigatória")]
        public List<Product> itens { get { return _itens; } set { _itens = value; } }
    }
}
