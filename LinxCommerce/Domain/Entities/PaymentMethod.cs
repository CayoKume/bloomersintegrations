namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Entities
{
    public class PaymentMethod
    {
        public string OrderPaymentMethodID { get; set; }
        public string OrderID { get; set; }
        public string PaymentNumber { get; set; }
        public string PaymentMethodID { get; set; }
        public string TransactionID { get; set; }
        public string ReconciliationNumber { get; set; }
        public string Status { get; set; }
        public string IntegrationID { get; set; }
        public string Amount { get; set; }
        public string AmountNoInterest { get; set; }
        public string InterestValue { get; set; }
        public string PaidAmount { get; set; }
        public string RefundAmount { get; set; }
        public string Installments { get; set; }
        public string InstallmentAmount { get; set; }
        public string TaxAmount { get; set; }
        public string PaymentDate { get; set; }
        public PaymentInfo PaymentInfo { get; set; } = new PaymentInfo();
        public List<Property> Properties { get; set; } = new List<Property>();
        public string CaptureDate { get; set; }
        public string AcquiredDate { get; set; }
        public string PaymentCancelledDate { get; set; }
    }

    public class PaymentInfo
    {
        public string Identifier { get; set; }
        public string Alias { get; set; }
        public string PaymentDate { get; set; }
        public string ExpirationDate { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Holder { get; set; }
        public string NumberHint { get; set; }
        public string SecurityCodeHint { get; set; }
        public string TransactionNumber { get; set; }
        public string AuthorizationCode { get; set; }
        public string ReceiptCode { get; set; }
        public string ReconciliationNumber { get; set; }
        public string ConfirmationNumber { get; set; }
        public string PaymentType { get; set; }
        public string Provider { get; set; }
        public string ProviderDocumentNumber { get; set; }
        public string PixQRCode { get; set; }
        public string PixKey { get; set; }

        public string OrderID { get; set; }
        public string OrderPaymentMethodID { get; set; }
        public string PaymentMethodID { get; set; }
        public string TransactionID { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
    }
}
