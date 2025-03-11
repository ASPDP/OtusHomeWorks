/// <summary>
/// Interface Segregation Principle: интерфейс для взаимодействия с пользователем 
/// </summary>
public interface IUserInterface
{
    void DisplayMessage(string message);
    int GetUserInput(string prompt);
}