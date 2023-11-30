namespace ClinicalPharmaSystem.Models
{
    public class ClinicalNotes
    {
        public int ClinicalNotesId { get; set; }
        public int PatientId { get; set; }
        public string? ClinicalNotesText { get; set; }
        public string? VisitDate { get; set; }
    }
}
