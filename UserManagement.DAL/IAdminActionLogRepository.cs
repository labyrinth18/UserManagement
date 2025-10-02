// Шлях: UserManagement.DAL/IAdminActionLogRepository.cs
using System.Collections.Generic;
//using UserManagement.DTO;

public interface IAdminActionLogRepository
{
    void LogAction(AdminActionLogDto logEntry);
    List<AdminActionLogDto> GetAllLogs();
}