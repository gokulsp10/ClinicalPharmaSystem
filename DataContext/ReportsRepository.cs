using ClinicalPharmaSystem.Models.PatientView;
using System.Data.SqlClient;
using System.Data;
using ClinicalPharmaSystem.Models.Reports;
using Dapper;

namespace ClinicalPharmaSystem.DataContext
{
    public class ReportsRepository
    {
        private readonly string connectionString;

        public ReportsRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<PurchaseSalesModel> GetPurchasedata()
        {
            List<PurchaseSalesModel> sales = new List<PurchaseSalesModel>();

            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();

                // Execute the stored procedure using Dapper
                sales = dbConnection.Query<PurchaseSalesModel>("GetPurchaseSalesData", commandType: CommandType.StoredProcedure).ToList();
            }

            return sales;
        }

        public List<ExpiringMedicineModel> GetExpiringMedicinedata()
        {
            List<ExpiringMedicineModel> expiringMedicines = new List<ExpiringMedicineModel>();
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();

                // Execute the stored procedure using Dapper
                expiringMedicines = dbConnection.Query<ExpiringMedicineModel>("GetExpiringMedicine", commandType: CommandType.StoredProcedure).ToList();
            }

            return expiringMedicines;
        }

        public List<ExpiringMedicineModel> GetMedOutofStockdata()
        {
            List<ExpiringMedicineModel> expiringMedicines = new List<ExpiringMedicineModel>();
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();

                // Execute the stored procedure using Dapper 
                expiringMedicines = dbConnection.Query<ExpiringMedicineModel>("GetOutofStockMedicine", commandType: CommandType.StoredProcedure).ToList();
            }

            return expiringMedicines;
        }

        public List<PatientPurchaseModel> GetPatientMedChkList()
        {
            List<PatientPurchaseModel> expiringMedicines = new List<PatientPurchaseModel>();
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();

                // Execute the stored procedure using Dapper GetPatientMedChkList
                expiringMedicines = dbConnection.Query<PatientPurchaseModel>("GetRecentPurchases", commandType: CommandType.StoredProcedure).ToList();
            }

            return expiringMedicines;
        }

    }
}
