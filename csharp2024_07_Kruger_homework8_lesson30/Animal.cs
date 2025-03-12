namespace charp2024_07_Kruger_homework_30;

public class Animal : IMyCloneable<Animal>, ICloneable
{
    public string Species { get; set; }
    public int Age { get; set; }

    public Animal(string species, int age)
    {
        Species = species;
        Age = age;
    }
    
    protected Animal(Animal source)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
            
        Species = source.Species;
        Age = source.Age;
    }

    // MyClone вызывает конструктор, который вызывает базовый конструктор
    public virtual Animal MyClone()
    {
        return new Animal(this);
    }
    
    public object Clone()
    {
        return MyClone();
    }

    public override string ToString()
    {
        return $"Animal: Species={Species}, Age={Age}";
    }
}