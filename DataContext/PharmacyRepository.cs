using ClinicalPharmaSystem.Models;
using ClinicalPharmaSystem.Models.PatientView;
using ClinicalPharmaSystem.Models.Pharmacy;
using Dapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;

namespace ClinicalPharmaSystem.DataContext
{
    public class PharmacyRepository
    {
        private readonly string connectionString;
        private readonly IHttpContextAccessor _contextAccessor;

        public PharmacyRepository(string connectionString, IHttpContextAccessor contextAccessor)
        {
            this.connectionString = connectionString;
            _contextAccessor = contextAccessor;
        }

        public int Insertsupplier(Models.Pharmacy.MedicineSupplier user, string username)
        {
            int Statusid = 0;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO MedicineSupplier (SupplierName, ContactPerson, Email, Phone, Address, CreatedBy, CreatedDate, Isdeleted) VALUES (@SupplierName, @ContactPerson, @Email, @Phone, @Address, @CreatedBy, @CreatedDate, @Isdeleted)";

                connection.Execute(sql, new
                {
                    user.SupplierName,
                    user.ContactPerson,
                    user.Email,
                    user.Phone,
                    user.Address,
                    CreatedBy = username,
                    CreatedDate = DateTime.Now,
                    Isdeleted = false 
                });

                Statusid = 1;
            }
            return Statusid;
        }


        public List<MedicineSupplier> GetSupplierData()
        {
            List<MedicineSupplier> diseaseTests = new List<MedicineSupplier>();
            using IDbConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();

            // Replace "Users" with the actual name of your Users table
            var query = "select * from MedicineSupplier where Isdeleted=0";

            // Execute the query and return a User object or null if not found
            diseaseTests = dbConnection.Query<MedicineSupplier>(query).ToList();
            return diseaseTests;
        }

        public IEnumerable<MedicineSupplier> GetSupplierDataForEntry(string term)
        {
            using IDbConnection dbConnection = new SqlConnection(connectionString);

            dbConnection.Open();

            // Replace "Users" with the actual name of your Users table
            var query = "SELECT * FROM MedicineSupplier WHERE SupplierName LIKE @searchTerm and Isdeleted = 0";

            // You need to prepend and append '%' to the term to perform a partial match
            term = '%' + term + '%';

            // Execute the query and return a collection of DiseaseAutoComplete objects
            IEnumerable<MedicineSupplier> diseaseModels = dbConnection.Query<MedicineSupplier>(query, new { searchTerm = term });
            return diseaseModels;
        }

