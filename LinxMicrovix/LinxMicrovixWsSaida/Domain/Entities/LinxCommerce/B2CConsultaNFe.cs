namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxEcommerce
{
    public class B2CConsultaNFe
    {
        public DateTime? lastupdateon { get; set; }
        public int? id_nfe { get; set; }
        public int? id_pedido { get; set; }
        public int? documento { get; set; }
        public DateTime data_emissao { get; set; }
        public string chave_nfe { get; set; }
        public int situacao { get; set; }
        public string xml { get; set; }
        public bool excluido { get; set; }
        public Guid identificador_microvix { get; set; }
        public DateTime dt_insert { get; set; }
        public decimal valor_nota { get; set; }
        public string serie { get; set; }
        public decimal frete { get; set; }
        public long timestamp { get; set; }
        public int portal { get; set; }
        public string nProt { get; set; }
        public string codigo_modelo_nf { get; set; }
        public string justificativa { get; set; }
    }
}
