using System.IO.Compression;

/// <summary>
/// Простой разархиватор brotli с одним статическим методом
/// выбран потому, что zip хуже сжимает большие текстовые файлы
/// </summary>
public static class BrotliArchiver
{
    public const string UnarchPostfix = ".decompressed";
    
    /// <summary>
    /// Разархивировать в папку с перезаписью, добавляя ко всем файлам .decompressed
    /// </summary>
    /// <param name="target">Полный путь до архива C:\tmp\compressed.br</param>
    /// <param name="outputFolder">Полный путь до папки разархивирования,
    /// к файлу добавится <see cref="UnarchPostfix"/></param>
    public static void ForcedFileUnarch(string target, string outputFolder)
    {
        var outputFile = Path.Combine(outputFolder, Path.GetFileName(target) + UnarchPostfix);
        
        using var inputStream = new FileStream(target, FileMode.Open, FileAccess.Read);
        using var outputStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
        using var brotliStream = new BrotliStream(inputStream, CompressionMode.Decompress);
        brotliStream.CopyTo(outputStream);
    }
}