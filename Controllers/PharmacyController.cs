using ClinicalPharmaSystem.DataContext;
using ClinicalPharmaSystem.Models;
using ClinicalPharmaSystem.Models.PatientView;
using ClinicalPharmaSystem.Models.Pharmacy;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;

namespace ClinicalPharmaSystem.Controllers
{
    public class PharmacyController : Controller
    {
        private readonly PharmacyRepository pharmacyRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public PharmacyController(PharmacyRepository pharmacyRepository, IHttpContextAccessor contextAccessor)
        {
            this.pharmacyRepository = pharmacyRepository;
            this._contextAccessor = contextAccessor;
        }
        public IActionResult AddSupplier()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveSupplier(MedicineSupplier supplier)
        {
            string username = HttpContext.Session.GetString("UserName");
            int status = pharmacyRepository.Insertsupplier(supplier, username);
            if (status == 0)
            {
                return Json(new { success = true, message = "MedicineSupplier added successfully." });
            }
            return Json(new { success = false, message = "Failed to add MedicineSupplier details." });
        }

        [HttpGet]
        public List<MedicineSupplier> GetSupplierData()
        {
            // Retrieve data for ClinicalNotes from your database or source
            List<MedicineSupplier> clinicalNotesList = pharmacyRepository.GetSupplierData();

            return clinicalNotesList; // Pass the data to the view
        }
        
        [HttpGet]
        public JsonResult GetSupplierDataForEntry(string term)
        {
            // Retrieve data for ClinicalNotes from your database or source
            var results = pharmacyRepository.GetSupplierDataForEntry(term);

            // Return results as JSON
            return Json(results);
        }

        [HttpPost]
        public ActionResult UpdateSupplierData(int SupplierID, string SupplierName, string ContactPerson
            , string Email, string Phone, string Address,string flag)
        {
            string username = HttpContext.Session.GetString("UserName");
            pharmacyRepository.UpdateSupplierData(SupplierID, SupplierName, ContactPerson, Email, Phone, Address, username, flag);
            return Json(new { success = true, message = "Medical note updated successfully." });
        }

        //medicine category 

        public IActionResult AddMedicineCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveMedicineCategory(MedicineCategory medicineCategory)
        {
            string username = HttpContext.Session.GetString("UserName");
            int status = pharmacyRepository.InsertMedicineCategory(medicineCategory, username);
            if (status == 0)
            {
                return Json(new { success = true, message = "Medicine category added successfully." });
            }
            return Json(new { success = false, message = "Failed to add Medicine category details." });
        }

        [HttpGet]
        public List<MedicineCategory> GetMedicineCategory()
        {
            // Retrieve data for ClinicalNotes from your database or source
            List<MedicineCategory> clinicalNotesList = pharmacyRepository.GetMedicineCategory();

            return clinicalNotesList; // Pass the data to the view
        }

        [HttpGet]
        public JsonResult GetMedicineCategoryForEntry(string term)
        {
            // Retrieve data for ClinicalNotes from your database or source
            var results = pharmacyRepository.GetMedicineCategoryForEntry(term);

            // Return results as JSON
            return Json(results);
        }

        [HttpPost]
        public ActionResult UpdateMedicineCategory(int CategoryID, string CategoryName, string flag)
        {
            string username = HttpContext.Session.GetString("UserName");
            pharmacyRepository.UpdateMedicineCategory(CategoryID, CategoryName, username, flag);
            return Json(new { success = true, message = "Medical note updated successfully." });
        }

        //new medicines

        public IActionResult AddMedicines()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveMedicine(Medicine medicine,int SupplierID,int CategoryID)
        {
            string username = HttpContext.Session.GetString("UserName");
            int status = pharmacyRepository.InsertMedicine(medicine, username, SupplierID, CategoryID);
            if (status == 0)
            {
                return Json(new { success = true, message = "Medicine category added successfully." });
            }
            return Json(new { success = false, message = "Failed to add Medicine category details." });
        }

        [HttpGet]
        public List<Medicine> GetMedicine()
        {
            // Retrieve data for ClinicalNotes from your database or source
            List<Medicine> clinicalNotesList = pharmacyRepository.GetMedicines();

            return clinicalNotesList; // Pass the data to the view
        }

