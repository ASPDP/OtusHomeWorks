using System.Text.Json;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<Measures>();
     
public class Measures
{    
    public static string CsvText = CsvHelper.SerializeToCsv(F.Get());
    public static  F HomeWorkVariant = F.Get();
    public static rF WeHaveRecord = rF.Get();
    
    [Benchmark]
    public void RecordSerialization()
    {
        Console.WriteLine(WeHaveRecord);
    }

    [Benchmark]
    // для решения пунктов 1-3 подойдет уже существующий JSON,
    // с небольшой поправкой, напишем свой конвертер,
    // т.к. не уверен, что можно опциями и без добавл. атрибутов
    // заставить сериализовать private fields класса F
    // а в рамках задачи филды заявлены приватными
    public void JsonWithCustomConverterSerialization()
    {
        var options = new JsonSerializerOptions
        {
            Converters = { new PrivateFieldJsonConverter<F>() },
        };

        Console.WriteLine(JsonSerializer.Serialize(HomeWorkVariant, options));
    }

    [Benchmark]
    public void Deserialisation()
    {
        CsvHelper.DeserializeFromCsv<F>(CsvText);
        // ну наверное смысла десериализовывать его нет, это еще сильнее замедлит выполнение
    }
}