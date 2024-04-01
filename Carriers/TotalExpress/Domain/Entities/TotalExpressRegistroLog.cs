namespace BloomersCarriersIntegrations.TotalExpress.Domain.Entities
{
    public class TotalExpressRegistroLog
    {
        public string? pedido { get; set; }
        public string? descricao_ultimo_status { get; set; }
        public string? retorno { get; set; }
        public string? remetenteid { get; set; }
        public DateTime? previsao_real_entrega { get; set; }
        public DateTime? entrega_realizada { get; set; }
        public DateTime? data_coleta { get; set; }
    }
}
