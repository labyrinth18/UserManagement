using Microsoft.Extensions.Configuration;
using System;
using System.IO;
//using UserManagement.DAL;

class Program
{
    static void Main(string[] args)
    {
        // --- 1. Налаштування конфігурації для читання appsettings.json ---
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        IConfigurationRoot configuration = builder.Build();
        string connectionString = configuration.GetConnectionString("DefaultConnection");

        // --- 2. Створення всіх наших "робітників" (репозиторіїв) ---
        IRoleRepository roleRepository = new RoleRepository(connectionString);
        IAdminActionLogRepository logRepository = new AdminActionLogRepository(connectionString);
        IUserRepository userRepository = new UserRepository(connectionString, logRepository);

        // --- 3. Створення об'єктів для керування інтерфейсом ---
        var menu = new MenuHandler();
        var actions = new UserActions(userRepository, roleRepository, logRepository);

        // --- 4. Головний цикл програми ---
        while (true)
        {
            menu.ShowMainMenu();
            string choice = menu.GetUserChoice();

            switch (choice)
            {
                case "1": actions.ShowAllUsers(); break;
                case "2": actions.AddNewUser(); break;
                case "3": actions.AssignRole(); break;
                case "4": actions.UpdateUserStatus(); break;
                case "5": actions.ShowActionHistory(); break;
                case "exit": return;
                default: Console.WriteLine("Неправильний вибір, спробуйте ще раз."); break;
            }
        }
    }
}