using BloomersIntegrationsCore.Domain.Entities;

namespace BloomersWorkers.LabelsPrinter.Domain.Entities
{
    public class Order : BloomersIntegrationsCore.Domain.Entities.Order
    {
        private List<string> _awb = new List<string>();

        private List<string> _zpl = new List<string>();
        public string delivery_to { get; set; }
        public string complement_address_store { get; set; }
        public string nProt { get; set; }
        public string tpNF { get; set; }
        public string _return { get; set; }
        public string rote { get; set; }
        public string serie { get; set; }
        public DateTime dateProt { get; set; }

        public Client client { get; set; }
        public Company company { get; set; }
        public ShippingCompany shippingCompany { get; set; }
        public Invoice invoice { get; set; }

        public List<string> awb { get { return _awb; } set { _awb = value; } }
        public List<string> zpl { get { return _zpl; } set { _zpl = value; } }
    }
}
