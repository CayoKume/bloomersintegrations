namespace BloomersCarriersIntegrations.FlashCourier.Domain.Entities
{
    public class AuthRequest
    {
        public AuthRequest()
        {

        }

        public AuthRequest(string login, string senha)
        {
            this.login = login;
            this.senha = senha;
        }

        public string login { get; set; }
        public string senha { get; set; }
    }
}
