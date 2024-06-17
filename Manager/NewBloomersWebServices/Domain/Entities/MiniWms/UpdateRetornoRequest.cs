namespace BloomersIntegrationsManager.Domain.Entities.MiniWms
{
    public class UpdateRetornoRequest
    {
        private List<BloomersMiniWmsIntegrations.Domain.Entities.Picking.Product> _itens = new List<BloomersMiniWmsIntegrations.Domain.Entities.Picking.Product>();

        public string? nr_pedido { get; set; }

        public int volumes { get; set; }

        public List<BloomersMiniWmsIntegrations.Domain.Entities.Picking.Product> itens { get { return _itens; } set { _itens = value; } }
    }
}
