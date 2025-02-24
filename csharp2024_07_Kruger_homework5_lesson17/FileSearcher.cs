using System.Timers;
using System.Xml;

namespace csharp2024_07_Kruger_homework5_lesson17;

public class FileSearcher
{
    private bool _riseEvents = false;

    public void StopNextEvents() =>
        _riseEvents = false;

    /// <summary>
    /// искать файлы с раширением заданным методу и уведомлять всех техm кто подписался на <see cref="FileFound"/>
    /// </summary>
    /// <param name="fileExtension"></param>
    public void SearchRecursively(string fileExtension)
    {
        _riseEvents = true;
        string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), $"*.{fileExtension}", SearchOption.AllDirectories);
        
        foreach (string file in files)
            if (_riseEvents)
                if (_onFileFound != null)
                    _onFileFound(this, new FileFoundArgs(file));
        
    }

    private FileFoundEventHandler? _onFileFound;
   
    /// <summary>
    /// Подписываемся на найденные файлики
    /// </summary>
    public event FileFoundEventHandler FileFound
    {
        add => _onFileFound += value;
        remove => _onFileFound -= value;
    }

}

public delegate void FileFoundEventHandler(object sender, FileFoundArgs args);

public class FileFoundArgs(string name) : EventArgs
{
    public string Name { get; set; } = name;
}