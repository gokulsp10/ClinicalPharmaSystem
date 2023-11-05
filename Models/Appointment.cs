using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicalPharmaSystem.Models
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentID { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        public string AppointmentTime {get;set;}

        public string Notes { get; set; }

        public string MobileNumber { get; set; }
    }
}
