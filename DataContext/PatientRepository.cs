using ClinicalPharmaSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using System;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Microsoft.AspNet.Identity;
using ClinicalPharmaSystem.Models.PatientView;
using System.Data.Common;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Web.WebPages;

namespace ClinicalPharmaSystem.DataContext
{
    public class PatientRepository
    {
        private readonly string connectionString;

        public PatientRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public PatientView ViewPatient(string patientId,string userName, string datepickerFrom, string datepickerTo)
        {
            PatientView patients = new PatientView();
            patients.patientInfo = GetPatientInfo(patientId, datepickerFrom, datepickerTo);
            patients.patientHealthData = GetHealthData(patientId, datepickerFrom, datepickerTo);
            patients.patientDiseaseMetrics = GetDiseaseMetrics(patientId, datepickerFrom, datepickerTo);
            patients.patientMedicalHistory = GetMedicalHistory(patientId, datepickerFrom, datepickerTo);
            patients.patientClinicalNotes = GetClinicalNotes(patientId, datepickerFrom, datepickerTo);
            patients.patientPrescriptions = GetPatientPrescription(patientId, datepickerFrom, datepickerTo);
            patients.doctorDetails = GetDoctorDetails(userName);
            patients.clinicDetails = GetClinicDetails();
            return patients;
        }

        public List<PatientModel> ViewPatientExist(string patientId)
        {
            List<PatientModel> patients = new List<PatientModel>();
            using IDbConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();

            // Replace "Users" with the actual name of your Users table
            var query = "SELECT PatientData.PatientId PatientId,PatientName,MobileNo,Address,Sex,Age,Date,BloodPressure,PulseRate,Weight,SpO2,DiseaseName,DiseasesTestName,Testvalue,ReportDate TestTakenDate,ClinicalNotesText, ClinicalNotes.VisitDate ClinicalNotesVisitDate,\r\nMedicalHistory.MedicalHistoryText MedicalHistoryText,\r\nMedicalHistory.VisitDate MedicalHistoryVisitDate FROM PatientData inner join PatientHealthData on PatientData.PatientId = PatientHealthData.PatientId \r\nleft outer join PatientTestValues on PatientData.PatientId = PatientTestValues.PatientId left outer join T_Diseases_Master on T_Diseases_Master.DiseasesId = PatientTestValues.DiseasesId\r\nleft outer join T_DiseaseTest_Values on T_DiseaseTest_Values.DiseasesId = PatientTestValues.DiseasesId\r\nleft outer join ClinicalNotes on PatientData.PatientId = ClinicalNotes.PatientId \r\nleft outer join MedicalHistory on PatientData.PatientId = MedicalHistory.PatientId WHERE PatientData.PatientId = @PatientId order by PatientTestValues.indexid desc";

            // Execute the query and return a User object or null if not found
            patients = dbConnection.Query<PatientModel>(query, new { PatientId = patientId }).ToList();
            return patients;
        }

        public List<PatientInfo> GetPatientInfo(string patientId, string datepickerFrom, string datepickerTo)
        {
            List<PatientInfo> patients = new List<PatientInfo>();

            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();

                string query = "SELECT PatientId, PatientName, MobileNo, Address, Sex, Age " +
                               "FROM PatientData " +
                               "WHERE ";
                
                    query += "PatientId = @PatientId ";
                    patients = dbConnection.Query<PatientInfo>(
                        query,
                        new { PatientId = patientId }
                    ).ToList();                
            }
            return patients;
        }

        static string ConvertToSQLDateFormat(string inputDate)
        {
            string sqlFormattedDate = string.Empty;
            if (inputDate != null)
            {
                // Parse the input date in 'dd-MM-yyyy' format to DateTime
                DateTime parsedDate = DateTime.ParseExact(inputDate, "dd-MM-yyyy", null);

                // Convert DateTime to 'yyyy-MM-dd' format
                sqlFormattedDate = parsedDate.ToString("yyyy-MM-dd");
            }
            return sqlFormattedDate;
        }

