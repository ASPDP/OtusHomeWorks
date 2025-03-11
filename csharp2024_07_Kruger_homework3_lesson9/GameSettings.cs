/// <summary>
/// Single Responsibility Principle: класс отвечает только за хранение настроек
/// Open/Closed Principle: можно расширить, создав производный класс с дополнительными настройками
/// </summary>
public class GameSettings : IGameSettings
{
    public int MinNumber { get; }
    public int MaxNumber { get; }
    public int MaxAttempts { get; }

    public GameSettings(int minNumber, int maxNumber, int maxAttempts)
    {
        MinNumber = minNumber;
        MaxNumber = maxNumber;
        MaxAttempts = maxAttempts;
    }
}