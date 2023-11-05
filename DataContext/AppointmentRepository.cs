using ClinicalPharmaSystem.Models;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace ClinicalPharmaSystem.DataContext
{
    public class AppointmentRepository
    {
        private readonly string connectionString;

        public AppointmentRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void InsertUser(RegisterViewModel user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Replace with your actual SQL query and parameters
                string sql = "INSERT INTO Users (UserName, Email, PhoneNumber, Password) VALUES (@UserName, @Email, @PhoneNumber, @Password)";

                connection.Execute(sql, new
                {
                    user.UserName,
                    user.Email,
                    user.PhoneNumber,
                    user.Password // Remember to hash the password before storing it in the database for security.
                });
            }
        }

        // Add a method to retrieve a user by username
        public LoginViewModel GetUserByUsername(string username)
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            using IDbConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();

            // Replace "Users" with the actual name of your Users table
            var query = "SELECT * FROM Users WHERE UserName = @Username";

            // Execute the query and return a User object or null if not found
            loginViewModel = dbConnection.QueryFirstOrDefault<LoginViewModel>(query, new { Username = username });
            return loginViewModel;
        }

        // Add a method to retrieve a user by username
        public bool AvailabilityCheckLogic(string appointmentDate, string appointmentTime)
        {
            bool result = false;
            using IDbConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();

            // Replace "Users" with the actual name of your Users table
            var query = "SELECT * FROM Appointments WHERE CAST(AppointmentDate AS DATE) = @appointmentDate and AppointmentTime=@appointmentTime";

            // Execute the query and return a User object or null if not found
            var status = dbConnection.QueryFirstOrDefault<Appointment>(query, new { appointmentDate = appointmentDate, appointmentTime = appointmentTime });
            
            if(status == null)
            {
                result = true;
            }
            return result;
        }

        public int SaveAppointment(Appointment appointment)
        {
            var id = 0;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Appointments (AppointmentDate, AppointmentTime, Notes, MobileNumber) " +
             "OUTPUT Inserted.AppointmentID " + // Add this line to capture the ID
             "VALUES (@AppointmentDate, @AppointmentTime, @Notes, @MobileNumber)";

                id = connection.ExecuteScalar<int>(sql, new
                {
                    AppointmentDate = appointment.AppointmentDate,
                    AppointmentTime = appointment.AppointmentTime,
                    Notes = appointment.Notes,
                    MobileNumber = appointment.MobileNumber
                });

                // If the insert was successful, 'id' will contain the newly inserted ID
                return id;

            }
        }

    }
}
