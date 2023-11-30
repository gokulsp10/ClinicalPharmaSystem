namespace ClinicalPharmaSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public int RoleId { get; set; }
        public string? action { get; set; }
        public bool IsActivated { get; set; }
        public bool IsDeleted { get; set; }
    }

}
