namespace BloomersGeneralIntegrations.AfterSale.Domain.Entities;

public class Page
{
    public int current_page { get; set; }
    public int last_page { get; set; }
    public List<Reverses> data { get; set; }
}
