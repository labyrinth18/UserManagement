// Шлях: UserManagement.DAL/RoleRepository.cs
using System.Collections.Generic;
//using System.Data.SqlClient;
//using UserManagement.DTO;
using Microsoft.Data.SqlClient;
public class RoleRepository : IRoleRepository // Кажемо, що цей клас виконує контракт IRoleRepository
{
    private readonly string _connectionString;

    // Конструктор, який буде отримувати рядок підключення ззовні
    public RoleRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<RoleDto> GetAllRoles()
    {
        var roleList = new List<RoleDto>();
        // 'using' гарантує, що з'єднання з базою буде закрито, навіть якщо виникне помилка
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string sql = "SELECT RoleID, RoleName FROM Roles ORDER BY RoleName";
            using (var command = new SqlCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var role = new RoleDto
                        {
                            RoleID = (int)reader["RoleID"],
                            RoleName = (string)reader["RoleName"]
                        };
                        roleList.Add(role);
                    }
                }
            }
        }
        return roleList;
    }
}