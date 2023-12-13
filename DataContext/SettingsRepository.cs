using ClinicalPharmaSystem.Models;
using ClinicalPharmaSystem.Models.Settings;
using Dapper;
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

    public List<User> GetPendingActivationUsers()
    {
        List<User> users = new List<User>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("select id,UserName,PhoneNumber,Email from Users where isActivated = 0 and RoleId is null", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User user = new User
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            UserName = reader["UserName"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            Email = reader["Email"].ToString()
                        };
                        users.Add(user);
                    }
                }
            }
        }

        return users;
    }


    public List<Role> GetRolesFromDatabase()
    {
        List<Role> roles;

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var query = @"
                select * from Roles where IsActive=1;";

            roles = connection.Query<Role>(query).ToList();
        }

        return roles;
    }

    public bool UpdateUser(User user)
    {
        bool updatesStatus =false;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            if(user.action == "1")
            {
                using (SqlCommand command = new SqlCommand("UPDATE Users SET RoleId = @RoleId, isActivated = @isActivated WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@RoleId", user.RoleId);
                    command.Parameters.AddWithValue("@isActivated", user.action);
                    command.Parameters.AddWithValue("@Id", user.Id);

                    command.ExecuteNonQuery();
                    updatesStatus = true;
                }
            }
            else if(user.action == "2")
            {
                using (SqlCommand command = new SqlCommand("UPDATE Users SET RoleId = @RoleId, IsDeleted = @IsDeleted WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@RoleId", user.RoleId);
                    command.Parameters.AddWithValue("@IsDeleted", user.action);
                    command.Parameters.AddWithValue("@Id", user.Id);

                    command.ExecuteNonQuery();
                    updatesStatus = true;
                }
            }
            else if (user.action == "3")
            {
                using (SqlCommand command = new SqlCommand("UPDATE Users SET RoleId = @RoleId, IsDeleted = @IsDeleted,isActivated=@isActivated WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@RoleId", user.RoleId);
                    command.Parameters.AddWithValue("@IsDeleted", user.IsDeleted);
                    command.Parameters.AddWithValue("@isActivated", user.IsActivated);
                    command.Parameters.AddWithValue("@Id", user.Id);

                    command.ExecuteNonQuery();
                    updatesStatus = true;
                }
            }

        }
        return updatesStatus;
    }

    public List<User> GetUsers()
    {
        List<User> users = new List<User>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("select id,UserName,PhoneNumber,Email,isnull(RoleId,0)RoleId,isActivated,isDeleted from Users", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User user = new User
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            UserName = reader["UserName"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            Email = reader["Email"].ToString(),
                            RoleId = Convert.ToInt32(reader["RoleId"]),
                            IsActivated = Convert.ToBoolean(reader["IsActivated"]),
                            IsDeleted = Convert.ToBoolean(reader["IsDeleted"])
                        };
                        users.Add(user);
                    }
                }
            }
        }

        return users;
    }

    public int doctordetailsUpload(DoctorVerification role)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("INSERT INTO Doctors (DoctorName, DoctorId, DoctorStudy, DoctorSpeciality, DoctorContactAddress, ContactMobile, ContactNumber, RegNo) VALUES (@DoctorName, @DoctorId, @DoctorStudy, @DoctorSpeciality, @DoctorContactAddress, @ContactMobile, @ContactNumber, @RegNo)", connection))
            {
                command.Parameters.AddWithValue("@DoctorName", role.DoctorName);
                command.Parameters.AddWithValue("@DoctorId", role.DoctorId);
                command.Parameters.AddWithValue("@DoctorStudy", role.DoctorStudy);
                command.Parameters.AddWithValue("@DoctorSpeciality", role.DoctorSpeciality);
                command.Parameters.AddWithValue("@DoctorContactAddress", role.DoctorContactAddress);
                command.Parameters.AddWithValue("@ContactMobile", role.ContactMobile);
                command.Parameters.AddWithValue("@ContactNumber", role.ContactNumber);
                command.Parameters.AddWithValue("@RegNo", role.RegNo);

                command.ExecuteNonQuery();
            }
        }
        return 1;
    }

}
