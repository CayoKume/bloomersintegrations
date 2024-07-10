namespace BloomersMiniWmsIntegrations.Domain.Entities.Attendence
{
    public class Order : BloomersIntegrationsCore.Domain.Entities.Order
    {
        private List<BloomersMiniWmsIntegrations.Domain.Entities.Attendence.ProductToContact> _itens = new List<BloomersMiniWmsIntegrations.Domain.Entities.Attendence.ProductToContact>();

        public string? contacted { get; set; }

        public List<BloomersMiniWmsIntegrations.Domain.Entities.Attendence.ProductToContact> itens { get { return _itens; } set { _itens = value; } }
    }
}
