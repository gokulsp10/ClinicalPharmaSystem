namespace ClinicalPharmaSystem.Models.PatientView
{
    public class PatientView
    {
        public List<PatientInfo>? patientInfo { get; set; }
        public List<PatientHealthData>? patientHealthData { get; set; }
        public List<PatientDiseaseMetrics>? patientDiseaseMetrics { get; set; }
        public List<PatientClinicalNotes>? patientClinicalNotes { get; set; }
        public List<PatientMedicalHistory>? patientMedicalHistory { get; set; }
        public List<PatientPrescription>?   patientPrescriptions { get; set; }
    }
}
