namespace ClinicalPharmaSystem.Models
{
    public class Medicalhistory
    {
        public int MedicalHistoryId { get; set; }
        public int PatientId { get; set; }
        public string? MedicalHistoryText { get; set; }
        public string? VisitDate { get; set; }
    }
}