        [HttpGet]
        public List<Medicine> GetMedicineByName(string term)
        {
            // Retrieve data for ClinicalNotes from your database or source
            List<Medicine> clinicalNotesList = pharmacyRepository.GetMedicinesByName(term);

            return clinicalNotesList; // Pass the data to the view
        }

        [HttpGet]
        public List<Medicine> SearchMedicine(string SupplierID, string CategoryID,string MedicineName)
        {
            // Retrieve data for ClinicalNotes from your database or source
            List<Medicine> clinicalNotesList = pharmacyRepository.SearchMedicine(MedicineName, SupplierID,CategoryID);

            return clinicalNotesList; // Pass the data to the view
        }

        [HttpGet]
        public IActionResult SelectMedicine(int medicineID)
        {
            // Retrieve data for Medicine from your database or source
            List<Medicine> medicineList = pharmacyRepository.GetMedicineById(medicineID);

            string jsonData = JsonConvert.SerializeObject(medicineList); // Convert to JSON

            return Content(jsonData, "application/json"); // Return JSON response
        }

        [HttpPost]
        public ActionResult UpdateMedicalData(int MedicineID, int CategoryID, int SupplierID, string MedicineName, int NumberOfStrips, double PricePerStrip, int NumberOfTablets, string RouteOfIntake, string Strengths, DateTime ManufacturingDate, DateTime ExpiryDate,string Container,int TabletCountPerStrip, string flag)
        {
            string username = HttpContext.Session.GetString("UserName");
            pharmacyRepository.UpdateMedicine(MedicineID, CategoryID, SupplierID, MedicineName, NumberOfStrips, PricePerStrip, NumberOfTablets, RouteOfIntake, Strengths,username, Container, TabletCountPerStrip, flag);

            // Return a success message or any appropriate response
            return Json(new { success = true, message = "Medicine updated successfully" });
        }

        [HttpPost]
        public IActionResult UpdateMedicineCount([FromBody] List<MedicineData> medicineData, string rollback)
        {
            string username = HttpContext.Session.GetString("UserName");
            pharmacyRepository.UpdateMedicineCount(medicineData, username, rollback);

            // Return a success message or any appropriate response
            return Json(new { success = true, message = "Medicine updated successfully" });
        }

        [HttpPost]
        public IActionResult RollBackUpdatedMedicineCount([FromBody] List<MedicineData> medicineData, string rollback)
        {
            string username = HttpContext.Session.GetString("UserName");
            pharmacyRepository.UpdateMedicineCount(medicineData, username, rollback);

            // Return a success message or any appropriate response
            return Json(new { success = true, message = "Medicine updated successfully" });
        }

        [HttpPost]
        public IActionResult InsertPurchaseSales([FromBody] List<PurchaseSales> medicineData)
        {
            string username = HttpContext.Session.GetString("UserName");
            int SaleId = pharmacyRepository.InsertPurchaseSales(medicineData, username);

            return Json(new { success = true, message = SaleId });
        }

        [HttpPost]
        public IActionResult InsertPurchasemedicineInfo([FromBody] List<PurchaseBillingInfo> medicineData)
        {
            string username = HttpContext.Session.GetString("UserName");
            pharmacyRepository.InsertPurchasemedicineInfo(medicineData, username);

            // Return a success message or any appropriate response
            return Json(new { success = true, message = "PurchaseSales added successfully" });
        }

        [HttpPost]
        public IActionResult InsertPurchasePatientInfo([FromBody] List<PatienBillingInfo> medicineData)
        {
            string username = HttpContext.Session.GetString("UserName");
            pharmacyRepository.InsertPurchasePatientInfo(medicineData, username);

            // Return a success message or any appropriate response
            return Json(new { success = true, message = "InsertPurchasePatientInfo added successfully" });
        }

        public IActionResult AddnewBill()
        {
            ClinicDetails ClinicDetails = pharmacyRepository.GetClinicDetails();
            return View(ClinicDetails);
        }


        [HttpGet]
        public List<PatientModel> GetPatienDetailsById(string term)
        {
            // Retrieve data for ClinicalNotes from your database or source
            List<PatientModel> clinicalNotesList = pharmacyRepository.GetPatienDetailsById(term);

            return clinicalNotesList; // Pass the data to the view
        }

    }
}
