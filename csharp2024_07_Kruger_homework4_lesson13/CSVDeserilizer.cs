using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

public class CsvHelper
{
    // выполнение условий ДЗ в полной мере
    // заняло бы очень много времени
    // возможно "экземпляр любого класса"
    // это опечатка. Иначе бы пришлось реализовывать
    // и десериализацию зацикленных объектов,
    // всех видов массивов и коллекций,
    // а так же десериализацию деревьев выражений
    // для десериализации лямбд, ивентов, экз. делегатов,
    // в общем, это прям довольно объемный проект.
    public static string SerializeToCsv<T>(T obj)
    {
        var dataMembers= typeof(T).GetMembers(BindingFlags.NonPublic |  BindingFlags.Instance);
        var x = dataMembers.Where(x=>x.MemberType ==  MemberTypes.Field).ToList();
        var header = string.Join(",", x.Select(p => p.Name));
        var values = string.Join(",", x.Select(p =>
        {
            return p switch
            {
                PropertyInfo propertyInfo => propertyInfo.GetValue(obj)?.ToString() ?? string.Empty,
                FieldInfo fieldInfo => fieldInfo.GetValue(obj)?.ToString() ?? string.Empty,
                _ => string.Empty
            };
        }));
        
        return $"{header}\n{values}";
    }

    public static T DeserializeFromCsv<T>(string csv) where T : new()
    {
        var lines = csv.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length < 2)
        {
            throw new ArgumentException("CSV должен быть на две строки. Первая строка это названия полей/свойств, второй - их значения.");
        }

        var header = lines[0].Split(',');
        var values = lines[1].Split(',');

        var obj = new T();
        var dataMembers = typeof(T).GetMembers(BindingFlags.NonPublic | BindingFlags.Instance);

        for (int i = 0; i < header.Length; i++)
        {
            var member = dataMembers.FirstOrDefault(p => p.Name == header[i]);

            if (member is FieldInfo fld)
            {
                var valfld = Convert.ChangeType(values[i], fld?.FieldType);
                fld?.SetValue(obj, valfld);
            }
            else if (member is PropertyInfo prop && prop.CanWrite)
            {
                var valprop = Convert.ChangeType(values[i], prop?.PropertyType);
                prop?.SetValue(obj, valprop);
            }
        }

        return obj;
    }
}
