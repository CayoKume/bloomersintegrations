namespace BloomersCarriersIntegrations.TotalExpress.Domain.Entities
{
    public class Status
    {
        public string pedido { get; set; }
        public string id_cliente { get; set; }
        public string awb { get; set; }
        public string nfiscal { get; set; }
        public string nfiscalserie { get; set; }
        public string cod_barra { get; set; }
        public string rota { get; set; }

        public detalhes detalhes { get; set; }

        public string json { get; set; }
    }

    public class detalhes
    {
        public statusDeEncomenda[] statusDeEncomenda { get; set; }
        public dataPrev dataPrev { get; set; }
    }

    public class dataPrev
    {
        public string PrevEntrega { get; set; }
        public string PrevEntregaAtualizada { get; set; }
    }

    public class statusDeEncomenda
    {
        public string statusid { get; set; }
        public string status { get; set; }
        public string data { get; set; }
    }
}
