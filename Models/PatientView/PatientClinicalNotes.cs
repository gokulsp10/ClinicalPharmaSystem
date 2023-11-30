namespace ClinicalPharmaSystem.Models.PatientView
{
    public class PatientClinicalNotes
    {
        public string? PatientId { get; set; }
        public string? ClinicalNotesText { get; set; }

        public DateTime ClinicalNotesVisitDate { get; set; }
    }
}
