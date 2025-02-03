using System.Diagnostics;
using System.Numerics;
using System.Text;

/// <summary>
/// Космический считалец 
/// </summary>
public class SpaceCounter
{
    private const byte targetByte = 0x20;
    
    private List<Task<CountResult>> _countingTasks = new();
    
    /// <summary>
    /// Рекурсивно находит все файлы в папке, и параллельно запускает
    /// для каждого файла <see cref="CountSpacesInFile"/> подсчет пробелов через Task.Run(()=>{ })
    /// получить список тасок можно через другой метод
    /// </summary>
    /// <param name="fullFolderPath">Полный путь до папки</param>
    /// <param name="pattern">паттерн для поиска файлов, к примеру: *.doc, по-умолчанию используется "все файлы": *</param>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="FileNotFoundException">Нечего подсчитывать, файлы не найдены.</exception>
    public List<Task<CountResult>> RunTasksForFolder(string fullFolderPath, string pattern = "*")
    {
        if (!Directory.Exists(fullFolderPath))
            throw new DirectoryNotFoundException($"Папка для поиска текстовых файлов отсутствует {fullFolderPath}");
        
        var texts = Directory.GetFiles(fullFolderPath, pattern, SearchOption.AllDirectories);
        if (texts.Length == 0)
            throw new FileNotFoundException(
                $"""
                 Нечего подсчитывать, файлы не найдены. 
                 Был использован путь - {fullFolderPath}. 
                 Был использован wildcard паттерн - {pattern}.
                 """);
        
        foreach (var file in texts)
            //таски начинают выполняться до добавляния из-за неявного .Invoke(file) 
            _countingTasks.Add(CountSpacesInFileAsync(file));

        return _countingTasks;
    }


    /// <summary>
    /// Считает количество пробелов в файлах с 8 битной кодировкой асинхронно,
    /// в которых пробел представляет из себя последовательность бит
    /// 00100000
    /// </summary>
    /// <param name="fullFilePath">Полный путь до файлы</param>
    /// <exception cref="ArgumentException">Проверка строки на IsNullOrWhiteSpace</exception>
    /// <exception cref="DirectoryNotFoundException">Проверка файла на наличие перед чтением</exception>
    public async Task<CountResult> CountSpacesInFileAsync(string fullFilePath)
    {
        var sw = new Stopwatch();
        sw.Start();

        if (string.IsNullOrWhiteSpace(fullFilePath))
            throw new ArgumentException($"Переданная в метод строка пути пуста или null {fullFilePath}");

        if (!File.Exists(fullFilePath))
            throw new DirectoryNotFoundException($"Файл не найден по переданному в метод пути {fullFilePath}");

        long spaceCounter = 0;
        using FileStream stream = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read);

        // попробовал разные значения,
        // что бы плодить меньше тасок
        // аж до 1073741824 байт, но
        // для больших файлов
        // время выполнения особо не поменялось
        int buffersize = 1073741824;
        byte[] buffer = new byte[buffersize];
        int bytesRead;

        while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            for (int i = 0; i < bytesRead; i++)
                if (buffer[i] == targetByte)
                    spaceCounter++;

        // если просто читать каждый байт, без буферизации,
        // каждый раз запрашивая позицию и длину,
        // то выходит в ~10 раз медленнее,
        // ну т.е. если вот так:
        /*
         long spaceCounter = 0;
         while (fs.Position != fs.Length)
             if (fs.ReadByte() == targetByte)
                 spaceCounter++;
         */

        sw.Stop();
        return new CountResult(fullFilePath, spaceCounter, sw.Elapsed);
    }
}