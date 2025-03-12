namespace charp2024_07_Kruger_homework_30;

public class Mammal : Animal, IMyCloneable<Mammal>, ICloneable
{
    public string FurColor { get; set; }
    public bool IsWarmBlooded { get; set; }

    public Mammal(string species, int age, string furColor, bool isWarmBlooded) 
        : base(species, age)
    {
        FurColor = furColor;
        IsWarmBlooded = isWarmBlooded;
    }
    
    protected Mammal(Mammal source) : base(source)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
            
        FurColor = source.FurColor;
        IsWarmBlooded = source.IsWarmBlooded;
    }

    // MyClone вызывает конструктор, который вызывает базовый конструктор
    public new Mammal MyClone()
    {
        return new Mammal(this);
    }
    
    public new object Clone()
    {
        return MyClone();
    }

    public override string ToString()
    {
        return $"{base.ToString()}, FurColor={FurColor}, IsWarmBlooded={IsWarmBlooded}";
    }
}