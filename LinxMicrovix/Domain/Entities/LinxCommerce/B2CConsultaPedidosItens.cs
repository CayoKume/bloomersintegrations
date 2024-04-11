namespace BloomersMicrovixIntegrations.Domain.Entities.Ecommerce
{
    public class B2CConsultaPedidosItens
    {
        public DateTime lastupdateon { get; set; }
        public int id_pedido_item { get; set; }
        public int id_pedido { get; set; }
        public long codigoproduto { get; set; }
        public int quantidade { get; set; }
        public decimal vl_unitario { get; set; }
        public long timestamp { get; set; }
        public int portal { get; set; }
    }
}
