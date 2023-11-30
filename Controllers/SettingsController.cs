using ClinicalPharmaSystem.Models;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Admin")]
        public IActionResult AddEditRoles()
        {
            var viewModel = new RoleFormViewModel
            {
                Roles = settingsRepository.GetAllRoles(),
                Role = new RoleForm()
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult RegistrationApproval()
        {
            var inactiveUsers = settingsRepository.GetPendingActivationUsers();
            var roles = settingsRepository.GetRolesFromDatabase();
            ViewBag.Roles = roles;
            return View(inactiveUsers);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult UpdateUser()
        {
            var inactiveUsers = settingsRepository.GetUsers();
            var roles = settingsRepository.GetRolesFromDatabase();
            ViewBag.Roles = roles;
            return View(inactiveUsers);
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
        public IActionResult UpdateUserStatus(User user) 
        {
            bool updatesStatus = settingsRepository.UpdateUser(user);
            if (updatesStatus)
            {
                return Json(new { success = true, message = "User status updated successfully" });
            }
            else
            {
                return Json(new { success = false, message = "Failed to update user status" });
            }
        }

        [HttpPost]
        public ActionResult SubmitClinicalNotes(List<ClinicalNotes> model)
        {
            // Process the received data, perform necessary operations
            // You can access the form data via the 'model' parameter

            // Return a response if needed
            return Json(new { success = true, message = "Medical tests submitted successfully" });
        }
    }
}
