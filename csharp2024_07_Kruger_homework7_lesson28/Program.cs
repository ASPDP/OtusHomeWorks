using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<Sums>();

/// <summary>
/// Внутрипроцессное взаимодействие // ДЗ  
/// </summary>
public class Sums
{
    private const string _fileName = "kruger_homework_random_generated_data.txt"; // файл создадим в папке пользователя ~100 мб
    private const int _maximumArraySize = 10_000_000;

    private static int[]? _dataForBenchmark;
    
    [Params(100_000, 1_000_000, _maximumArraySize)]
    public int ArraySize;

    // https://benchmarkdotnet.org/articles/features/setup-and-cleanup.html
    // Согласно доке метод вызывается каждый раз при смене полей/свойств отмеченных атрибом Params
    [GlobalSetup]
    public void SetupBeforeMeasurements()
    {
        string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string filePath = Path.Combine(userFolder, _fileName);
        
        if (File.Exists(filePath))
        {
            //---------------------------------------------------------
            //              DATA REGENERATION FROM FILE
            //---------------------------------------------------------
            string[] allLines = File.ReadAllLines(filePath);
            if (allLines.Length >= ArraySize)
                _dataForBenchmark = Array.ConvertAll(allLines, int.Parse)[..ArraySize];
            else throw new Exception(
                $"""
                 Кол-во строк не соотв. кол-ву элементов в массиве!
                 Строк должно быть больше или столько же, сколько элементов.
                 Удалите файл {filePath} и перезапустите приложение.
                 """);
        }
        else
        {
            //---------------------------------------------------------
            //              DATA GENERATION + SAVE TO FILE
            //---------------------------------------------------------
            var mostBiggestArray = new int[_maximumArraySize];
            for (int i = 0; i < _maximumArraySize; i++)
                mostBiggestArray[i] = new Random().Next(int.MinValue,int.MaxValue);
            var lines = Array.ConvertAll(mostBiggestArray, element => element.ToString());
            File.WriteAllLines(filePath, lines);
            //---------------------------------------------------------
            //           INIT ARRAY FOR TARGETED MEASUREMENT 
            //---------------------------------------------------------
            _dataForBenchmark = mostBiggestArray[..ArraySize];
        }
    }

    /// <summary>
    /// Обычное
    /// </summary>
    [Benchmark] 
    public void Common()
    {
        long result = 0; // даже если все int будут равны 2147483647, то их сумма будет меньше макс. знач. long
        foreach (var element in _dataForBenchmark)
            result += element;

        Log(result, "Common");
    }

    /// <summary>
    /// Параллельное (для реализации использован Thread в List)
    /// </summary>
    /// <param name="threads">0 mean 1 thread for 1 core</param>
    [Benchmark]
    [Arguments(2)]
    [Arguments(7)] // самый быстрый для моего компа при 10 млн элементов
    [Arguments(0)]
    public void Thread(int threads)
    {
        if (threads == 0)
            threads = Environment.ProcessorCount;
        var partSize = ArraySize / threads; // размер куска массива для передачи в тред
        var threadList = new List<Thread>();
        long result = 0;
        var lockobject = new object();

        for (int i = 0; i < threads; i++)
        {
            var iteration = i; // исключаем захват переменной тредом
            var thread = new Thread(() =>
                {
                    var from = partSize * iteration;
                    var to = (threads - iteration == 1) ? ArraySize : partSize * (iteration+1);
                    //       ^последняя итерация?         ^до конца   ^до конца куска массива

                    long threadResult = 0;
                    for (long j = from; j < to; j++)
                        threadResult += _dataForBenchmark[j];
                    
                    lock (lockobject)
                        result += threadResult;
                });
            threadList.Add(thread);
            thread.Start();
        }

        foreach (var thread in threadList)
            thread.Join();

        Log(result, "Thread");
    }
    
    /// <summary>
    /// Параллельное с помощью LINQ (без партишининга)
    /// </summary>
    [Benchmark]
    public void Plinq()
    {
        // для решения проблемы с переполнением
        // и ускорения, вместо Plinq
        // можно глянуть в сторону https://github.com/DragonSpit/HPCsharp
        var result = _dataForBenchmark.AsParallel().Sum(e => (long)e);

        Log(result, "Plinq");
    }
    
    private void Log(long result, string methName)
    {
        // раскомментировать если хочется посмотреть правильно ли суммируется 
        /*
        Console.WriteLine(
            $"""
             ┌ real size:     {_dataForBenchmark.Length,20}
             | expected size: {ArraySize,20}
             | meth name:     {methName,20}
             | frst element:  {_dataForBenchmark.First(),20}
             | last element:  {_dataForBenchmark.Last(),20}
             └ summ: {result}
             """);
         */
    }
}