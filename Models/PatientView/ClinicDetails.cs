namespace ClinicalPharmaSystem.Models.PatientView
{
    public class ClinicDetails
    {
            public int ClinicalId { get; set; } // Assuming an identity primary key
            public string? ClinicName { get; set; }
            public string? ClinicContactAddress { get; set; }
            public string? ClinicContact { get; set; }
            public bool IsDeleted { get; set; }
            public string? AppointmentLink { get; set; }
            public string? AppointmentPhone { get; set; }
            public string? WorkingHours { get; set; }
    }
}
