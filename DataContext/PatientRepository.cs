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

namespace ClinicalPharmaSystem.DataContext
{
    public class PatientRepository
    {
        private readonly string connectionString;

        public PatientRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<PatientModel> ViewPatient(string patientId)
        {
            List<PatientModel> patients = new List<PatientModel>();
            using IDbConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();

            // Replace "Users" with the actual name of your Users table
            var query = "SELECT top 3 PatientData.PatientId PatientId,PatientName,MobileNo,Address,Sex,Age,Date,BloodPressure,PulseRate,Weight,SpO2,DiseaseName,DiseasesTestName,Testvalue,ReportDate TestTakenDate,ClinicalNotesText, ClinicalNotes.VisitDate ClinicalNotesVisitDate,\r\nMedicalHistory.MedicalHistoryText MedicalHistoryText,\r\nMedicalHistory.VisitDate MedicalHistoryVisitDate FROM PatientData inner join PatientHealthData on PatientData.PatientId = PatientHealthData.PatientId \r\nleft outer join PatientTestValues on PatientData.PatientId = PatientTestValues.PatientId left outer join T_Diseases_Master on T_Diseases_Master.DiseasesId = PatientTestValues.DiseasesId\r\nleft outer join T_DiseaseTest_Values on T_DiseaseTest_Values.DiseasesId = PatientTestValues.DiseasesId\r\nleft outer join ClinicalNotes on PatientData.PatientId = ClinicalNotes.PatientId \r\nleft outer join MedicalHistory on PatientData.PatientId = MedicalHistory.PatientId WHERE PatientData.PatientId = @PatientId order by PatientTestValues.indexid desc";

            // Execute the query and return a User object or null if not found
            patients = dbConnection.Query<PatientModel>(query, new { PatientId = patientId }).ToList();
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
                var query = "INSERT INTO PatientHealthData (PatientId, Date, BloodPressure, PulseRate, Weight,SpO2) " +
                    "VALUES (@PatientId, @Date, @BloodPressure, @PulseRate, @Weight,@SpO2);";

                // Execute the query to save the row data
                dbConnection.Execute(query, new
                {
                    PatientId = healthData.PatientId,
                    Date = DateTime.Now,
                    BloodPressure = healthData.BloodPressure,
                    PulseRate = healthData.PulseRate,
                    Weight = healthData.Weight,
                    SpO2 = healthData.SpO2
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

    }
}