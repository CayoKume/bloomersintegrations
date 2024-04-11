namespace BloomersMicrovixIntegrations.Domain.Entities.Ecommerce
{
    public class B2CConsultaClientes
    {
        public DateTime? lastupdateon { get; set; }
        public int? cod_cliente_b2c { get; set; }
        public int? cod_cliente_erp { get; set; }
        public string? doc_cliente { get; set; }
        public string? nm_cliente { get; set; }
        public string? nm_mae { get; set; }
        public string? nm_pai { get; set; }
        public string? nm_conjuge { get; set; }
        public DateTime? dt_cadastro { get; set; }
        public DateTime? dt_nasc_cliente { get; set; }
        public string? end_cliente { get; set; }
        public string? complemento_end_cliente { get; set; }
        public string? nr_rua_cliente { get; set; }
        public string? bairro_cliente { get; set; }
        public string? cep_cliente { get; set; }
        public string? cidade_cliente { get; set; }
        public char? uf_cliente { get; set; }
        public string? fone_cliente { get; set; }
        public string? fone_comercial { get; set; }
        public string? cel_cliente { get; set; }
        public string? email_cliente { get; set; }
        public string? rg_cliente { get; set; }
        public string? rg_orgao_emissor { get; set; }
        public int? estado_civil_cliente { get; set; }
        public string? empresa_cliente { get; set; }
        public string? cargo_cliente { get; set; }
        public char? sexo_cliente { get; set; }
        public DateTime? dt_update { get; set; }
        public bool? ativo { get; set; }
        public bool? receber_email { get; set; }
        public DateTime? dt_expedicao_rg { get; set; }
        public string? naturalidade { get; set; }
        public int? tempo_residencia { get; set; }
        public decimal? renda { get; set; }
        public string? numero_compl_rua_cliente { get; set; }
        public long? timestamp { get; set; }
        public char? tipo_pessoa { get; set; }
        public int? portal { get; set; }
    }
}
