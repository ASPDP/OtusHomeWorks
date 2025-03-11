/// <summary>
///Single Responsibility Principle: класс отвечает только за генерацию чисел 
/// </summary>
public class RandomNumberGenerator : INumberGenerator
{
    private readonly Random _random = new Random();

    public int GenerateNumber(int min, int max)
    {
        return _random.Next(min, max + 1);
    }
}