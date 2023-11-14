using System.ComponentModel.DataAnnotations;

namespace ClinicalPharmaSystem.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string? UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public string? Email { get; set; }

        public string? RoleName { get; set; }

        public string? PhoneNumber { get; set; }

        public int RoleId { get; set; }
    }
}
