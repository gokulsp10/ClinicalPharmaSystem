using System.Data;
using System.Data.SqlClient;
using ClinicalPharmaSystem.Models;
using Dapper;

public class UserRepository
{
    private readonly string connectionString;

    public UserRepository(string connectionString)
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
}
