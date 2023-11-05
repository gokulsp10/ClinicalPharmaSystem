namespace ClinicalPharmaSystem.Models
{
    public class patientSecondaryMetrics
    {
        public int PatientId { get; set; }
        public int DiseasesTestId { get; set; }
        public int DiseasesId { get; set; }
        public string? ReferenceValues { get; set; }
        public string? Notes { get; set; }
    }
}
