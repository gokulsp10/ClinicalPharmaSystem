namespace ClinicalPharmaSystem.Models.Pharmacy
{
    public class PurchaseSales
    {
        public int SaleId { get; set; }
        public string? ModeOfPayment { get; set; }
        public string? SaleAmount { get; set; }
        public string? BillDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? Comments { get; set; }
    }
}
