namespace NewBloomersWebApplication.Infrastructure.Domain.Entities.Attendance
{
    public class Order : BloomersIntegrationsCore.Domain.Entities.Order
    {
        private List<ProductToContact> _itens = new List<ProductToContact>();

        public string? buttonText { get; set; }
        public string? buttonClass { get; set; }
        public string? contacted { get; set; }

        public List<ProductToContact> itens { get { return _itens; } set { _itens = value; } }
    }
}
