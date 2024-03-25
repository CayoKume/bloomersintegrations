namespace BloomersCarriersIntegrations.FlashCourier.Domain.Entities
{
    public class FlashCourierRegisterLog
    {
        public string orderNumber { get; set; }
        public DateTime shippingDate { get; set; }
        public string _return { get; set; }
        public string statusEcom { get; set; }
        public string sender { get; set; }
        public string statusFlash { get; set; }
        public string keyNFe { get; set; }
        public string doc_company { get; set; }
    }
}
