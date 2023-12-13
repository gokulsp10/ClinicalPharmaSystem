namespace ClinicalPharmaSystem.Models.Pharmacy
{
    public class MedicineCategory
    {
        public int CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
