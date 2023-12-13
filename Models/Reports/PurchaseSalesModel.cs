namespace ClinicalPharmaSystem.Models.Reports
{
    public class PurchaseSalesModel
    {
        public int SaleId { get; set; }
        public string ModeOfPayment { get; set; }
        public decimal SaleAmount { get; set; }
        public DateTime SaleDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Comments { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
