namespace ClinicalPharmaSystem.Models.Reports
{
    public class ExpiringMedicineModel
    {
        public int MedicineID { get; set; }
        public string? MedicineName { get; set; }
        public int SupplierID { get; set; }
        public int CategoryID { get; set; }
        public int NumberOfTablets { get; set; }
        public int NumberOfStrips { get; set; }
        public decimal PricePerStrip { get; set; }
        public string? RouteofIntake { get; set; }
        public string? Strengths { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string? Container { get; set; }
        public int TabletCountPerStrip { get; set; }
    }
}
