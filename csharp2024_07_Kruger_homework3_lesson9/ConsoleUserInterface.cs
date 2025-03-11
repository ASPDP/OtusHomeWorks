/// <summary>
/// Single Responsibility Principle: отвечает только за взаимодействие через консоль
/// Liskov Substitution Principle: корректно реализует все методы интерфейса
/// </summary>
public class ConsoleUserInterface : IUserInterface
{
    public void DisplayMessage(string message)
    {
        Console.WriteLine(message);
    }

    public int GetUserInput(string prompt)
    {
        int input;
        Console.Write(prompt);
        while (!int.TryParse(Console.ReadLine(), out input))
        {
            Console.Write("Некорректный ввод. Пожалуйста, введите число: ");
        }
        return input;
    }
}