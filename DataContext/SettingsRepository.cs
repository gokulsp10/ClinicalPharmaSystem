using ClinicalPharmaSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

public class SettingsRepository
{
    private readonly string connectionString;

    public SettingsRepository(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public List<Role> GetAllRoles()
    {
        List<Role> roles = new List<Role>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("SELECT * FROM Roles", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Role role = new Role
                            {
                                RoleId = Convert.ToInt32(reader["RoleId"]),
                                RoleName = reader["RoleName"].ToString(),
                                RoleDescription = reader["RoleDescription"].ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                        };
                        roles.Add(role);
                    }
                }
            }
        }

        return roles;
    }

    public void AddRole(RoleForm role)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("INSERT INTO Roles (RoleName, RoleDescription, IsActive) VALUES (@RoleName, @RoleDescription, @IsActive)", connection))
            {
                command.Parameters.AddWithValue("@RoleName", role.RoleName);
                command.Parameters.AddWithValue("@RoleDescription", role.RoleDescription);
                command.Parameters.AddWithValue("@IsActive", role.IsActive);

                command.ExecuteNonQuery();
            }
        }
    }

    public void UpdateRole(Role role)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("UPDATE Roles SET RoleName = @RoleName, RoleDescription = @RoleDescription, IsActive = @IsActive WHERE RoleId = @RoleId", connection))
            {
                command.Parameters.AddWithValue("@RoleId", role.RoleId);
                command.Parameters.AddWithValue("@RoleName", role.RoleName);
                command.Parameters.AddWithValue("@RoleDescription", role.RoleDescription);
                command.Parameters.AddWithValue("@IsActive", role.IsActive);

                command.ExecuteNonQuery();
            }
        }
    }

    public void DeleteRole(int roleId)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("UPDATE Roles SET IsActive=1 WHERE RoleId = @RoleId", connection))
            {
                command.Parameters.AddWithValue("@RoleId", roleId);

                command.ExecuteNonQuery();
            }
        }
    }
}
