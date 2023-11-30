namespace ClinicalPharmaSystem.Models.PatientView
{
    public class PatientHealthData
    {
        public string? PatientId { get; set; }
        public DateTime Date { get; set; }
        public string? BloodPressure { get; set; }
        public string? PulseRate { get; set; }
        public string? Weight { get; set; }
        public string? SpO2 { get; set; }
    }
}
