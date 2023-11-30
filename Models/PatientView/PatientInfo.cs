namespace ClinicalPharmaSystem.Models.PatientView
{
    public class PatientInfo
    {
        public int PatientId { get; set; }
        public string? PatientName { get; set; }
        public string? MobileNo { get; set; }
        public string? Address { get; set; }
        public char Sex { get; set; }
        public int Age { get; set; }
    }
}
