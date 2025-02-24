namespace csharp2024_07_Kruger_homework5_lesson17;

/// <summary>
/// Создает и при надобности подчищает созданные файлы с расширением значения константы <see cref="_uniqueFileExtension"/>
/// </summary>
/// <param name="files">количество файлов для создания. Точка булде</param>
/// <param name="userFileExtension">расшинерения файлов. первая точка будет удалена :)</param>
public class FileCreator(int files, string userFileExtension)
{
    //удаляем точку
    private string fileExtension = (userFileExtension.IndexOf('.')==0) ? userFileExtension.Remove(0,1): userFileExtension;

    /// <summary>
    /// создать файлов с уникальным расширением 
    /// </summary>
    public void Create()
    {
        for (int i = 0; i < files; i++)
        {
            var tickstamp = DateTime.Now.Ticks;
            string fileName = $"{tickstamp}.{fileExtension}";

            // Create the file and write a sample text into it
            File.WriteAllText(fileName, $"delete me please");
        }
    }

    /// <summary>
    /// убрать файлы которые мы ранее наплодили основываясь на том, что мы передали в прошлый раз в метод Create
    /// </summary>
    /// <returns>Возвращает List с отчетом о проделанных операциях и возникших ошибках</returns>
    public List<string> Clean()
    {
        List<string> fileErrors = [""];

        string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), $"*.{fileExtension}",
            SearchOption.AllDirectories);

        var filesForDelete = files.Length;
        foreach (string file in files)
            try
            {
                File.Delete(file);
                filesForDelete--;
            }
            catch (Exception e)
            {
                fileErrors.Add( $"не удалось удалить файл с именем {file}.{Environment.NewLine}{e}");
            }
        
        fileErrors[0]=$"Файлов найдено: {files.Length}; осталось удалить: {filesForDelete}.";
        return fileErrors;
    }
}