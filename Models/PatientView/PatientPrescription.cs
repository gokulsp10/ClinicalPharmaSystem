namespace ClinicalPharmaSystem.Models.PatientView
{
    public class PatientPrescription
    {
        public int SerialNumber { get; set; }
        public int PatientId { get; set; }
        public string? Name { get; set; }
        public string? Frequency { get; set; }
        public string? Instructions { get; set; }
        public string? Days { get; set; }
        public string? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
