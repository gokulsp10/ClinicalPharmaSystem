namespace ClinicalPharmaSystem.Models
{
    public class PatientHealthDataModel
    {
        public DateTime VisitDate { get; set; }
        public string? BloodPressure { get; set; }
        public string? PulseRate { get; set; }
        public string? Weight { get; set; }
        public string? SpO2 { get; set; }
    }

}
