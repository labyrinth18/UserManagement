// Шлях: UserManagement.DAL/UserRepository.cs
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
//using UserManagement.DTO;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;
    private readonly IAdminActionLogRepository _logRepository;

    // Конструктор тепер приймає не тільки рядок підключення,
    // а й "робітника" для логів. Це називається Dependency Injection.
    public UserRepository(string connectionString, IAdminActionLogRepository logRepository)
    {
        _connectionString = connectionString;
        _logRepository = logRepository;
    }

    // --- МЕТОДИ ДЛЯ ЧИТАННЯ ДАНИХ (Read) ---

    public List<UserDto> GetAllUsersWithRoles()
    {
        // Цей код ми вже бачили, він дістає юзерів з їхніми ролями
        var userList = new List<UserDto>();
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string sql = @"SELECT u.UserID, u.Login, u.FirstName, u.LastName, u.IsActive, r.RoleID, r.RoleName
                           FROM Users u JOIN Roles r ON u.RoleID = r.RoleID";
            using (var command = new SqlCommand(sql, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    userList.Add(new UserDto { /* ... заповнення полів ... */ });
                }
            }
        }
        return userList;
    }

    public UserDto? GetUserById(int userId)
    {
        // Реалізація пошуку одного користувача
        // ... (ми додамо її пізніше, якщо знадобиться) ...
        return null;
    }

    // --- МЕТОДИ ДЛЯ ЗМІНИ ДАНИХ (Create, Update, Delete) ---

    public void AddUser(UserDto user, int adminId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string sql = "INSERT INTO Users (Login, FirstName, LastName, IsActive, RoleID) OUTPUT INSERTED.UserID VALUES (@Login, @FirstName, @LastName, @IsActive, @RoleID)";
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Login", user.Login);
                // ... додати інші параметри ...

                // Виконуємо запит і отримуємо ID нового користувача
                var newUserId = (int)command.ExecuteScalar();

                // ✨ Логуємо дію!
                _logRepository.LogAction(new AdminActionLogDto
                {
                    AdminUserID = adminId,
                    TargetUserID = newUserId,
                    ActionDescription = $"Created new user with login '{user.Login}'."
                });
            }
        }
    }

    public void UpdateUser(UserDto user, int adminId)
    {
        // Код для оновлення FirstName, LastName...
    }

    public void UpdateUserStatus(int targetUserId, bool isActive, int adminId)
    {
        // Код для оновлення статусу IsActive...

        // ✨ Логуємо дію!
        string action = isActive ? "Unblocked" : "Blocked";
        _logRepository.LogAction(new AdminActionLogDto
        {
            AdminUserID = adminId,
            TargetUserID = targetUserId,
            ActionDescription = $"{action} user account."
        });
    }

    public void AssignRoleToUser(int targetUserId, int newRoleId, int adminId)
    {
        // Код для оновлення RoleID...

        // ✨ Логуємо дію!
        _logRepository.LogAction(new AdminActionLogDto
        {
            AdminUserID = adminId,
            TargetUserID = targetUserId,
            ActionDescription = $"Assigned new role with ID: {newRoleId}"
        });
    }

    public void DeleteUser(int userId, int adminId)
    {
        // Код для видалення користувача...

        // ✨ Логуємо дію!
        _logRepository.LogAction(new AdminActionLogDto
        {
            AdminUserID = adminId,
            TargetUserID = userId,
            ActionDescription = $"Deleted user with ID: {userId}"
        });
    }
}