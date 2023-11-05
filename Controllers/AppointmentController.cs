using ClinicalPharmaSystem.DataContext;
using ClinicalPharmaSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicalPharmaSystem.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly AppointmentRepository repository;

        public AppointmentController(AppointmentRepository repository)
        {
            this.repository = repository;
        }

        public IActionResult AddAppointment()
        {
            return View();
        }

        public IActionResult AppointmentSuccess()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                int appointmentId = repository.SaveAppointment(appointment);
                if (appointmentId != 0)
                {
                    ViewBag.AppointmentId = appointmentId; // Store the ID in ViewBag
                    return View("AppointmentSuccess");
                }}

            return View("AddAppointment");
        }
        [HttpGet]
        public ActionResult CheckAvailability(string appointmentDate, string appointmentTime)
        {
            // Implement your logic to check the availability based on appointmentDate and appointmentTime
            // You can return a message indicating the availability status
            bool isAvailable = repository.AvailabilityCheckLogic(appointmentDate, appointmentTime);

            if (isAvailable)
            {
                return Content("Available"); // You can customize this message
            }
            else
            {
                return Content("Not Available"); // You can customize this message
            }
        }

    }
}
