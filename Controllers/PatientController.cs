using ClinicalPharmaSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using Dapper;
using ClinicalPharmaSystem.DataContext;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.SqlServer.Server;
using ClinicalPharmaSystem.Models.PatientView;

namespace ClinicalPharmaSystem.Controllers
{
    public class PatientController : Controller
    {
        private readonly PatientRepository patientRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public PatientController(PatientRepository patientRepository,IHttpContextAccessor contextAccessor)
        {
            this.patientRepository = patientRepository;
            this._contextAccessor = contextAccessor;
        }

        [Authorize(Roles = "Doctor,Admin")]
        public IActionResult NewPatient()
        {
            var patient = new PatientModel();
            return View(patient);
        }

        [Authorize(Roles = "Doctor,Admin")]
        public IActionResult AddRecords()
        {
            return View();
        }

        [Authorize(Roles = "Doctor,Admin")]
        public IActionResult AddClinicalNotes()
        {
            return View();
        }

        [Authorize(Roles ="Doctor,Admin")]
        public IActionResult AddMedicalHistory()
        {
            return View();
        }

        [Authorize(Roles = "Doctor,Admin")]
        public IActionResult NewHealthMetrics()
        {
            return View();
        }

        [Authorize(Roles = "Doctor,Admin")]
        public IActionResult AddPrescription()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SavePrescription(int patientId, List<PatientPrescription> prescriptions)
        {
            int lastInsertedId;
            string username = HttpContext.Session.GetString("UserName");
            lastInsertedId = patientRepository.SavePrescription(patientId, prescriptions, username);
            // Redirect to a success page or display a success message
            if (lastInsertedId != 0)
            {
                return RedirectToAction("ViewPatient", new { id = patientId });
            }
            return View();
        }

        [HttpPost]
        public ActionResult UpdatePrescription(int SerialNumber, string Name, string Frequency, string Instructions, string Days)
        {
            string username = HttpContext.Session.GetString("UserName");
            int result = patientRepository.UpdatePrescription(SerialNumber, Name, Frequency, Instructions, Days, username);
            if (result == 0)
            {
                return Json(new { success = false, message = "Prescription note update failed." });
            }
            return Json(new { success = true, message = "Prescription note updated successfully." });
        }

        [HttpGet]
        public List<PatientPrescription> GetPrescription(int PatientId)
        {
            // Retrieve data for ClinicalNotes from your database or source
            List<PatientPrescription> patientPrescriptions = patientRepository.GetPrescriptions(PatientId);

            return patientPrescriptions; // Pass the data to the view
        }

        [HttpPost]
        public IActionResult SavePatientHealthData(PatientHealthDataModel healthData)
        {
            int lastInsertedId;
            if (ModelState.IsValid)
            {
                lastInsertedId = patientRepository.SavePatientHealthData(healthData);
                // Redirect to a success page or display a success message
                if (lastInsertedId != 0)
                {
                    return RedirectToAction("ViewPatient", new { id = healthData.PatientId });
                }
            }

            return View();
        }

        [Authorize(Roles = "Doctor,Admin")]
        public IActionResult ViewPatient(string id)
        {
            string username = HttpContext.Session.GetString("UserName");
            string userRole = HttpContext.Session.GetString("Role");
            Request.Query.TryGetValue("id", out var Qid);
            ViewBag.PatientId = Qid;
            ViewBag.UserRole = userRole;
            PatientView patients = patientRepository.ViewPatient(id, username,DateTime.Now.ToShortDateString(),null);
            return View(patients);
        }

        [HttpPost]
        public IActionResult ViewPatientById(string patientId, string datepickerFrom,string datepickerTo)
        {
            string username = HttpContext.Session.GetString("UserName");
            string userRole = HttpContext.Session.GetString("Role");
            ViewBag.PatientId = patientId;
            ViewBag.UserRole = userRole;
            PatientView patients = patientRepository.ViewPatient(patientId, username, datepickerFrom, datepickerTo);
            return View("ViewPatient", patients);
        }


        [HttpPost]
        public IActionResult SavePatient(PatientModel patient)
        {
            int lastInsertedId;
                lastInsertedId = patientRepository.SavePatient(patient);
                // Redirect to a success page or display a success message
                if (lastInsertedId != 0)
                {
                    return RedirectToAction("ViewPatient", new { id = lastInsertedId, success = "true" });
                }            

            // If the model state is not valid, return the form with validation errors
            return View("ViewPatient",patient);
        }

