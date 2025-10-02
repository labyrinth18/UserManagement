// Шлях: UserManagement.ConsoleApp/UserActions.cs
using System;
using System.Linq;
//using UserManagement.DAL;
//using UserManagement.DTO;

public class UserActions
{
    // Цей клас "знає" про всіх наших "робітників"
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IAdminActionLogRepository _logRepository;

    // Імітуємо, що ми увійшли як адміністратор з ID = 1
    private readonly int _currentAdminId = 1;

    public UserActions(IUserRepository userRepository, IRoleRepository roleRepository, IAdminActionLogRepository logRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _logRepository = logRepository;
    }

    public void ShowAllUsers()
    {
        Console.WriteLine("\n--- Список користувачів ---");
        var users = _userRepository.GetAllUsersWithRoles();
        foreach (var user in users)
        {
            string status = user.IsActive ? "Активний" : "Заблокований";
            Console.WriteLine($"ID: {user.UserID}, Логін: {user.Login}, Ім'я: {user.FirstName} {user.LastName}, Роль: {user.RoleName}, Статус: {status}");
        }
    }

    public void AddNewUser()
    {
        Console.WriteLine("\n--- Додавання нового користувача ---");
        var user = new UserDto();
        Console.Write("Логін: ");
        user.Login = Console.ReadLine();
        Console.Write("Ім'я: ");
        user.FirstName = Console.ReadLine();
        Console.Write("Прізвище: ");
        user.LastName = Console.ReadLine();

        // Показуємо доступні ролі
        ShowAllRoles();
        Console.Write("ID Ролі: ");
        user.RoleID = int.Parse(Console.ReadLine());
        user.IsActive = true;

        _userRepository.AddUser(user, _currentAdminId);
        Console.WriteLine("Нового користувача успішно додано!");
    }

    public void AssignRole()
    {
        Console.WriteLine("\n--- Призначення нової ролі ---");
        Console.Write("Введіть ID користувача: ");
        int userId = int.Parse(Console.ReadLine());

        // Показуємо доступні ролі, щоб було з чого вибирати
        ShowAllRoles();
        Console.Write("Введіть ID нової ролі: ");
        int roleId = int.Parse(Console.ReadLine());

        _userRepository.AssignRoleToUser(userId, roleId, _currentAdminId);
        Console.WriteLine("Роль успішно призначено!");
    }

    public void UpdateUserStatus()
    {
        Console.WriteLine("\n--- Зміна статусу користувача ---");
        Console.Write("Введіть ID користувача: ");
        int userId = int.Parse(Console.ReadLine());
        Console.Write("Введіть новий статус (true для активного, false для заблокованого): ");
        bool isActive = bool.Parse(Console.ReadLine());

        _userRepository.UpdateUserStatus(userId, isActive, _currentAdminId);
        Console.WriteLine("Статус успішно оновлено!");
    }

    public void ShowActionHistory()
    {
        Console.WriteLine("\n--- Історія дій адміністраторів ---");
        var logs = _logRepository.GetAllLogs();
        if (!logs.Any())
        {
            Console.WriteLine("Історія порожня.");
            return;
        }
        foreach (var log in logs)
        {
            Console.WriteLine($"[{log.ActionDate:yyyy-MM-dd HH:mm:ss}] Адмін '{log.AdminUserLogin}' -> Користувач '{log.TargetUserLogin}': {log.ActionDescription}");
        }
    }

    // Допоміжний приватний метод, щоб не дублювати код
    private void ShowAllRoles()
    {
        Console.WriteLine("Доступні ролі:");
        var roles = _roleRepository.GetAllRoles();
        foreach (var role in roles)
        {
            Console.WriteLine($"  ID: {role.RoleID}, Назва: {role.RoleName}");
        }
    }
}