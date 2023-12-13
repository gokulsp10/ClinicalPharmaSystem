using ClinicalPharmaSystem.DataContext;
using ClinicalPharmaSystem.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using System.Web.Mvc;

namespace ClinicalPharmaSystem.Controllers
{
    public class ReportsController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly ReportsRepository reportsRepository;

        public ReportsController(ReportsRepository reportsRepository, IHttpContextAccessor contextAccessor)
        {
            this.reportsRepository = reportsRepository;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Sales()
        {
            // Call the stored procedure to get data from PurchaseSales table
            List<PurchaseSalesModel> purchaseSalesData = reportsRepository.GetPurchasedata();

            return View(purchaseSalesData);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult MedToExpire()
        {
            // Call the stored procedure to get data from PurchaseSales table
            List<ExpiringMedicineModel> purchaseSalesData = reportsRepository.GetExpiringMedicinedata();

            return View(purchaseSalesData);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult MedOutofStock()
        {
            // Call the stored procedure to get data from PurchaseSales table 
            List<ExpiringMedicineModel> purchaseSalesData = reportsRepository.GetMedOutofStockdata();

            return View(purchaseSalesData);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult PatientMedChkList()
        {
            // Call the stored procedure to get data from PurchaseSales table PatientPurchaseModel
            List<PatientPurchaseModel> purchaseSalesData = reportsRepository.GetPatientMedChkList();

            return View(purchaseSalesData);
        }
    }
}
