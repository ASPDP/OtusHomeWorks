using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<Sums>();

/// <summary>
/// Внутрипроцессное взаимодействие // ДЗ  
/// </summary>
public class Sums
{
    private const int maximum = 10_000_000;
    private static int[] _dataForBenchmark;
    private static string _fileName = "kruger_homework_random_generated_data.txt"; // файл создадим в папке пользователя ~100 мб
    
    [Params(100_000,1_000_000,maximum)]
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
            var mostBiggestArray = new int[maximum];
            for (int i = 0; i < maximum; i++)
                mostBiggestArray[i] = new Random().Next();
            var lines = Array.ConvertAll(mostBiggestArray, element => element.ToString());
            File.WriteAllLines(filePath, lines);
            //---------------------------------------------------------
            //           INIT ARRAY FOR TARGETED MEASUREMENT 
            //---------------------------------------------------------
            _dataForBenchmark = mostBiggestArray[..(ArraySize)];
        } 
    }


    [Benchmark] 
    public void CommonFor()
    {
        long result = 0; // даже если все int будут равны 2147483647, то их сумма будет меньше макс. знач. long
        foreach (var element in _dataForBenchmark)
            result += element;
        
        //Console.WriteLine($"real size: {_dataForBenchmark.Length,10} - expected size: {ArraySize,10} - {"CommonWOLinq",15} - {result}");
    }

    [Benchmark] 
    public void CommonLinq()
    {
        var result = _dataForBenchmark.AsParallel().Sum(e=>(long)e);
        
        //Console.WriteLine($"real size: {_dataForBenchmark.Length,10} - expected size: {ArraySize,10} - {"CommonLinq",15} - {result}");
    }
    
    /// <param name="threads">0 mean 1 thread for 1 core</param>
    [Benchmark]
    [Arguments(2)]
    [Arguments(7)]
    [Arguments(0)]
    public void Thread(int threads)
    {
        if (threads == 0)
            threads = Environment.ProcessorCount;
        var partSize = ArraySize / threads; // размер куска массива
        var threadList = new List<Thread>();
        long result = 0;
        var lockobject = new object();

        for (int iterationForNewThread = 0; iterationForNewThread < threads; iterationForNewThread++)
        {
            var i = iterationForNewThread; // исключаем захват переменной тредом
            var thread = new Thread(() =>
                {
                    var from = partSize * i;
                    var to = (threads - i == 1) ? ArraySize : partSize * (i+1);
                    //       ^посл. итерация?     ^до конца   ^до конца куска массива

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
        
        //Console.WriteLine($"real size: {_dataForBenchmark.Length,10} - expected size: {ArraySize,10} - {"ThreadManual",15} - {result}");
    }

    [Benchmark]
    public void Plinq()
    {
        var result = _dataForBenchmark.AsParallel().Sum(e => (long)e);

        //Console.WriteLine($"real size: {_dataForBenchmark.Length,10} - expected size: {ArraySize,10} - {"Plinq",15} - {result}");
    }
}