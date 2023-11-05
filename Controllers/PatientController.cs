using ClinicalPharmaSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using Dapper;
using ClinicalPharmaSystem.DataContext;
using Newtonsoft.Json.Linq;

namespace ClinicalPharmaSystem.Controllers
{
    public class PatientController : Controller
    {
        private readonly PatientRepository patientRepository;

        public PatientController(PatientRepository patientRepository)
        {
            this.patientRepository = patientRepository;
        }

        public IActionResult NewPatient()
        {
            var patient = new PatientModel();
            return View(patient);
        }

        public IActionResult AddRecords()
        {
            return View();
        }

        public IActionResult ViewPatient(string id)
        {
            Request.Query.TryGetValue("id", out var Qid);
            ViewBag.PatientId = Qid;
            List<PatientModel> patients = patientRepository.ViewPatient(id);
            return View(patients);
        }

        [HttpPost]
        public IActionResult ViewPatientById(string patientId)
        {
            ViewBag.PatientId = patientId;
            List<PatientModel> patients = patientRepository.ViewPatient(patientId);
            return View("ViewPatient", patients);
        }


        [HttpPost]
        public IActionResult SavePatient(PatientModel patient)
        {
            int lastInsertedId;
            if (ModelState.IsValid)
            {
                lastInsertedId = patientRepository.SavePatient(patient);
                // Redirect to a success page or display a success message
                if (lastInsertedId != 0)
                {
                    return RedirectToAction("ViewPatient", new { id = lastInsertedId, success = "true" });
                }
            }

            // If the model state is not valid, return the form with validation errors
            return View(patient);
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
            List<PatientModel> patients = patientRepository.ViewPatient(patientId);
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
        
    }
}
