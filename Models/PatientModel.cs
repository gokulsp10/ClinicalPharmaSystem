namespace ClinicalPharmaSystem.Models
{
    public class PatientModel
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public char Sex { get; set; }
        public int Age { get; set; }
        public DateTime Date { get; set; }
        public string BloodPressure { get; set; }
        public string PulseRate { get; set; }
        public string Weight { get; set; }
        public string SpO2 { get; set; }

        public PatientModel()
        {
            // Set a default date, for example, today's date
            Date = DateTime.Now;
        }

        public string DiseaseName { get; set; }
        public string DiseasesTestName { get; set; }
        public string Testvalue { get; set; }
        public string Status { get; set; }
        public DateTime TestTakenDate { get; set; }

        public string ClinicalNotesText { get; set; }

        public DateTime ClinicalNotesVisitDate { get; set; }

        public string MedicalHistoryText { get; set; }

        public DateTime MedicalHistoryVisitDate { get; set; }

    }

    public class PatientHealthData
    {
        public string? BloodPressure { get; set; }
        public string? PulseRate { get; set; }
        public string? Weight { get; set; }
        public string? SpO2 { get; set; }

    }

}