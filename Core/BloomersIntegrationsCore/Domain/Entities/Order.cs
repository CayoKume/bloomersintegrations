namespace BloomersIntegrationsCore.Domain.Entities;

public class Order
{
    public string? number { get; set; }
    public string? cfop { get; set; }
    public int volumes { get; set; }
    private List<Product> _itens = new List<Product>();

    public Client? client { get; set; }
    public Company? company { get; set; }
    public ShippingCompany? shippingCompany { get; set; }
    public Invoice? invoice { get; set; }
    public List<Product> itens { get { return _itens; } set { _itens = value; } }
}
