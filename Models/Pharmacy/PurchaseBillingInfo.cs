namespace ClinicalPharmaSystem.Models.Pharmacy
{
    public class PurchaseBillingInfo
    {
        public string? MedicineID { get; set; }
        public string? MedicineName { get; set; }
        public string? MrpPerUnit { get; set; }
        public string? PurchasedQuantity { get; set; }
        public string? Discount { get; set; }
        public string? GST { get; set; }
        public string? Total { get; set; }
        public string? ExpiryDate { get; set; }
        public string? InvoiceID { get; set; }
    }
}
