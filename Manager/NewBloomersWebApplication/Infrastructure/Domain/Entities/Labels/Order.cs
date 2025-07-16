namespace NewBloomersWebApplication.Infrastructure.Domain.Entities.Labels
{
    public class Order : BloomersIntegrationsCore.Domain.Entities.Order
    {
        private List<string> _awb = new List<string>();
        private List<string> _zpl = new List<string>();
        private List<byte[]> _requests = new List<byte[]>();

        public bool buttonDisabled { get; set; }
        public string? buttonText { get; set; }
        public string? buttonClass { get; set; }
        public string? buttonPresentText { get; set; }
        public string? buttonPresentClass { get; set; }
        public string? nProt { get; set; }
        public string? tpNF { get; set; }
        public string? complementAddressStore { get; set; }
        public string? returnShippingCompany { get; set; }
        public string? roteShippingCompany { get; set; }
        public string? printed { get; set; }
        public string? present { get; set; }
        public string? token { get; set; }
        public string? serie { get; set; }

        public DateTime dateProt { get; set; }

        public List<string> awb { get { return _awb; } set { _awb = value; } }
        public List<string> zpl { get { return _zpl; } set { _zpl = value; } }
        public List<byte[]> requests { get { return _requests; } set { _requests = value; } }

    }
}
