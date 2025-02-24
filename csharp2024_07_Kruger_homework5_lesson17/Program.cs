using System.Collections;

namespace csharp2024_07_Kruger_homework5_lesson17;

class Program
{
    
    static void Main(string[] args)
    {
        Console.WriteLine("Вывод первой части задания:");
        GetMaxWith(StringMeasurer);
        GetMaxWith(StringMeasurerNull);

        Console.WriteLine("Вывод второй части задания:");
        var fileExtension = "SFEFHW"; //расширение файлов без точкис которыми будут производиться манипуляции
        var fileCreator = new FileCreator(files:10, fileExtension);
        var fileSearcher = new FileSearcher();
        var fileSubscriber = new EgoisticSubcriber(maximumFileForFound: 5);
        
        fileSearcher.FileFound += fileSubscriber.IamGladFileFound;
        fileSearcher.FileFound += fileSubscriber.MeeToo;
        
        fileCreator.Create();
        
        fileSearcher.SearchRecursively(fileExtension);
        
        Console.WriteLine(
            """
            Работа программ закончена нажмите любую клавишу 
            для удаления файлов сделанных домашним заданием
            """);
        Console.ReadKey(true);
        
        foreach (var info in fileCreator.Clean())
            Console.WriteLine(info);
    }
    
    /// <summary>
    /// Лист Листов из стрингов который мы будем мучать
    /// </summary>
    public static List<List<string>> ListForExperiments =
    [
    /* 0 */   ["xxxxxx","sdf", "sss", "ssy", "sss"],
    /* 1 */   ["xxxxxx",null,null],
    /* 2 */   [null,null,null,"xxxxxx"],
    /* 3 */   [null,null,null,null,""],
    /* 4 */   [null,null,null,null,null],
    /* 5 */   [],
    /* 6 */   null,
    /* 7 */   ["xxxxxx", "sdf", null, "ss", "s"],
    /* 8 */   ["xxxxxx", null, "ss", "s"],
    /* 9 */   ["sd", null, "xxxxxx", "s"],
    ];
    
    /// <summary>
    /// Обыкновенный статический метод, возвращающий длину строки.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static float StringMeasurer(string input) =>
        input.Length;

    /// <summary>
    /// Обыкновенный статический метод, возвращающий длину строки. но Null трактуем как -1
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static float StringMeasurerNull(string? input)
    {
        if (input is not null)
            return input.Length;
        else
            return -1;
    }


    private static void GetMaxWith(Func<string, float> withThat)
    {
        Console.WriteLine(
            $"""

             -----------------------------------------------------------

             ┌──────────────────────────────────────┐
             """);
        for (var index = 0; index < ListForExperiments.Count; index++)
        {
            try
            {
                Console.WriteLine(
                    $"""
                     │ ,___________________________________
                     │ │ #{index} элемент массива будет обработан...
                     """);
                
                // Делегат можно передать кучей способов:
                // ----------------
                //  .GetMax(s => s.Length);
                // ----------------
                //  .GetMax(delegate(string s)
                //  {
                //      captureMeBaby++;
                //      return s.Length;
                //  });
                // ----------------
                // var funnyFunc = new Func<string, float>(e => e.Length);
                //  .GetMax(funnyFunc);
                // ----------------
                
                Console.WriteLine(
                    $"""
                     │ └ эл.мас. обработан, выдал: {ListForExperiments[index].GetMax(withThat)}.
                     """);
                
            }
            catch (Exception e)
            {
                Console.Write($"│ └ ");
                var savedColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"x_x - {e.GetType()}");
                Console.ForegroundColor = savedColor;
            }
        } //++
        Console.WriteLine("└──────────────────────────────────────┘");
    }

}