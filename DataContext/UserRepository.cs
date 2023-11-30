using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using ClinicalPharmaSystem.DataContext;
using ClinicalPharmaSystem.Models;
using Dapper;
using Microsoft.AspNetCore.Http;

public class UserRepository
{
    private readonly string connectionString;
    private readonly IHttpContextAccessor _contextAccessor;

    public UserRepository(string connectionString, IHttpContextAccessor contextAccessor)
    {
        this.connectionString = connectionString;
        _contextAccessor = contextAccessor;
    }

    public int InsertUser(RegisterViewModel user)
    {
        int Statusid = 0;
        List<object> list = ValidateClinicalId(user.ClinicalId);

        if (list.Count > 0)
        {
            Statusid = 1;
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
                Statusid = 2;
            }
        }
        return Statusid;
    }

    // Add a method to retrieve a user by username
    public LoginViewModel GetUserByUsername(string username)
    {
        LoginViewModel loginViewModel;
        using IDbConnection dbConnection = new SqlConnection(connectionString);
        dbConnection.Open();

        // Replace "Users" with the actual name of your Users table
        var query = "select UserName,Email,PhoneNumber,Password,RoleName,Users.RoleId RoleId from Users inner join Roles on Users.RoleId = Roles.RoleId where isDeleted = 0 and isActivated = 1 and IsActive=1 and UserName = @Username";

        // Execute the query and return a User object or null if not found
        loginViewModel = dbConnection.QueryFirstOrDefault<LoginViewModel>(query, new { Username = username });

        if (loginViewModel !=null)
        {
            // Fetch menu items from the database
            List<MenuViewModel> menuItems = InvokemenuItems(loginViewModel.RoleName);

            // Store menu items in session
            var menuItemsJson = JsonSerializer.Serialize(menuItems);
            _contextAccessor.HttpContext.Session.SetString("MenuItems", menuItemsJson);
            _contextAccessor.HttpContext.Session.SetString("RoleName", loginViewModel.RoleName);
            _contextAccessor.HttpContext.Session.SetString("RoleId", Convert.ToString(loginViewModel.RoleId));
            _contextAccessor.HttpContext.Session.SetString("UserName", loginViewModel.UserName);
        }
        return loginViewModel;
    }

    public List<MenuViewModel> InvokemenuItems(string RoleName)
    {
        List<MenuViewModel> menuItems;

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var query = @"
            SELECT
                PM.Text AS PrimaryMenuItemText,
                PM.IconClass AS PrimaryMenuItemIconClass,
                PM.Url AS PrimaryMenuItemUrl,
                SM.Text AS SecondaryMenuItemText,
                SM.Url AS SecondaryMenuItemUrl
            FROM
                PrimaryMenuItems AS PM
            LEFT JOIN
                SecondaryMenuItems AS SM ON PM.Id = SM.PrimaryMenuItemId
            WHERE
                PM.IsActive = 1 AND
                (SM.IsActive = 1 OR SM.IsActive IS NULL) AND
                PM.roleaccess LIKE @RoleName
            ORDER BY
                PM.Ordering, SM.Ordering;
        ";

            // Use Dapper's Query method with parameters
            menuItems = connection.Query<MenuViewModel>(query, new { RoleName = "%" + RoleName + "%" }).ToList();
        }

        return menuItems;
    }

    public List<object> ValidateClinicalId(int ClinicalId)
    {
        List<object> menuItems;

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var query = @"
               select * from ClinicalInfo
            WHERE
               ClinicalId = @ClinicalId

            ";

            menuItems = connection.Query<object>(query, new { ClinicalId = ClinicalId }).ToList();
        }

        return menuItems;
    }
}
