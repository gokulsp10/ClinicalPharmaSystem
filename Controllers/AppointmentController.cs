using ClinicalPharmaSystem.DataContext;
using ClinicalPharmaSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

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
            List<string> timeOptions = repository.GetTimeOptions();
            ViewBag.TimeOptions = timeOptions;
            return View();
        }

        public IActionResult AppointmentSuccess()
        {
            return View();
        }

        public IActionResult ApproveRejectAppointment() 
        {
            return View();
        }

        public IActionResult AddClinicalHours()
        {
            return View();
        }

        public IActionResult UpdateClinicalHours()
        {
            var timeOption = repository.GetClinicalTimes();
            return View(timeOption);
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
                    ViewBag.Date = appointment.AppointmentDate;
                    ViewBag.Time = appointment.AppointmentTime;
                    ViewBag.Contact = appointment.MobileNumber;
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

        [HttpPost]
        public IActionResult UpdateClinicalTime([FromBody] AppointmentTimeOption updatedTimeOption)
            {
            int status = 0;
            status = repository.UpdateClinicalTime(updatedTimeOption);
            if(status==1)
            {
                return Json(new { status = 1 });
            }
            return Json(new { status = 0 });
        }

        [HttpPost]
        public IActionResult AddAppointmentTime([FromBody] Dictionary<string, string> formData)
        {
            string hours = formData["Hours"];
            string minutes = formData["Minutes"];
            string ampm = formData["AMPM"];
            var timeoptions = $"{hours}:{minutes} {ampm}";

            int status = 0;
            status = repository.SaveAppointmentTimeOption(timeoptions);

            if (status != 0)
            {
                return Json(new { status = 1 });
            }
            return Json(new { status = 0 });
        }

    }
}
