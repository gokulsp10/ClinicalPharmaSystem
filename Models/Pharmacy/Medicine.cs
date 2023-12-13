namespace ClinicalPharmaSystem.Models.Pharmacy
{
    public class Medicine
    {
        public int MedicineID { get; set; }
        public int SupplierID { get; set; }
        public int CategoryID { get; set; }
        public string? MedicineName { get; set; }
        public int NumberOfStrips { get; set; }
        public double PricePerStrip { get; set; }
        public int NumberOfTablets { get; set; }
        public string? RouteOfIntake { get; set; }
        public string? Strengths { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string? CategoryName { get; set; }
        public string? SupplierName { get; set; }
        public string? Container { get; set; }
        public int TabletCountPerStrip { get; set; }
    }
}
