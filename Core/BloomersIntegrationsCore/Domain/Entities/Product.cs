namespace BloomersIntegrationsCore.Domain.Entities;

public class Product
{
    public int cod_product { get; set; }
    public double quantity_product { get; set; }
    public string? sku_product { get; set; }
    public string? description_product { get; set; }
    public string? cod_ean_product { get; set; }
    public string? unitary_value_product { get; set; }
    public string? amount_product { get; set; }
    public string? shipping_value_product { get; set; }

    public int CompareTo(ProdutoBase other)
    {
        if (cod_product > other.cod_product)
            return 1;
        else if (cod_product < other.cod_product)
            return -1;
        else
            return 0;
    }
}
