namespace BloomersMicrovixIntegrations.Domain.Entities.Ecommerce
{
    public class B2CConsultaPedidos
    {
        public DateTime lastupdateon { get; set; }
        public int id_pedido { get; set; }
        public DateTime dt_pedido { get; set; }
        public int cod_cliente_erp { get; set; }
        public int cod_cliente_b2c { get; set; }
        public decimal vl_frete { get; set; }
        public int forma_pgto { get; set; }
        public int plano_pagamento { get; set; }
        public string? anotacao { get; set; }
        public decimal taxa_impressao { get; set; }
        public bool finalizado { get; set; }
        public decimal valor_frete_gratis { get; set; }
        public int tipo_frete { get; set; }
        public int id_status { get; set; }
        public int cod_transportador { get; set; }
        public int tipo_cobranca_frete { get; set; }
        public bool ativo { get; set; }
        public int empresa { get; set; }
        public int id_tabela_preco { get; set; }
        public decimal valor_credito { get; set; }
        public int cod_vendedor { get; set; }
        public long timestamp { get; set; }
        public DateTime dt_insert { get; set; }
        public DateTime dt_disponivel_faturamento { get; set; }
        public int portal { get; set; }
        public string? mensagem_falha_faturamento { get; set; }
        public int id_tipo_b2c { get; set; }
        public string? ecommerce_origem { get; set; }
        public string? order_id { get; set; }
        public string? fulfillment_id { get; set; }
    }
}
