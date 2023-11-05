using ClinicalPharmaSystem.DataContext;
using ClinicalPharmaSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;

namespace ClinicalPharmaSystem.Controllers
{
    public class MedicalTestController : Controller
    {
        private readonly MedicalTestRepository medicalTestRepository;

        public MedicalTestController(MedicalTestRepository medicalTestRepository)
        {
            this.medicalTestRepository = medicalTestRepository;
        }

        public IActionResult AddDisease()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetDiseaseNames(string term)
        {
            // Fetch disease names and IDs based on the user's input term
            var results = medicalTestRepository.GetDiseasename(term);

            // Return results as JSON
            return Json(results);
        }

        [HttpGet]
        public JsonResult GetDiseaseNamesForEdit(string term)
        {
            // Fetch disease names and IDs based on the user's input term
            var results = medicalTestRepository.GetDiseasename(term);

            // Return results as JSON
            return Json(results);
        }

        [HttpGet]
        public JsonResult GetDiseaseTestNames(string term,string diseaseid, string Gender)
        {
            // Fetch disease names and IDs based on the user's input term
            var results = medicalTestRepository.GetDiseaseTestNames(term, diseaseid, Gender);

            // Return results as JSON
            return Json(results);
        }

        [HttpGet]
        public JsonResult GetDiseaseTestsForEdit(string term, string diseaseid)
        {
            // Fetch disease names and IDs based on the user's input term
            var results = medicalTestRepository.GetDiseaseTestsForEdit(term, diseaseid);

            // Return results as JSON
            return Json(results);
        }

        public IActionResult AddMedicalTest()
        {
            return View();
        }

        public IActionResult ViewEditDisease()
        {
            return View();
        }

        [HttpPost]
        public ActionResult postViewEditDiseases(string editDiseaseName, bool isDiseaseDeleted, int editDiseaseid)
        {
            try
            {
                medicalTestRepository.UpdateDiseaseDetails(editDiseaseName, isDiseaseDeleted, editDiseaseid);
                return View("ViewEditDisease");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during processing
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        public IActionResult ViewMedicalTests()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubmitDiseaseName([FromBody] DiseaseModel model)
        {
            medicalTestRepository.SaveDiseaseName(model.diseaseName);
            return View("AddDisease"); // Redirect to the view where you display the table
        }

        [HttpPost]
        public IActionResult SaveMedicalTest([FromBody] DiseaseModel model)
        {
            int lastInsertedId;
            if (ModelState.IsValid)
            {
                // Access the DiseaseName and DiseaseTests from the model
                string diseaseName = model.diseaseName;
                List<DiseaseTestModel> diseaseTests = model.DiseaseTests;

                lastInsertedId = medicalTestRepository.SaveDiseaseTestMasterRows(diseaseName, diseaseTests);
                // Redirect to a success page or display a success message
                if (lastInsertedId != 0)
                {
                    return View("ViewMedicalTests");
                }
            }

            // If the ModelState is not valid, return to the form with validation errors
            return View("AddMedicalTest", model);
        }

        [HttpGet]
        public List<DiseaseListModelEdit> GetDiseaseTestsEdit(string diseaseNameId)
        {
            List<DiseaseListModelEdit> diseaseTestModels = new List<DiseaseListModelEdit>();

            diseaseTestModels = medicalTestRepository.GetDiseaseTestsEdit(diseaseNameId);

            return diseaseTestModels;
        }

        public ActionResult UpdateDiseaseTestValues(string diseaseNameId, string diseaseId)
        {
            string modifiedJson = diseaseNameId.Replace(@"""IsActive"":""1""", @"""IsActive"":""true""").Replace(@"""IsActive"":""0""", @"""IsActive"":""false""");
            

            List<DiseaseListModelEdit> diseaseList = new List<DiseaseListModelEdit>();
            if (diseaseNameId != null && diseaseNameId.Any())
            {
                // Deserialize the JSON string into a list of DiseaseListModelEdit
                diseaseList = JsonConvert.DeserializeObject<List<DiseaseListModelEdit>>(modifiedJson);

                medicalTestRepository.updateMedicalTestValues(diseaseId, diseaseList);
            }

            return View("success");
        }

        public ActionResult DeleteDiseaseTestValues(string diseaseNameId, string diseaseId)
        {
            List<DiseaseListModelEdit> diseaseList = new List<DiseaseListModelEdit>();
            if (diseaseNameId != null && diseaseNameId.Any())
            {
                // Deserialize the JSON string into a list of DiseaseListModelEdit
                diseaseList = JsonConvert.DeserializeObject<List<DiseaseListModelEdit>>(diseaseNameId);

                medicalTestRepository.updateMedicalTestValues(diseaseId, diseaseList);
            }

            return View("success");
        }

        public IActionResult success()
        {
            return View();
        }
    }
}
