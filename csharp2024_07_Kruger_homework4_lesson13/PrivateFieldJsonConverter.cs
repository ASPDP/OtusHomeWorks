using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

public class PrivateFieldJsonConverter<T> : JsonConverter<T> where T : class
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException("Read method is not implemented!");
    }
    

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        
        // NonPublic исползуем с инстанс, иначе магии не произойдет :)
        var fields = value.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var field in fields)
        {
            writer.WritePropertyName(field.Name);
            JsonSerializer.Serialize(writer, field.GetValue(value), field.FieldType, options);
        }
        
        writer.WriteEndObject();
    }
}
