/// <summary>
/// Interface Segregation Principle: интерфейс содержит только нужные для настроек методы
/// Dependency Inversion Principle: зависимость от абстракции, а не реализации
/// </summary>
public interface IGameSettings
{
    int MinNumber { get; }
    int MaxNumber { get; }
    int MaxAttempts { get; }
}