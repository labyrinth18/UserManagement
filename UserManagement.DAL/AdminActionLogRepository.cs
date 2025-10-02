// Шлях: UserManagement.DAL/AdminActionLogRepository.cs
using Microsoft.Data.SqlClient; // Використовуємо нову бібліотеку
using System;
using System.Collections.Generic;
//using UserManagement.DTO;

public class AdminActionLogRepository : IAdminActionLogRepository
{
    private readonly string _connectionString;

    public AdminActionLogRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void LogAction(AdminActionLogDto logEntry)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string sql = "INSERT INTO AdminActionLog (AdminUserID, TargetUserID, ActionDescription) VALUES (@AdminUserID, @TargetUserID, @ActionDescription)";
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@AdminUserID", logEntry.AdminUserID);
                command.Parameters.AddWithValue("@TargetUserID", logEntry.TargetUserID);
                command.Parameters.AddWithValue("@ActionDescription", logEntry.ActionDescription);
                command.ExecuteNonQuery();
            }
        }
    }

    public List<AdminActionLogDto> GetAllLogs()
    {
        var logList = new List<AdminActionLogDto>();
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            // SQL-запит, що з'єднує таблицю логів з таблицею користувачів двічі,
            // щоб отримати логіни і адміна, і цільового користувача.
            string sql = @"SELECT
                             l.LogID, l.ActionDescription, l.ActionDate,
                             l.AdminUserID, admin.Login as AdminLogin,
                             l.TargetUserID, target.Login as TargetLogin
                           FROM AdminActionLog l
                           JOIN Users admin ON l.AdminUserID = admin.UserID
                           JOIN Users target ON l.TargetUserID = target.UserID
                           ORDER BY l.ActionDate DESC";

            using (var command = new SqlCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var logEntry = new AdminActionLogDto
                        {
                            LogID = (int)reader["LogID"],
                            ActionDescription = (string)reader["ActionDescription"],
                            ActionDate = (DateTime)reader["ActionDate"],
                            AdminUserID = (int)reader["AdminUserID"],
                            AdminUserLogin = (string)reader["AdminLogin"],
                            TargetUserID = (int)reader["TargetUserID"],
                            TargetUserLogin = (string)reader["TargetLogin"]
                        };
                        logList.Add(logEntry);
                    }
                }
            }
        }
        return logList;
    }
}