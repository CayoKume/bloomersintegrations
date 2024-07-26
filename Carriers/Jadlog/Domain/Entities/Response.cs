namespace BloomersCarriersIntegrations.Jadlog.Domain.Entities
{
    public class Response
    {
        public string codigo { get; set; }
        public string shipmentId { get; set; }
        public string status { get; set; }
        public Erro erro { get; set; }
    }

    public class Erro
    {
        public int id { get; set; }
        public string descricao { get; set; }
    }
}
