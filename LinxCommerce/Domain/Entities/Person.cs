namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Entities
{
    public class Person
    {
        public string Surname { get; set; }
        public string BirthDate { get; set; }
        public string Gender { get; set; }
        public string RG { get; set; }
        public string Cpf { get; set; }
        public string CreatedDate { get; set; }
        public string CustomerID { get; set; }
        public string CustomerStatusID { get; set; }
        public string WebSiteID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CustomerHash { get; set; }
        public string Password { get; set; }
        public string CustomerType { get; set; }
        public List<Groups> Groups { get; set; }
        public Contact Contact { get; set; }
        public List<PersonAddress> Address { get; set; }
        public EmailConfirmation EmailConfirmation { get; set; }
    }
}
