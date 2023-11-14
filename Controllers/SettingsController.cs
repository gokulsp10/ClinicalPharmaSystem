using ClinicalPharmaSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ClinicalPharmaSystem.Controllers
{
    public class SettingsController : Controller
    {
        private readonly SettingsRepository settingsRepository;

        public SettingsController(SettingsRepository settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }

        public IActionResult AddEditRoles()
        {
            var viewModel = new RoleFormViewModel
            {
                Roles = settingsRepository.GetAllRoles(),
                Role = new RoleForm()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SaveRole(RoleFormViewModel roleForm)
        {
            // Save logic
            settingsRepository.AddRole(roleForm.Role);
            // Update the Roles list in the model
            var viewModel = new RoleFormViewModel
            {
                Roles = settingsRepository.GetAllRoles(),
                Role = new RoleForm()
            };

            return View("AddEditRoles", viewModel);
        }

        [HttpPost]
        public IActionResult UpdateRole([FromBody] Role role)
        {
            settingsRepository.UpdateRole(role);
            return Json(new { success = true, message = "Role updated successfully" });
        }


    }
}
