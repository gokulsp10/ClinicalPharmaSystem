using ClinicalPharmaSystem.Models;

public class RoleFormViewModel
{
    public RoleForm Role { get; set; }
    public List<Role> Roles { get; set; }
}

public class RoleForm
{
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public string RoleDescription { get; set; }
    public bool IsActive { get; set; }
}
