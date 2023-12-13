namespace ClinicalPharmaSystem.Models.PatientView
{
    public class DoctorDetails
    {
        public int IndexId { get; set; } // Primary Key - Identity column in the database
        public string? DoctorName { get; set; }
        public string? DoctorId { get; set; }
        public string? DoctorStudy { get; set; }
        public string? DoctorSpeciality { get; set; }
        public string? DoctorContactAddress { get; set; }
        public string? ContactMobile { get; set; }
        public string? ContactNumber { get; set; }

        public string? RegNo { get; set; }  
    }
}
