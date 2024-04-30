namespace BloomersMiniWmsIntegrations.Domain.Entities.Labels
{
    public class Order : BloomersIntegrationsCore.Domain.Entities.Order
    {
        private List<string> _awb = new List<string>();
        private List<string> _zpl = new List<string>();
        private List<byte[]> _requests = new List<byte[]>();

        public DateTime dateProt { get; set; }
        public string? nProt { get; set; }
        public string? tpNF { get; set; }
        public string? complemento_endereco_loja { get; set; }
        public string? retorno_transportadora { get; set; }
        public string? rota_transportadora { get; set; }
        public string? impresso { get; set; }
        public string? serie { get; set; }

        public List<string> awb { get { return _awb; } set { _awb = value; } }
        public List<string> zpl { get { return _zpl; } set { _zpl = value; } }
        public List<byte[]> requests { get { return _requests; } set { _requests = value; } }
    }
}
