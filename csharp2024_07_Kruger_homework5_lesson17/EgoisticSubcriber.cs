namespace csharp2024_07_Kruger_homework5_lesson17;

/// <summary>
/// Подписчик досчитывает до указанного при создании максимума и вызывает у sender'а остановку
/// </summary>
/// <param name="maximumFileForFound">максимальное число "реакций" на событие</param>
public class EgoisticSubcriber(int maximumFileForFound)
{
    private int _maximumFilesForFound  = maximumFileForFound;
    private int _currentRiseCount;

    public void IamGladFileFound(object sender, FileFoundArgs args)
    {
        _currentRiseCount++;
        if (_currentRiseCount >= _maximumFilesForFound)
        {
            var stopPreparation = sender as FileSearcher;
            stopPreparation?.StopNextEvents();
            if (stopPreparation != null)
            {
                var savedColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    $"""
                     Это последний файл который мне нужен. 
                     {_currentRiseCount}/{_maximumFilesForFound}.
                     Теперь искатель файлов может больше никому 
                     не сообщать о другних файлах которые он нашел. 
                     Вот так вот!
                     """);
                Console.ForegroundColor = savedColor;
            }
        }

        Console.WriteLine($"{_currentRiseCount} файл. Мне нашли файл! {args.Name}");
    }
    
    public void MeeToo(object sender, FileFoundArgs args)
    {
        Console.WriteLine($"{_currentRiseCount} файл. мне тоже, мне тоже нашли файл! {args.Name}");
    }

}