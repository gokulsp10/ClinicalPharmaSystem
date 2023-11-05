using ClinicalPharmaSystem.Models;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace ClinicalPharmaSystem.DataContext
{
    public class MedicalTestRepository
    {
        private readonly string connectionString;

        public MedicalTestRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int SaveDiseaseName(string diseaseName)
        {
            if (string.IsNullOrEmpty(diseaseName) )
            {
                return 0;
            }

            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                var query = "INSERT INTO T_Diseases_Master (DiseaseName, EnteredDate, IsActive) " +
                           "OUTPUT INSERTED.DiseasesId " +  // Use the OUTPUT clause to get the last inserted identity value
                           "VALUES (@diseaseName, @EnteredDate, @IsActive);";

                int DiseasesId = dbConnection.QuerySingle<int>(query, new
                {
                    diseaseName,
                    EnteredDate = DateTime.Now,
                    IsActive = 1
                });

            }

            return 1;
        }

        public int SaveDiseaseTestMasterRows(string diseaseId, List<DiseaseTestModel> rows)
        {
            if (string.IsNullOrEmpty(diseaseId) || rows == null || rows.Count == 0)
            {
                return 0;
            }

            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {   
                // Loop through the rows and save the data for each row
                foreach (var row in rows)
                {
                    string insertQuery = "INSERT INTO T_DiseaseTest_Values (DiseasesTestName, DiseasesId, Ref_NormalStartValue, Ref_NormalEndValue, Ref_AboveNormalStartValue, Ref_AboveNormalEndValue, Ref_HighStartValue, Ref_HighEndValue, Ref_HigherStartValue, Ref_HigherEndValue, EnteredDate, IsActive, Gender) VALUES (@DiseasesTestName, @DiseasesId, @Ref_NormalStartValue, @Ref_NormalEndValue, @Ref_AboveNormalStartValue, @Ref_AboveNormalEndValue, @Ref_HighStartValue, @Ref_HighEndValue, @Ref_HigherStartValue, @Ref_HigherEndValue, @EnteredDate, @IsActive, @Gender)";

                    // Execute the query to save the row data
                    dbConnection.Execute(insertQuery, new
                    {
                        DiseasesTestName = row.DiseasesTestName,
                        DiseasesId = diseaseId, // You need to provide a valid DiseasesId
                        Gender = row.Gender,
                        Ref_NormalStartValue = row.Ref_NormalStartValue,
                        Ref_NormalEndValue = row.Ref_NormalEndValue,
                        Ref_AboveNormalStartValue = row.Ref_AboveNormalStartValue,
                        Ref_AboveNormalEndValue = row.Ref_AboveNormalEndValue,
                        Ref_HighStartValue = row.Ref_HighStartValue,
                        Ref_HighEndValue = row.Ref_HighEndValue,
                        Ref_HigherStartValue = row.Ref_HigherStartValue,
                        Ref_HigherEndValue = row.Ref_HigherEndValue,
                        EnteredDate = DateTime.Now,
                        IsActive = 1
                    });

                }
            }

            return 1;
        }

        public IEnumerable<DiseaseAutoComplete> GetDiseasename(string term)
        {
            using IDbConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();

            // Replace "Users" with the actual name of your Users table
            var query = "SELECT * FROM T_Diseases_Master WHERE DiseaseName LIKE @searchTerm";

            // You need to prepend and append '%' to the term to perform a partial match
            term = '%' + term + '%';

            // Execute the query and return a collection of DiseaseAutoComplete objects
            IEnumerable<DiseaseAutoComplete> diseaseModels = dbConnection.Query<DiseaseAutoComplete>(query, new { searchTerm = term });

            return diseaseModels;
        }

        public IEnumerable<DiseaseTestModel> GetDiseaseTestNames(string term,string diseaseid, string Gender)
        {
            using IDbConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();

            // Replace "Users" with the actual name of your Users table
            var query = "SELECT DiseasesTestId, DiseasesTestName FROM T_DiseaseTest_Values WHERE IsActive = 1 AND Gender =@Gender AND DiseasesId= @DiseasesId AND DiseasesTestName LIKE @searchTerm";

            // You need to prepend and append '%' to the term to perform a partial match
            term = '%' + term + '%';

            // Execute the query and return a collection of DiseaseTestModel objects
            IEnumerable<DiseaseTestModel> diseaseModels = dbConnection.Query<DiseaseTestModel>(query, new { searchTerm = term , DiseasesId = diseaseid, Gender= Gender.Replace("\"", "") });

            return diseaseModels;
        }

        public IEnumerable<DiseaseTestModel> GetDiseaseTestsForEdit(string term, string diseaseid)
        {
            using IDbConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();

            // Replace "Users" with the actual name of your Users table
            var query = "SELECT DiseasesTestId, DiseasesTestName FROM T_DiseaseTest_Values WHERE DiseasesId= @DiseasesId AND DiseasesTestName LIKE @searchTerm";

            // You need to prepend and append '%' to the term to perform a partial match
            term = '%' + term + '%';

            // Execute the query and return a collection of DiseaseTestModel objects
            IEnumerable<DiseaseTestModel> diseaseModels = dbConnection.Query<DiseaseTestModel>(query, new { searchTerm = term, DiseasesId = diseaseid });

            return diseaseModels;
        }

        public int UpdateDiseaseDetails(string diseaseName, bool isDeleted, int diseasesId)
        {
            if (string.IsNullOrEmpty(diseaseName))
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    var query = "UPDATE T_Diseases_Master SET IsActive = @IsDeleted, ModifiedDate = @ModifiedDate WHERE DiseasesId = @DiseasesId";

                    dbConnection.Execute(query, new
                    {
                        DiseaseName = diseaseName,
                        IsDeleted = isDeleted == true ? 0 : 1,
                        ModifiedDate = DateTime.Now,
                        DiseasesId = diseasesId
                    });
                }
            }
            else
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    var query = "UPDATE T_Diseases_Master SET DiseaseName = @DiseaseName, IsActive = @IsDeleted, ModifiedDate = @ModifiedDate WHERE DiseasesId = @DiseasesId";

                    dbConnection.Execute(query, new
                    {
                        DiseaseName = diseaseName,
                        IsDeleted = isDeleted == true ? 0 : 1,
                        ModifiedDate = DateTime.Now,
                        DiseasesId = diseasesId
                    });
                }
            }
            return 1;
        }

        public List<DiseaseListModelEdit> GetDiseaseTestsEdit(string diseaseid)
        {
            List<DiseaseListModelEdit> diseaseTests = new List<DiseaseListModelEdit>();
            using IDbConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();

            // Replace "Users" with the actual name of your Users table
            var query = "select DiseasesTestId, DiseasesTestName,Ref_NormalStartValue,Ref_NormalEndValue,Ref_AboveNormalStartValue,Ref_AboveNormalEndValue,Ref_HighStartValue,Ref_HighEndValue,Ref_HigherStartValue,Ref_HigherEndValue,IsActive,Gender  from [dbo].[T_DiseaseTest_Values] where DiseasesId=@DiseasesTestId";

            // Execute the query and return a User object or null if not found
            diseaseTests = dbConnection.Query<DiseaseListModelEdit>(query, new { DiseasesTestId = diseaseid }).ToList();
            return diseaseTests;
        }

        public int updateMedicalTestValues(string diseaseNameId, List<DiseaseListModelEdit> values)
        {
            if (values != null && values.Any())
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    foreach (var value in values)
                    {
                        var query = @"UPDATE T_DiseaseTest_Values
                              SET 
                                DiseasesTestName = @DiseasesTestName,
                                Ref_NormalStartValue = @Ref_NormalStartValue,
                                Ref_NormalEndValue = @Ref_NormalEndValue,
                                Ref_AboveNormalStartValue = @Ref_AboveNormalStartValue,
                                Ref_AboveNormalEndValue = @Ref_AboveNormalEndValue,
                                Ref_HighStartValue = @Ref_HighStartValue,
                                Ref_HighEndValue = @Ref_HighEndValue,
                                Ref_HigherStartValue = @Ref_HigherStartValue,
                                Ref_HigherEndValue = @Ref_HigherEndValue,
                                ModifiedDate = @ModifiedDate,
                                IsActive = @IsActive,
                                Gender = @Gender
                              WHERE DiseasesId = @DiseasesId AND DiseasesTestId = @DiseasesTestId";

                        dbConnection.Execute(query, new
                        {
                            DiseasesTestName = value.DiseasesTestName,
                            Ref_NormalStartValue = value.Ref_NormalStartValue,
                            Ref_NormalEndValue = value.Ref_NormalEndValue,
                            Ref_AboveNormalStartValue = value.Ref_AboveNormalStartValue,
                            Ref_AboveNormalEndValue = value.Ref_AboveNormalEndValue,
                            Ref_HighStartValue = value.Ref_HighStartValue,
                            Ref_HighEndValue = value.Ref_HighEndValue,
                            Ref_HigherStartValue = value.Ref_HigherStartValue,
                            Ref_HigherEndValue = value.Ref_HigherEndValue,
                            ModifiedDate = DateTime.Now,
                            IsActive = value.IsActive,
                            Gender = value.Gender,
                            DiseasesId = diseaseNameId,
                            DiseasesTestId = value.DiseasesTestId
                        });
                    }
                }
            }


            return 1;
        }
    }
}