        public List<Models.PatientView.PatientHealthData> GetHealthData(string patientId, string datepickerFrom, string datepickerTo)
        {
            List<Models.PatientView.PatientHealthData> patients = new List<Models.PatientView.PatientHealthData>();

            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();

                string query = "SELECT Date, BloodPressure, PulseRate, Weight, SpO2, CONVERT(VARCHAR(10), CreatedDate, 103) AS CreatedDate " +
                               "FROM PatientHealthData " +
                               "WHERE PatientId = @PatientId ";


                if (datepickerFrom == null)
                {
                    var dates = ParseDateRange(datepickerTo);
                    datepickerFrom = dates.Item1;
                    datepickerTo = dates.Item2;
                }
                string sqlStartDate = ConvertToSQLDateFormat(datepickerFrom);
                string sqlEndDate = ConvertToSQLDateFormat(datepickerTo);
                if (datepickerTo != null)
                {
                    query += "AND CONVERT(datetime, CONVERT(VARCHAR, Date, 23), 102) BETWEEN @datepickerFrom AND @datepickerTo " +
                             "ORDER BY indexid DESC";

                    patients = dbConnection.Query<Models.PatientView.PatientHealthData>(
                        query,
                        new { PatientId = patientId, datepickerFrom = sqlStartDate, datepickerTo = sqlEndDate }).ToList();
                }
                else
                {
                    query += "AND CONVERT(datetime, CONVERT(VARCHAR, Date, 23), 102) = @datepickerFrom " +
                             "ORDER BY indexid DESC";

                    patients = dbConnection.Query<Models.PatientView.PatientHealthData>(
                        query,
                        new { PatientId = patientId, datepickerFrom = sqlStartDate }
                    ).ToList();
                }
            }
            return patients;
        }

        public List<PatientDiseaseMetrics> GetDiseaseMetrics(string patientId, string datepickerFrom, string datepickerTo)
        {
            List<PatientDiseaseMetrics> patients = new List<PatientDiseaseMetrics>();

            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();

                string query = "SELECT PatientData.PatientId, DiseaseName, DiseasesTestName, Testvalue, " +
                               "CASE " +
                               "   WHEN Testvalue BETWEEN Ref_NormalStartValue AND Ref_NormalEndValue THEN 'Normal' " +
                               "   WHEN Testvalue BETWEEN Ref_AboveNormalStartValue AND Ref_AboveNormalEndValue AND Gender = PatientData.Sex THEN 'Above Normal - Alarmning' " +
                               "   WHEN Testvalue BETWEEN Ref_HighStartValue AND Ref_HighEndValue AND Gender = PatientData.Sex THEN 'High - Moderate Risk' " +
                               "   WHEN Testvalue BETWEEN Ref_HigherStartValue AND Ref_HigherEndValue AND Gender = PatientData.Sex THEN 'Very High - High Risk' " +
                               "END AS 'Status', " +
                               "CONVERT(VARCHAR(10), PatientTestValues.CreatedDate, 103) AS TestTakenDate " +
                               "FROM PatientData " +
                               "INNER JOIN PatientTestValues ON PatientData.PatientId = PatientTestValues.PatientId " +
                               "INNER JOIN T_Diseases_Master ON PatientTestValues.DiseasesId = T_Diseases_Master.DiseasesId " +
                               "INNER JOIN T_DiseaseTest_Values ON T_DiseaseTest_Values.DiseasesTestId = PatientTestValues.DiseasesTestId " +
                               "WHERE PatientData.PatientId = @PatientId ";
                if (datepickerFrom == null)
                {
                    var dates = ParseDateRange(datepickerTo);
                    datepickerFrom = dates.Item1;
                    datepickerTo = dates.Item2;
                }
                string sqlStartDate = ConvertToSQLDateFormat(datepickerFrom);
                string sqlEndDate = ConvertToSQLDateFormat(datepickerTo);
                if (datepickerTo != null)
                {
                    query += "AND CONVERT(datetime, CONVERT(VARCHAR, PatientTestValues.CreatedDate, 23), 102) BETWEEN @datepickerFrom AND @datepickerTo";
                }
                else
                {
                    query += "AND CONVERT(datetime, CONVERT(VARCHAR, PatientTestValues.CreatedDate, 23), 102) = @datepickerFrom";
                }

                patients = dbConnection.Query<PatientDiseaseMetrics>(
                    query,
                    new { PatientId = patientId, datepickerFrom = sqlStartDate, datepickerTo = sqlEndDate }
                ).ToList();
            }

            return patients;
        }

        public List<PatientClinicalNotes> GetClinicalNotes(string patientId, string datepickerFrom, string datepickerTo)
        {
            List<PatientClinicalNotes> patients = new List<PatientClinicalNotes>();

            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();

                string query = "SELECT PatientId, ClinicalNotesText, CONVERT(VARCHAR(10), VisitDate, 103) AS ClinicalNotesVisitDate " +
                               "FROM ClinicalNotes " +
                               "WHERE PatientId = @PatientId ";

                if (datepickerFrom == null)
                {
                    var dates = ParseDateRange(datepickerTo);
                    datepickerFrom = dates.Item1;
                    datepickerTo = dates.Item2;
                    query += "AND CONVERT(datetime, CONVERT(VARCHAR, VisitDate, 23), 102) BETWEEN @datepickerFrom AND @datepickerTo";
                }
                else
                {
                    query += "AND CONVERT(datetime, CONVERT(VARCHAR, VisitDate, 23), 102) = @datepickerFrom";
                }
                string sqlStartDate = ConvertToSQLDateFormat(datepickerFrom);
                string sqlEndDate = ConvertToSQLDateFormat(datepickerTo);

                query += " ORDER BY ClinicalNotesId DESC";

                patients = dbConnection.Query<PatientClinicalNotes>(
                    query,
                    new { PatientId = patientId, datepickerFrom = sqlStartDate, datepickerTo = sqlEndDate }
                ).ToList();
            }

            return patients;
        }

        public List<PatientMedicalHistory> GetMedicalHistory(string patientId, string datepickerFrom, string datepickerTo)
        {
            List<PatientMedicalHistory> patients = new List<PatientMedicalHistory>();

            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();

                string query = "SELECT PatientId, MedicalHistoryText, CONVERT(VARCHAR(10), VisitDate, 103) AS MedicalHistoryVisitDate " +
                               "FROM MedicalHistory " +
                               "WHERE PatientId = @PatientId ";

                if (datepickerFrom == null)
                {
                    var dates = ParseDateRange(datepickerTo);
                    datepickerFrom = dates.Item1;
                    datepickerTo = dates.Item2;
                    query += "AND CONVERT(datetime, CONVERT(VARCHAR, VisitDate, 23), 102) BETWEEN @datepickerFrom AND @datepickerTo";
                }
                else
                {
                    query += "AND CONVERT(datetime, CONVERT(VARCHAR, VisitDate, 23), 102) = @datepickerFrom";
                }
                string sqlStartDate = ConvertToSQLDateFormat(datepickerFrom);
                string sqlEndDate = ConvertToSQLDateFormat(datepickerTo);
                query += " ORDER BY MedicalHistoryId DESC";

                patients = dbConnection.Query<PatientMedicalHistory>(
                    query,
                    new { PatientId = patientId, datepickerFrom = sqlStartDate, datepickerTo = sqlEndDate }
                ).ToList();
            }

            return patients;
        }
        public List<PatientPrescription> GetPatientPrescription(string patientId, string datepickerFrom, string datepickerTo)
        {
            List<PatientPrescription> patients = new List<PatientPrescription>();

            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();

                string query = "SELECT Name, Frequency, Instructions, Days, CONVERT(VARCHAR(10), CreatedDate, 103) AS CreatedDate " +
                               "FROM PatientPrescription " +
                               "WHERE PatientId = @PatientId ";

                if (datepickerFrom == null)
                {
                    var dates = ParseDateRange(datepickerTo);
                    datepickerFrom = dates.Item1;
                    datepickerTo = dates.Item2;
                    query += "AND CONVERT(datetime, CONVERT(VARCHAR, CreatedDate, 23), 102) BETWEEN @datepickerFrom AND @datepickerTo";
                }
                else
                {
                    query += "AND CONVERT(datetime, CONVERT(VARCHAR, CreatedDate, 23), 102) = @datepickerFrom";
                }
                string sqlStartDate = ConvertToSQLDateFormat(datepickerFrom);
                string sqlEndDate = ConvertToSQLDateFormat(datepickerTo);
                query += " ORDER BY SerialNumber DESC";

                patients = dbConnection.Query<PatientPrescription>(
                    query,
                    new { PatientId = patientId, datepickerFrom = sqlStartDate, datepickerTo = sqlEndDate }
                ).ToList();
            }

            return patients;
        }

        public DoctorDetails GetDoctorDetails(string userName)
        {
            using IDbConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();

            // Replace "Users" with the actual name of your Users table
            var query = "select DoctorName,DoctorStudy,DoctorSpeciality,DoctorContactAddress,ContactMobile,ContactNumber,RegNo from Users inner join Doctors on Id=DoctorId where UserName =@userName";

            // Execute the query and return a User object or null if not found
            DoctorDetails doctor = dbConnection.QueryFirstOrDefault<DoctorDetails>(query, new { userName = userName });
            return doctor;
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

        public int SavePatient(PatientModel patient)
        {
            int PatientId = 0;

            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                var query = "INSERT INTO PatientData (PatientName, MobileNo, Address, Sex, Age) " +
                           "OUTPUT INSERTED.PatientId " +  // Use the OUTPUT clause to get the last inserted identity value
                           "VALUES (@PatientName, @MobileNo, @Address, @Sex, @Age);";

                PatientId = dbConnection.QuerySingle<int>(query, new
                {
                    patient.PatientName,
                    patient.MobileNo,
                    patient.Address,
                    patient.Sex,
                    patient.Age
                });

                var query1 = "INSERT INTO PatientHealthData (PatientId, Date, BloodPressure, PulseRate, Weight, SpO2) " +
                            "OUTPUT INSERTED.IndexId " +  // Use the OUTPUT clause to get the last inserted identity value
                            "VALUES (@PatientId, @Date, @BloodPressure, @PulseRate, @Weight, @SpO2);";

                dbConnection.QuerySingle<int>(query1, new
                {
                    PatientId,
                    patient.Date,
                    patient.BloodPressure,
                    patient.PulseRate,
                    patient.Weight,
                    patient.SpO2
                });
            }
            return PatientId;
        }

        public int SavePatientRows(string patientId, List<PatientHealthDataModel> rows)
        {
            if (string.IsNullOrEmpty(patientId) || rows == null || rows.Count == 0)
            {
                return 0;
            }

            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                // Loop through the rows and save the data for each row
                foreach (var row in rows)
                {
                    var query1 = "INSERT INTO PatientHealthData (PatientId, Date, BloodPressure, PulseRate, Weight, SpO2) " +
                        "OUTPUT INSERTED.IndexId " + // Use the OUTPUT clause to get the last inserted identity value
                        "VALUES (@PatientId, @Date, @BloodPressure, @PulseRate, @Weight, @SpO2);";

                    // Execute the query to save the row data
                    var indexId = dbConnection.QuerySingle<int>(query1, new
                    {
                        PatientId = patientId,
                        Date = row.VisitDate,
                        BloodPressure = row.BloodPressure,
                        PulseRate = row.PulseRate,
                        Weight = row.Weight,
                        SpO2 = row.SpO2
                    });

                }
            }

            return 1;
        }

        public int getPatientId(int IndexId)
        {
            using IDbConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();

            // Replace "Users" with the actual name of your Users table
            var query = "SELECT PatientId FROM PatientHealthData WHERE IndexId = @IndexId";

            // Execute the query and return a User object or null if not found
            var patientModel = dbConnection.QueryFirstOrDefault<PatientModel>(query, new { IndexId = IndexId });
            return patientModel.PatientId;
        }

        public int SaveTestValueResults(DiseaseTestModel rows)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                var query = "INSERT INTO PatientTestValues (PatientId, DiseasesId, DiseasesTestId, Testvalue, CreatedDate,ReportDate) " +
                    "VALUES (@PatientId, @DiseasesId, @DiseasesTestId, @Testvalue, @CreatedDate,@ReportDate);";

                // Execute the query to save the row data
                dbConnection.Execute(query, new
                {
                    PatientId = rows.PatientId,
                    DiseasesId = rows.DiseasesId,
                    DiseasesTestId = rows.DiseasesTestId,
                    Testvalue = rows.DiseaseTestValue,
                    CreatedDate = DateTime.Now,
                    ReportDate = rows.ReportDate
                });
            }

            return 1; // Indicates successful insertion
        }

        public int SavePatientHealthData(PatientHealthDataModel healthData)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                var query = "INSERT INTO PatientHealthData (PatientId, Date, BloodPressure, PulseRate, Weight,SpO2,CreatedDate) " +
                    "VALUES (@PatientId, @Date, @BloodPressure, @PulseRate, @Weight,@SpO2,@CreatedDate);";

                // Execute the query to save the row data
                dbConnection.Execute(query, new
                {
                    PatientId = healthData.PatientId,
                    Date = DateTime.Now,
                    BloodPressure = healthData.BloodPressure,
                    PulseRate = healthData.PulseRate,
                    Weight = healthData.Weight,
                    SpO2 = healthData.SpO2,
                    CreatedDate = DateTime.Now
                });
            }

            return 1; // Indicates successful insertion
        }

        public int SavePrescription(int patientId, List<PatientPrescription> prescriptions,string CreatedBy)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                foreach (var row in prescriptions)
                {
                    var query = "INSERT INTO PatientPrescription (PatientId, Name, Frequency, Instructions, Days,CreatedBy) " +
                    "VALUES (@PatientId, @Name, @Frequency, @Instructions, @Days,@CreatedBy);";

                    // Execute the query to save the row data
                    dbConnection.Execute(query, new
                    {

                        PatientId = patientId,
                        Name = row.Name,
                        Frequency = row.Frequency,
                        Instructions = row.Instructions,
                        Days = row.Days,
                        CreatedBy = CreatedBy
                    });
                }
            }

            return 1;
        }

        public List<PatientPrescription> GetPrescriptions(int PatientId)
        {
            List<PatientPrescription> diseaseTests = new List<PatientPrescription>();
            using IDbConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();

            // Replace "Users" with the actual name of your Users table
            var query = "select SerialNumber,PatientId,Name,Frequency,Instructions,Days,CONVERT(VARCHAR(10), CreatedDate, 103) VisitDate from PatientPrescription where PatientId = @PatientId and IsDeleted=0 order by SerialNumber desc";
            
            // Execute the query and return a User object or null if not found
            diseaseTests = dbConnection.Query<PatientPrescription>(query, new { PatientId = PatientId }).ToList();
            return diseaseTests;
        }

        public int UpdatePrescription(int SerialNumber, string Name,string Frequency,string Instructions, string Days, string username)
        {
            if (Name == "delete")
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    var query = "update PatientPrescription set IsDeleted=1,ModifiedDate=@ModifiedDate,ModifiedBy=@ModifiedBy where SerialNumber=@SerialNumber;";

                    // Execute the query to save the row data
                    dbConnection.Execute(query, new
                    {
                        SerialNumber = SerialNumber,
                        ModifiedDate = DateTime.Now,
                        ModifiedBy = username
                    });
                }
            }
            else
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    var query = "update PatientPrescription set Name=@Name,Frequency=@Frequency,Instructions=@Instructions,Days=@Days,ModifiedDate=@ModifiedDate,ModifiedBy=@ModifiedBy where SerialNumber=@SerialNumber;";

                    // Execute the query to save the row data
                    dbConnection.Execute(query, new
                    {
                        SerialNumber = SerialNumber,
                        Name = Name,
                        Frequency = Frequency,
                        Instructions= Instructions,
                        Days=Days,
                        ModifiedDate = DateTime.Now,
                        ModifiedBy = username
                    });
                }
            }
            return 1; // Indicates successful insertion

        }

        public List<DiseaseListModel> GetDiseaseTests(string diseaseid)
        {
            List<DiseaseListModel> diseaseTests = new List<DiseaseListModel>();
            using IDbConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();

            // Replace "Users" with the actual name of your Users table
            var query = "select distinct DiseasesTestName  from [dbo].[T_DiseaseTest_Values] where isactive=1 and DiseasesId=@DiseasesTestId";

            // Execute the query and return a User object or null if not found
            diseaseTests = dbConnection.Query<DiseaseListModel>(query, new { DiseasesTestId = diseaseid }).ToList();
            return diseaseTests;
        }

        public int SaveClinicalNotes(ClinicalNotes rows,string username)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                var query = "INSERT INTO ClinicalNotes (PatientId, ClinicalNotesText, VisitDate,CreatedDate,CreatedBy) " +
                    "VALUES (@PatientId, @ClinicalNotesText, @VisitDate,@CreatedDate,@CreatedBy);";

                // Execute the query to save the row data
                dbConnection.Execute(query, new
                {
                    PatientId = rows.PatientId,
                    ClinicalNotesText = rows.ClinicalNotesText,
                    VisitDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CreatedBy = username
                });
            }

            return 1; // Indicates successful insertion
        }

        public int SaveMedicalHistory(Medicalhistory rows)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                var query = "INSERT INTO MedicalHistory (PatientId, MedicalHistoryText, VisitDate) " +
                    "VALUES (@PatientId, @ClinicalNotesText, @VisitDate);";

                // Execute the query to save the row data
                dbConnection.Execute(query, new
                {
                    PatientId = rows.PatientId,
                    ClinicalNotesText = rows.MedicalHistoryText,
                    VisitDate = DateTime.Now
                });
            }

            return 1; // Indicates successful insertion

        }

        public List<ClinicalNotes> GetClinicalNotes(int PatientId)
        {
            List<ClinicalNotes> diseaseTests = new List<ClinicalNotes>();
            using IDbConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();

            // Replace "Users" with the actual name of your Users table
            var query = "select ClinicalNotesId,PatientId,ClinicalNotesText,CONVERT(VARCHAR(10), VisitDate, 103) VisitDate from ClinicalNotes where PatientId=@PatientId and isnull(IsDeleted,0) <> 1 order by ClinicalNotesId desc";

            // Execute the query and return a User object or null if not found
            diseaseTests = dbConnection.Query<ClinicalNotes>(query, new { PatientId = PatientId }).ToList();
            return diseaseTests;
        }

        public int UpdateClinicalNote(int ClinicalNotesId, string ClinicalNotesText, string username)
        {
            if (ClinicalNotesText == "delete") {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    var query = "update ClinicalNotes set IsDeleted=1,ModifiedDate=@ModifiedDate,ModifiedBy=@ModifiedBy where ClinicalNotesId=@ClinicalNotesId;";

                    // Execute the query to save the row data
                    dbConnection.Execute(query, new
                    {
                        ClinicalNotesId = ClinicalNotesId,
                        ModifiedDate = DateTime.Now,
                        ModifiedBy = username
                });
                }
            }
            else
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    var query = "update ClinicalNotes set ClinicalNotesText=@ClinicalNotesText,ModifiedDate=@ModifiedDate,ModifiedBy=@ModifiedBy where ClinicalNotesId=@ClinicalNotesId;";

                    // Execute the query to save the row data
                    dbConnection.Execute(query, new
                    {
                        ClinicalNotesId = ClinicalNotesId,
                        ClinicalNotesText = ClinicalNotesText,
                        ModifiedDate = DateTime.Now,
                        ModifiedBy = username
                    });
                }
            }
            return 1; // Indicates successful insertion

        }

        public List<Medicalhistory> GetMedicalNote(int PatientId)
        {
            List<Medicalhistory> diseaseTests = new List<Medicalhistory>();
            using IDbConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();

            // Replace "Users" with the actual name of your Users table
            var query = "select MedicalHistoryId,PatientId,MedicalHistoryText,CONVERT(VARCHAR(10), VisitDate, 103) VisitDate from MedicalHistory where PatientId=@PatientId and isnull(IsDeleted,0) <> 1 order by MedicalHistoryId desc";

            // Execute the query and return a User object or null if not found
            diseaseTests = dbConnection.Query<Medicalhistory>(query, new { PatientId = PatientId }).ToList();
            return diseaseTests;
        }

        public int UpdateMedicalNote(int MedicalHistoryId, string MedicalHistoryText, string username)
        {
            if (MedicalHistoryText == "delete")
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    var query = "update MedicalHistory set IsDeleted=1,ModifiedDate=@ModifiedDate,ModifiedBy=@ModifiedBy where MedicalHistoryId=@MedicalHistoryId;";

                    // Execute the query to save the row data
                    dbConnection.Execute(query, new
                    {
                        MedicalHistoryId = MedicalHistoryId,
                        ModifiedDate = DateTime.Now,
                        ModifiedBy = username
                    });
                }
            }
            else
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    var query = "update MedicalHistory set MedicalHistoryText=@MedicalHistoryText,ModifiedDate=@ModifiedDate,ModifiedBy=@ModifiedBy where MedicalHistoryId=@MedicalHistoryId;";

                    // Execute the query to save the row data
                    dbConnection.Execute(query, new
                    {
                        MedicalHistoryId = MedicalHistoryId,
                        MedicalHistoryText = MedicalHistoryText,
                        ModifiedDate = DateTime.Now,
                        ModifiedBy = username
                    });
                }
            }
            return 1; // Indicates successful insertion

        }

        public static (string, string) ParseDateRange(string dateRangeString)
        {
            string[] dateParts = dateRangeString.Split(new string[] { " to " }, StringSplitOptions.RemoveEmptyEntries);

            if (dateParts.Length == 2)
            {
                if (DateTime.TryParseExact(dateParts[0], "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fromDate) &&
                    DateTime.TryParseExact(dateParts[1], "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime toDate))
                {
                    string formattedFromDate = fromDate.ToString("dd-MM-yyyy");
                    string formattedToDate = toDate.ToString("dd-MM-yyyy");

                    return (formattedFromDate, formattedToDate);
                }
            }

            return (null, null);
        }


    }
}