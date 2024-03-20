namespace BloomersCarriersIntegrations.FlashCourier.Domain.Entities
{
    public class HAWBRequest
    {
        public HAWBRequest(int ClienteId, int[] CttId, string[] NumEncCli)
        {
            clienteId = ClienteId;
            cttId = CttId;
            numEncCli = NumEncCli;
        }

        public int clienteId { get; set; }
        public int[] cttId { get; set; }
        public string[] numEncCli { get; set; }
    }
}
