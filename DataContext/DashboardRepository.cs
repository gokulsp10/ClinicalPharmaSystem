using ClinicalPharmaSystem.Models;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace ClinicalPharmaSystem.DataContext
{
    public class DashboardRepository
    {
        private readonly string connectionString;

        public DashboardRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public object GetPatientRecordsByUserID(string UserId)
        {            
            using IDbConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();

            // Replace "Users" with the actual name of your Users table
            var query = "SELECT * FROM HealthData WHERE UserId = @UserId";

            // Execute the query and return a User object or null if not found
           var loginViewModel = dbConnection.QueryFirstOrDefault<object>(query, new { UserId = UserId });
            return loginViewModel;
        }
    }
}
