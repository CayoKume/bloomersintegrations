namespace BloomersMicrovixIntegrations.Domain.Entities.Ecommerce
{
    public class B2CConsultaPedidosStatus
    {
        public DateTime lastupdateon { get; set; }
        public long id { get; set; }
        public int id_status { get; set; }
        public int id_pedido { get; set; }
        public DateTime data_hora { get; set; }
        public string? anotacao { get; set; }
        public long timestamp { get; set; }
        public int portal { get; set; }
    }
}
