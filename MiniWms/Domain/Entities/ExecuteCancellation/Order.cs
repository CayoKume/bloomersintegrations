namespace BloomersMiniWmsIntegrations.Domain.Entities.ExecuteCancellation
{
    public class Order : BloomersIntegrationsCore.Domain.Entities.Order
    {
        private List<BloomersMiniWmsIntegrations.Domain.Entities.ExecuteCancellation.ProductToExecuteCancellation> _itens = new List<BloomersMiniWmsIntegrations.Domain.Entities.ExecuteCancellation.ProductToExecuteCancellation>();

        public string suport { get; set; }
        public int reason { get; set; }
        public string obs { get; set; }
        public string? canceled { get; set; }
        public string? id_motivo { get; set; }
        public string? descricao_motivo { get; set; }

        public List<BloomersMiniWmsIntegrations.Domain.Entities.ExecuteCancellation.ProductToExecuteCancellation> itens { get { return _itens; } set { _itens = value; } }
    }
}
