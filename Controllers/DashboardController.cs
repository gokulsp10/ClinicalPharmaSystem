using ClinicalPharmaSystem.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicalPharmaSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DashboardRepository dashboardRepository;
        public DashboardController(DashboardRepository dashboardRepository)
        {
            this.dashboardRepository = dashboardRepository;
        }

        [Authorize(Roles = "Doctor,Admin")]
        public IActionResult Clinical ()
        {
            return View();
        }

        [Authorize(Roles = "Doctor,Admin")]
        public IActionResult Patient()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FetchRecords(string patientId)
        {
            // Query the database to fetch records for the given patient ID
            var healthRecords = dashboardRepository.GetPatientRecordsByUserID(patientId);

            return View(healthRecords);
        }
    }
}