        public int UpdateSupplierData(int SupplierID, string SupplierName, string ContactPerson
            , string Email, string Phone, string Address,string username,string flag)
        {
            if (flag == "update")
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    var query = "update MedicineSupplier set SupplierName=@SupplierName,ContactPerson=@ContactPerson,Email=@Email,Phone=@Phone,Address=@Address,ModifiedDate=@ModifiedDate,ModifiedBy=@ModifiedBy where SupplierID=@SupplierID;";

                    // Execute the query to save the row data
                    dbConnection.Execute(query, new
                    {
                        SupplierID = SupplierID,
                        SupplierName = SupplierName,
                        ContactPerson = ContactPerson,
                        Email = Email,
                        Phone = Phone,
                        Address = Address,
                        ModifiedDate = DateTime.Now,
                        ModifiedBy = username
                    });
                }
            }
            else if (flag == "delete")
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    var query = "update MedicineSupplier set Isdeleted=@Isdeleted,ModifiedDate=@ModifiedDate,ModifiedBy=@ModifiedBy where SupplierID=@SupplierID;";

                    // Execute the query to save the row data
                    dbConnection.Execute(query, new
                    {
                        SupplierID = SupplierID,
                        Isdeleted = true,
                        ModifiedDate = DateTime.Now,
                        ModifiedBy = username
                    });
                }
            }
            return 1;
        }

        //medicine category 

        public int InsertMedicineCategory(MedicineCategory medicineCategory, string CreatedBy)
        {
            int Statusid = 0;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO MedicineCategory (CategoryName,CreatedBy, CreatedDate, Isdeleted) VALUES (@CategoryName, @CreatedBy, @CreatedDate, @Isdeleted)";

                connection.Execute(sql, new
                {
                    medicineCategory.CategoryName,
                    CreatedBy = CreatedBy,
                    CreatedDate = DateTime.Now,
                    Isdeleted = false
                });

                Statusid = 1;
            }
            return Statusid;
        }


        public List<MedicineCategory> GetMedicineCategory()
        {
            List<MedicineCategory> diseaseTests = new List<MedicineCategory>();
            using IDbConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();

            // Replace "Users" with the actual name of your Users table
            var query = "select * from MedicineCategory where Isdeleted=0";

            // Execute the query and return a User object or null if not found
            diseaseTests = dbConnection.Query<MedicineCategory>(query).ToList();
            return diseaseTests;
        }

        public IEnumerable<MedicineCategory> GetMedicineCategoryForEntry(string term)
        {
            using IDbConnection dbConnection = new SqlConnection(connectionString);

            dbConnection.Open();

            // Replace "Users" with the actual name of your Users table
            var query = "SELECT * FROM MedicineCategory WHERE CategoryName LIKE @searchTerm and Isdeleted = 0";

            // You need to prepend and append '%' to the term to perform a partial match
            term = '%' + term + '%';

            // Execute the query and return a collection of DiseaseAutoComplete objects
            IEnumerable<MedicineCategory> diseaseModels = dbConnection.Query<MedicineCategory>(query, new { searchTerm = term });
            return diseaseModels;
        }

        public int UpdateMedicineCategory(int CategoryID, string CategoryName,string username, string flag)
        {
            if (flag == "update")
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    var query = "update MedicineCategory set CategoryName=@CategoryName,ModifiedDate=@ModifiedDate,ModifiedBy=@ModifiedBy where CategoryID=@CategoryID;";

                    // Execute the query to save the row data
                    dbConnection.Execute(query, new
                    {
                        CategoryID = CategoryID,
                        CategoryName = CategoryName,
                        ModifiedDate = DateTime.Now,
                        ModifiedBy = username
                    });
                }
            }
            else if (flag == "delete")
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    var query = "update MedicineCategory set Isdeleted=@Isdeleted,ModifiedDate=@ModifiedDate,ModifiedBy=@ModifiedBy where CategoryID=@CategoryID;";

                    // Execute the query to save the row data
                    dbConnection.Execute(query, new
                    {
                        CategoryID = CategoryID,
                        Isdeleted = true,
                        ModifiedDate = DateTime.Now,
                        ModifiedBy = username
                    });
                }
            }
            return 1;
        }

        // New Medicine

        public int InsertMedicine(Medicine medicine, string CreatedBy,int SupplierID, int CategoryID)
        {
            int Statusid = 0;
            int NumberOfTablets = 0;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Medicines (SupplierID,CategoryID,MedicineName,NumberOfStrips,PricePerStrip,NumberOfTablets,Container,TabletCountPerStrip,RouteofIntake,Strengths,CreatedBy, CreatedDate, Isdeleted,ManufacturingDate,ExpiryDate) VALUES " +
                    "(@SupplierID,@CategoryID,@MedicineName,@NumberOfStrips,@PricePerStrip,@NumberOfTablets,@Container,@TabletCountPerStrip,@RouteofIntake,@Strengths, @CreatedBy, @CreatedDate, @Isdeleted,@ManufacturingDate,@ExpiryDate)";
                NumberOfTablets = Convert.ToInt32(medicine.NumberOfStrips * medicine.PricePerStrip);
                connection.Execute(sql, new
                {
                    SupplierID,
                    CategoryID,
                    medicine.MedicineName,
                    medicine.NumberOfStrips,
                    medicine.PricePerStrip,
                    @NumberOfTablets = NumberOfTablets,
                    medicine.Container,
                    medicine.TabletCountPerStrip,
                    medicine.RouteOfIntake,
                    medicine.Strengths,
                    CreatedBy = CreatedBy,
                    CreatedDate = DateTime.Now,
                    Isdeleted = false,
                    medicine.ManufacturingDate,
                    medicine.ExpiryDate
                });

                Statusid = 1;
            }
            return Statusid;
        }


        public List<Medicine> GetMedicines()
        {
            List<Medicine> diseaseTests = new List<Medicine>();
            using IDbConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();

            // Replace "Users" with the actual name of your Users table
            var query = "select * from Medicines where Isdeleted=0";

            // Execute the query and return a User object or null if not found
            diseaseTests = dbConnection.Query<Medicine>(query).ToList();
            return diseaseTests;
        }

        public List<Medicine> GetMedicinesByName(string MedicineName)
        {
            List<Medicine> medicines = new List<Medicine>();
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();

                // Replace "Medicines" with the actual name of your table
                var query = "SELECT * FROM Medicines WHERE MedicineName LIKE '%' + @MedicineName + '%' AND Isdeleted = 0";

                // Execute the query and return a list of Medicine objects
                medicines = dbConnection.Query<Medicine>(query, new { MedicineName }).ToList();
            }
            return medicines;
        }
        
        public List<PatientModel> GetPatienDetailsById(string PatientId)
        {
            List<PatientModel> medicines = new List<PatientModel>();
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();

                // Replace "Medicines" with the actual name of your table
                var query = "SELECT * FROM PatientData WHERE PatientName LIKE '%' + @PatientId + '%'";

                // Execute the query and return a list of Medicine objects
                medicines = dbConnection.Query<PatientModel>(query, new { PatientId }).ToList();
            }
            return medicines;
        }

        public List<Medicine> GetMedicineById(int MedicineID)
        {
            List<Medicine> medicines = new List<Medicine>();

            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();

                // Create the SQL query with parameters
                var query = "select MC.CategoryName,MS.SupplierName,M.MedicineID,M.MedicineName,M.NumberOfStrips,M.NumberOfTablets,M.NumberOfTablets,M.PricePerStrip,M.RouteofIntake,M.Strengths,M.ManufacturingDate,M.ExpiryDate,M.CategoryID,M.SupplierID,M.Container,M.TabletCountPerStrip from Medicines M inner join MedicineSupplier MS on M.SupplierID = MS.SupplierID inner join MedicineCategory MC on MC.CategoryID = M.CategoryID where M.MedicineID=@MedicineID AND M.Isdeleted = 0";

                // Execute the query and pass parameters using an anonymous object
                medicines = dbConnection.Query<Medicine>(query, new { MedicineID }).ToList();
            }

            return medicines;
        }

        public List<Medicine> SearchMedicine(string MedicineName, string SupplierName, string CategoryName)
        {
            List<Medicine> medicines = new List<Medicine>();

            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();

                // Start building the base query
                var query = "SELECT MC.CategoryName, MS.SupplierName, M.MedicineID, M.MedicineName, M.NumberOfStrips, M.NumberOfTablets, M.NumberOfTablets, M.PricePerStrip, M.RouteofIntake, M.Strengths, M.ManufacturingDate, M.ExpiryDate FROM Medicines M INNER JOIN MedicineSupplier MS ON M.SupplierID = MS.SupplierID INNER JOIN MedicineCategory MC ON MC.CategoryID = M.CategoryID WHERE M.Isdeleted = 0";

                // Create a list to hold WHERE clause conditions
                var conditions = new List<string>();

                // Add conditions based on non-empty parameters
                if (!string.IsNullOrEmpty(MedicineName))
                    conditions.Add("M.MedicineName LIKE @MedicineName");

                if (!string.IsNullOrEmpty(SupplierName))
                    conditions.Add("MS.SupplierName LIKE @SupplierName");

                if (!string.IsNullOrEmpty(CategoryName))
                    conditions.Add("MC.CategoryName LIKE @CategoryName");

                // Append WHERE clause if conditions exist
                if (conditions.Any())
                {
                    query += " AND " + string.Join(" OR ", conditions);
                }

                // Execute the query and pass parameters using an anonymous object
                medicines = dbConnection.Query<Medicine>(query, new { MedicineName = "%" + MedicineName + "%", SupplierName = "%" + SupplierName + "%", CategoryName = "%" + CategoryName + "%" }).ToList();
            }

            return medicines;
        }

        public int UpdateMedicineCount (List<MedicineData> medicineData,string username,string Rollback)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                foreach (var data in medicineData)
                {
                    var parameters = new
                    {
                        MedicineID = data.MedicineID,
                        UserRequiredCount = data.Quantity,
                        ModifiedBy = username,
                        Rollback = Rollback
                    };

                    dbConnection.Execute("UpdateMedicineDetails", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            return 1;
        }
        public int InsertPurchasePatientInfo(List<PatienBillingInfo> medicineData, string username)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                foreach (var data in medicineData)
                {
                    var parameters = new
                    {
                        PatientID = data.PatientID,
                        PatientName = data.PatientName,
                        MobileNo = data.MobileNo,
                        InvoiceID = data.InvoiceID,
                        CreatedBy = username
                    };

                    dbConnection.Execute("InsertPatientBillingInfo", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            return 1;
        }

        public int InsertPurchaseSales(List<PurchaseSales> medicineData, string username)
        {
            int lastInsertedSaleId = 0;
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                foreach (var data in medicineData)
                {
                    DateTime.TryParseExact(data.BillDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate);
                    var parameters = new DynamicParameters();
                    parameters.Add("@SaleAmount",Convert.ToDecimal(data.SaleAmount));
                    parameters.Add("@SaleDate", parsedDate);
                    parameters.Add("@CreatedBy", username);
                    parameters.Add("@ModeOfPayment", data.ModeOfPayment);
                    parameters.Add("Comments", data.Comments);
                    parameters.Add("@LastInsertedSaleId", dbType: DbType.Int32, direction: ParameterDirection.Output); // Output parameter for SaleId

                    // Execute the stored procedure
                    dbConnection.Execute("InsertPurchaseSaleData", parameters, commandType: CommandType.StoredProcedure);

                    // Retrieve the returned SaleId
                    lastInsertedSaleId = parameters.Get<int>("@LastInsertedSaleId");
                }
            }
            return lastInsertedSaleId;
        }

        public int InsertPurchasemedicineInfo(List<PurchaseBillingInfo> medicineData, string username)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                foreach (var data in medicineData)
                {
                    var parameters = new
                    {
                        MedicineID = data.MedicineID,
                        MedicineName = data.MedicineName,
                        MrpPerUnit = data.MrpPerUnit,
                        PurchasedQuantity = data.PurchasedQuantity,
                        Discount = data.Discount,
                        GST = data.GST,
                        Total = data.Total,
                        ExpiryDate= data.ExpiryDate,
                        CreatedBy = username,
                        InvoiceID = data.InvoiceID
                    };

                    dbConnection.Execute("InsertPurchaseMedicineData", parameters, commandType: CommandType.StoredProcedure);

                }
            }
            return 1;
        }        

        public int UpdateMedicine(int MedicineID, int CategoryID, int SupplierID, string MedicineName,int NumberOfStrips,double PricePerStrip,int NumberOfTablets,string RouteofIntake,string Strengths, string username, string Container, int TabletCountPerStrip, string flag)
        {
            if (flag == "update")
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    var query = "update Medicines set SupplierID=@SupplierID,CategoryID=@CategoryID,MedicineName=@MedicineName,NumberOfStrips=@NumberOfStrips,PricePerStrip=@PricePerStrip,NumberOfTablets=@NumberOfTablets,RouteofIntake=@RouteofIntake,Strengths=@Strengths,ModifiedDate=@ModifiedDate,ModifiedBy=@ModifiedBy,Container=@Container,TabletCountPerStrip=@TabletCountPerStrip where MedicineID=@MedicineID;";

                    // Execute the query to save the row data
                    dbConnection.Execute(query, new
                    {
                        MedicineID = MedicineID,
                        SupplierID = SupplierID,
                        CategoryID = CategoryID,
                        MedicineName= MedicineName,
                        NumberOfStrips = NumberOfStrips,
                        PricePerStrip = PricePerStrip,
                        NumberOfTablets = NumberOfTablets,
                        RouteofIntake = RouteofIntake,
                        Strengths = Strengths,
                        Username = username,
                        ModifiedDate = DateTime.Now,
                        ModifiedBy = username,
                        Container = Container,
                        TabletCountPerStrip = TabletCountPerStrip
                    });
                }
            }
            else if (flag == "delete")
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    var query = "update Medicines set Isdeleted=@Isdeleted,ModifiedDate=@ModifiedDate,ModifiedBy=@ModifiedBy where MedicineID=@MedicineID;";

                    // Execute the query to save the row data
                    dbConnection.Execute(query, new
                    {
                        MedicineID = MedicineID,
                        Isdeleted = true,
                        ModifiedDate = DateTime.Now,
                        ModifiedBy = username
                    });
                }
            }
            return 1;
        }
        public ClinicDetails GetClinicDetails()
        {
            ClinicDetails patients = new ClinicDetails();
            using IDbConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();

            // Replace "Users" with the actual name of your Users table
            var query = "select * from ClinicalInfo";

            // Execute the query and return a User object or null if not found
            patients = dbConnection.QueryFirstOrDefault<ClinicDetails>(query);
            return patients;
        }
    }
}
