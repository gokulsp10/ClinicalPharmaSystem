namespace ClinicalPharmaSystem.Models.PatientView
{
    public class PatientMedicalHistory
    {
        public string? PatientId { get; set; }
        public string? MedicalHistoryText { get; set; }

        public DateTime MedicalHistoryVisitDate { get; set; }
    }
}
