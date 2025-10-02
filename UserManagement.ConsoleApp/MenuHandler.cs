// Шлях: UserManagement.ConsoleApp/MenuHandler.cs
using System;

public class MenuHandler
{
    public void ShowMainMenu()
    {
        Console.WriteLine("\n--- Система керування користувачами ---");
        Console.WriteLine("1. Показати всіх користувачів");
        Console.WriteLine("2. Додати нового користувача");
        Console.WriteLine("3. Змінити роль користувача");
        Console.WriteLine("4. Заблокувати/Розблокувати користувача");
        Console.WriteLine("5. Переглянути історію дій");
        Console.WriteLine("Для виходу введіть 'exit'");
    }

    public string GetUserChoice()
    {
        Console.Write("--> Ваш вибір: ");
        return Console.ReadLine() ?? "";
    }
}