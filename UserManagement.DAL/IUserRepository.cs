// Шлях: UserManagement.DAL/IUserRepository.cs
using System.Collections.Generic;
//using UserManagement.DTO;

public interface IUserRepository
{
    List<UserDto> GetAllUsersWithRoles();
    UserDto? GetUserById(int userId);
    void AddUser(UserDto user, int adminId);
    void UpdateUser(UserDto user, int adminId);
    void UpdateUserStatus(int targetUserId, bool isActive, int adminId);
    void AssignRoleToUser(int targetUserId, int newRoleId, int adminId);
    void DeleteUser(int userId, int adminId);
}