        [HttpPost]
        public IActionResult SavePatientRows(string patientId, List<PatientHealthDataModel> formData)
        {
            int lastInsertedId;
            if (ModelState.IsValid)
            {
                lastInsertedId = patientRepository.SavePatientRows(patientId, formData);
                // Redirect to a success page or display a success message
                if (lastInsertedId != 0)
                {
                    return RedirectToAction("ViewPatient", new { id = patientId });
                }
            }

            // If the model state is not valid, return the form with validation errors
            return View("ViewPatient");
        }
        public JsonResult CheckPatientExistence(string patientId)
        {
            List<PatientModel> patients = patientRepository.ViewPatientExist(patientId);
            var patientExists = patients.Count > 0 ? patients[0].Sex.ToString() : "";

            return Json(patientExists);
        }

        [HttpPost]
        public ActionResult SubmitMedicalTest(DiseaseTestModel model)
        {
            int lastInsertedId;
            if (ModelState.IsValid)
            {
                lastInsertedId = patientRepository.SaveTestValueResults(model);
                // Redirect to a success page or display a success message
                if (lastInsertedId != 0)
                {
                    return View("AddRecords");
                }
            }

            // If the model state is not valid, return the form with validation errors
            return View("AddRecords");
        }

        [HttpGet]
        public List<DiseaseListModel> getDiseaseTest(string diseaseNameId)
        {
            List<DiseaseListModel>   diseaseTestModels = new List<DiseaseListModel>();

            diseaseTestModels = patientRepository.GetDiseaseTests(diseaseNameId);

            return diseaseTestModels;
        }

        [HttpPost]
        public ActionResult SubmitClinicalNotes(ClinicalNotes model)
        {
            int lastInsertedId;
            if (ModelState.IsValid)
            {
                string username = HttpContext.Session.GetString("UserName");
                lastInsertedId = patientRepository.SaveClinicalNotes(model, username);
                // Redirect to a success page or display a success message
                if (lastInsertedId != 0)
                {
                    return Json(new { status = 1 });
                }
            }

            return Json(new { status = 0 });
        }

        [HttpPost]
        public ActionResult SubmitMedicalhistory(Medicalhistory model)
        {
            int lastInsertedId;
            if (ModelState.IsValid)
            {
                lastInsertedId = patientRepository.SaveMedicalHistory(model);
                // Redirect to a success page or display a success message
                if (lastInsertedId != 0)
                {
                    return Json(new { status = 1 });
                }
            }

            return Json(new { status = 0 });
        }

        [HttpGet]
        public List<ClinicalNotes> GetClinicalNotes(int PatientId)
        {
            // Retrieve data for ClinicalNotes from your database or source
            List<ClinicalNotes> clinicalNotesList = patientRepository.GetClinicalNotes(PatientId);

            return clinicalNotesList; // Pass the data to the view
        }

        [HttpPost]
        public ActionResult UpdateClinicalNote(int ClinicalNotesId, string ClinicalNotesText)
        {
            string username =HttpContext.Session.GetString("UserName");
            int result = patientRepository.UpdateClinicalNote(ClinicalNotesId, ClinicalNotesText, username);
            if(result == 0) {
                return Json(new { success = false, message = "Clinical note update failed." });
            }
            return Json(new { success = true, message = "Clinical note updated successfully." });
        }

        [HttpGet]
        public List<Medicalhistory> GetMedicalHistory(int PatientId)
        {
            // Retrieve data for ClinicalNotes from your database or source
            List<Medicalhistory> clinicalNotesList = patientRepository.GetMedicalNote(PatientId);

            return clinicalNotesList; // Pass the data to the view
        }

        [HttpPost]
        public ActionResult UpdateMedicalNote(int ClinicalNotesId, string ClinicalNotesText)
        {
            string username = HttpContext.Session.GetString("UserName");
            int result = patientRepository.UpdateMedicalNote(ClinicalNotesId, ClinicalNotesText, username);
            if (result == 0)
            {
                return Json(new { success = false, message = "Medical note update failed." });
            }
            return Json(new { success = true, message = "Medical note updated successfully." });
        }

    }
}