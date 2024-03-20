namespace BloomersIntegrationsCore.Domain.Entities;

public class Invoice
{
    public string? number_nf { get; set; }
    public string? key_nfe_nf { get; set; }
    public string? xml_nf { get; set; }
    public string? xml_distribuition_nf { get; set; }
    public decimal amount_nf { get; set; }
    public decimal shipping_value_nf { get; set; }
    public decimal weight_nf { get; set; }
    public string? type_nf { get; } = "NF";
    public string? serie_nf { get; set; }
    public DateTime data_emission_nf { get; set; }
}
