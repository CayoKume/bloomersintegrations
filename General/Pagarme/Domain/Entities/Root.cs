using System.Net.NetworkInformation;

namespace BloomersGeneralIntegrations.Pagarme.Models
{
    public class Root
    {
        public List<Data> data { get; set; }
        public Paging paging { get; set; }
    }

    public class Data
    {
        public Int64 id { get; set; }
        public string? status { get; set; }
        public int amount { get; set; }
        public int fee { get; set; }
        public int anticipation_fee { get; set; }
        public int fraud_coverage_fee { get; set; }
        public int installment { get; set; }
        public Int64 gateway_id { get; set; }
        public string? split_id { get; set; }
        public string? charge_id { get; set; }
        public string? recipient_id { get; set; }
        public DateTime payment_date { get; set; }
        public string? type { get; set; }
        public string? payment_method { get; set; }
        public DateTime accrual_at { get; set; }
        public DateTime created_at { get; set; }
    }

    public class Paging
    {
    }
}
