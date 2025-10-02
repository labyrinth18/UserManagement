// Шлях: UserManagement.DAL/IRoleRepository.cs
using System.Collections.Generic;
//using UserManagement.DTO;

public interface IRoleRepository
{
    List<RoleDto> GetAllRoles();
}