using ClinicalPharmaSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using System;
using Newtonsoft.Json.Linq;
using System.Reflection;

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
            var query = "SELECT top 3 * FROM PatientData inner join PatientHealthData on PatientData.PatientId = PatientHealthData.PatientId  \r\ninner join PatientTestValues on PatientData.PatientId = PatientTestValues.PatientId  WHERE PatientData.PatientId = @PatientId order by PatientTestValues.indexid desc";

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

        public int SavePatientRows(string patientId, List<PatientHealthDataModel> rows )
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

        public int SaveTestValueResults( DiseaseTestModel rows)
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
        
    }
    
}
