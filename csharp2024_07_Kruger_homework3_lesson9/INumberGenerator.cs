/// <summary>
///Interface Segregation Principle: узкий специализированный интерфейс
///Dependency Inversion Principle: высокоуровневые компоненты зависят от абстракции
/// </summary>
public interface INumberGenerator
{
    int GenerateNumber(int min, int max);
}