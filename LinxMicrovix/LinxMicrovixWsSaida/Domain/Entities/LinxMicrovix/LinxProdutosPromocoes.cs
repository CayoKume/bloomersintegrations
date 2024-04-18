namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix
{
    public class LinxProdutosPromocoes
    {
        public DateTime lastupdateon { get; set; }
        public string? portal { get; set; }
        public string? cnpj_emp { get; set; }
        public string? cod_produto { get; set; }
        public string? preco_promocao { get; set; }
        public string? data_inicio_promocao { get; set; }
        public string? data_termino_promocao { get; set; }
        public string? data_cadastro_promocao { get; set; }
        public string? promocao_ativa { get; set; }
        public string? id_campanha { get; set; }
        public string? nome_campanha { get; set; }
        public string? promocao_opcional { get; set; }
        public string? custo_total_campanha { get; set; }
    }
}
