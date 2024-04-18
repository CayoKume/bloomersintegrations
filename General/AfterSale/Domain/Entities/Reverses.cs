namespace BloomersGeneralIntegrations.AfterSale.Domain.Entities;

public class Reverses
{
    public int id { get; set; }
    public string reverse_type { get; set; }
    public string reverse_type_name { get; set; }
    public string created_at { get; set; }
    public string updated_at { get; set; }
    public string order_id { get; set; }
    public string total_amount { get; set; }
    public string returned_invoice { get; set; }
    public Customer customer { get; set; }
    public StatusReverse status { get; set; }
    public Tracking tracking { get; set; }
    public int refunds_count { get; set; }
}
