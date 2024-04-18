namespace BloomersGeneralIntegrations.Movidesk.Domain.Entities
{
    public class Ticket
    {
        public string id { get; set; }
        public string subject { get; set; }
        public string serviceFirstLevel { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime resolvedIn { get; set; }
        public Person createdBy { get; set; }
        public CustomField[] customFieldValues { get; set; }
        public Actions[] actions { get; set; }
    }

    public class Person
    {
        public string businessName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }

    public class CustomField
    {
        public int customFieldId { get; set; }
        public string value { get; set; }
        public Items[] items { get; set; }
    }

    public class Items
    {
        public string customFieldItem { get; set; }
    }

    public class Actions
    {
        public int id { get; set; }
        public string description { get; set; }
    }
}
