namespace ClinicalPharmaSystem.Models
{
    public class PatientMedications
    {
        public int PatientId { get; set; }
        public int DiseasesTestId { get; set; }
        public int DiseasesId { get; set; }
        public string? DrugName { get; set; }
        public string? Dossage { get; set; }
        public string? Frequency { get; set; }
        public string? Duration { get; set; }
        public string? Route { get; set; }
        public string? Instruction { get; set; }
        public string? Notes { get; set; }
    }
}
