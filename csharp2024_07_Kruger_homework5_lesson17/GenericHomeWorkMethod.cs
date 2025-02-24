using System.Collections;

namespace csharp2024_07_Kruger_homework5_lesson17;

public static class GenericHomeWorkMethod
{
    /// <summary>
    /// Код нагло украден из кода Linq для MaxFloat и приправлен не популярным Tuple :)
    /// Единственное, не знаю нужно ли мне тут обрабатывать элемент коллекции равный null,
    /// я выбрал стратегию fail fast и оставил обработку null на откуп пользователю, который 
    /// имплементриует convertToNumber.
    /// Если бы в условии задания был IE
    /// </summary>
    /// <param name="source"></param>
    /// <param name="convertToNumber">Метод, который так же должен обрабатывать null как элемент коллекции</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static T GetMax<T>(this IEnumerable<T> source, Func<T, float> convertToNumber)
        where T : class
    {
        (float, T) value;

        (float, T) convertButTuple(T elem) =>
            (convertToNumber(elem), elem);
        
        using (IEnumerator<T> e = source.GetEnumerator())
        {
            if (!e.MoveNext())
                throw new InvalidOperationException("No elements are available!");

            value = convertButTuple(e.Current);
            
            while (float.IsNaN(value.Item1))
            {
                if (!e.MoveNext())
                    return value.Item2;

                value = convertButTuple(e.Current);
            }

            while (e.MoveNext())
            {
                var x = convertButTuple(e.Current);
                if (x.Item1 > value.Item1)
                    value = x;
            }
        }

        return value.Item2;
    }
    
    
    
}

