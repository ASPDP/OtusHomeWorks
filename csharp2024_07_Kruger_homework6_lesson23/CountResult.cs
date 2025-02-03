public class CountResult
{
    public CountResult(string fullFilePath, long spaceCount, TimeSpan time)
    {
        FullFilePath = fullFilePath;
        Spaces = spaceCount;
        TimeElapsed = time;
        FileName = Path.GetFileName(FullFilePath);
        FolderPath = Path.GetDirectoryName(FullFilePath);
    }

    /// <summary>
    /// Имя файла
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// Путь до папки
    /// </summary>
    public string? FolderPath { get; }

    /// <summary>
    /// Полный путь
    /// </summary>
    public string FullFilePath { get; }

    /// <summary>
    /// Времени затрачено в мсек
    /// </summary>
    public TimeSpan TimeElapsed { get; }

    /// <summary>
    /// Количество пробелов 
    /// </summary>
    public long Spaces { get; }
}