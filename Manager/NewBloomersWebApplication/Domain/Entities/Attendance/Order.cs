namespace NewBloomersWebApplication.Domain.Entities.Attendance
{
    public class Order : BloomersIntegrationsCore.Domain.Entities.Order
    {
        private List<NewBloomersWebApplication.Domain.Entities.Attendance.ProductToContact> _itens = new List<NewBloomersWebApplication.Domain.Entities.Attendance.ProductToContact>();

        public string? buttonText { get; set; }
        public string? buttonClass { get; set; }
        public string? contacted { get; set; }

        public List<NewBloomersWebApplication.Domain.Entities.Attendance.ProductToContact> itens { get { return _itens; } set { _itens = value; } }
    }
}
