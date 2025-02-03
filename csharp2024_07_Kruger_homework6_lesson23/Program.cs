// За неимением технического образования, было круто
// закрыть гештальт связанный с пониманием того,
// как устроено большинство популярных кодировок :)
//
// Конечно, это практически не имеет отношения к заданию,
// но я подумал, почему бы не искать пробелы как последовательность 
// битов. На случай, если на вход метода подадут
// файл размером в несколько ГБ.
//
// Я почитал как устроены популярные кодировки и это оказалось
// не так уж и сложно. Полагаю, что будет работать с
// UTF8/UTF8 with BOM, KOI8-R, cp1251, OEM866
// + с остальными 8-битовыми кодировками основанными на ASCII
// с пробелом закодированным как 00100000
//
// НО
// так же выяснил, что этот код не будет работать
// для супер древних файлов из 60х в которых текст
// закодирован последовательностями в 7 бит. И не будет работать
// с UTF16 и UTF32, хотя для последних можно просто читать 
// по 2/4 байта соотв., но пришлось бы еще заморочиться с BOM LE/BE.
//
// Ну и да, открыл для себя Brotli, что бы сжать 2 ГигаБайта
// одинаковых текстовых параграфов, zip это делает хуже.
//
// Еще узнал, что у китайцев нет пробелов :)

using System.Globalization;
using Humanizer;

//---------------------------------------------------------
//                    WAIT USER INPUT 
//---------------------------------------------------------
Console.WriteLine("Нажмите S что бы стартовать распаковку ~2.5GB файлов для тестирования...");
if (Console.ReadKey(true).Key != ConsoleKey.S)
    return;

//---------------------------------------------------------
//            PREPARE ~2.5GB FILES FOR HOMEWORK
//---------------------------------------------------------
const string demoFolderName = "DELETE_ME_PLEASE";
const string archivesFolderName = "BROTLI_ARCHIVES";

var usrFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
var demoFolderFullPath = Path.Combine(usrFolder, demoFolderName);
var archivesFullPath = Path.Combine(Directory.GetCurrentDirectory(), archivesFolderName);

Console.WriteLine("Распаковываем данные для ДЗ...");
Directory.CreateDirectory(demoFolderFullPath);

var brotliArchives = Directory.GetFiles(archivesFullPath, "*.br");
if (brotliArchives.Length == 0)
    throw new Exception($"Нет .br архивов в папке {archivesFullPath}");

foreach (var archive in brotliArchives)
    BrotliArchiver.ForcedFileUnarch(archive, demoFolderFullPath);

//---------------------------------------------------------
//               DO HOMEWORK TASKS 1+2,3
//---------------------------------------------------------

Console.WriteLine("Считаем пробелы в каждом файле асинхронно...");
var awaitableTasks = HomeWorkFunction(demoFolderFullPath);
//вот тут можно что-то поделать, поток не заблокирован

await VeryImportantWork();

WriteResultsIntoConsole(await awaitableTasks); // эксепшны, если есть - поваляться тут, во время "распаковки таски"

Console.WriteLine("Подсчитано, выведено, нажмите Enter для удаления папки с текстами.");
//---------------------------------------------------------
//                    WAIT USER INPUT 
//---------------------------------------------------------
Console.ReadLine();

// Из-за того, что файлы очень большие, добавил проверку
// их удаления и сообщение пользователю о том, что произошла ошибка
// и надо удалить файлы самостоятельно
try
{
    Directory.Delete(demoFolderFullPath, true);
}
catch (Exception e)
{
    Console.WriteLine($"""
                       Ошибка удаления!
                       Удалите файлы самостоятельно из папки {demoFolderFullPath}
                       {e}
                       """);
    throw;
}

Console.WriteLine("Папка успешно удалена.");


//---------------------------------------------------------
//                   ЛОКАЛЬНЫЕ ФУНКЦИИ
//---------------------------------------------------------

// собственно само домашнее задание. ФУНКЦИЯ ПРИНИМАЮЩАЯ ПУТЬ К ПАПКЕ
async Task<CountResult[]> HomeWorkFunction(string path)
{
    var counter = new SpaceCounter();
    var tasks = counter.RunTasksForFolder(
        fullFolderPath: path,
        pattern: "*" + BrotliArchiver.UnarchPostfix
    );

    return await Task.WhenAll(tasks);
}

// Воспользовался Humanizer
async void WriteResultsIntoConsole(CountResult[] results)
{
    foreach (var rslt in results)
        Console.WriteLine(
            $"""
             ┌ - -  -   -    -     -
             | имя: {rslt.FileName}
             | пробелов: {rslt.Spaces}
             └ время: {rslt.TimeElapsed.Humanize(2, new CultureInfo("ru"))}
             """);
}

// имитируем полезную работу
async Task VeryImportantWork()
{
    var cp = Console.GetCursorPosition();
    Console.CursorVisible = false;
    while (!awaitableTasks.IsCompleted) // если хотя бы одна таска умрет, мы никогда не получим true :)
    {
        switch (new Random().Next(0, 5))
        {
            case 1:
                Console.WriteLine(@"_(o_o)_");
                break;

            case 2:
                Console.WriteLine(@"_(o_O)/");
                break;

            case 3:
                Console.WriteLine(@"\(O_o)_");
                break;

            case 4:
                Console.WriteLine(@"\(O_O)/");
                break;
        }

        Console.SetCursorPosition(cp.Left, cp.Top);
        await Task.Delay(500);
    }

    Console.SetCursorPosition(0, cp.Top);
    Console.WriteLine("_(-_-)_");
    Console.CursorVisible = true;
}
