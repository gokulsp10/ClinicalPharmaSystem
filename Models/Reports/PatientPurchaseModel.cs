namespace ClinicalPharmaSystem.Models.Reports
{
    public class PatientPurchaseModel
    {
        public int PatientID { get; set; }
        public string? PatientName { get; set; }
        public string? MobileNo { get; set; }
        public int MedicineID { get; set; }
        public string? MedicineName { get; set; }
        public int PurchasedQuantity { get; set; }
    }
}
