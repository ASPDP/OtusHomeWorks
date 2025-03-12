namespace charp2024_07_Kruger_homework_30;

public class Dog : Mammal, IMyCloneable<Dog>, ICloneable
{
    public string Breed { get; set; }
    public bool IsTrained { get; set; }

    public Dog(string species, int age, string furColor, bool isWarmBlooded, string breed, bool isTrained) 
        : base(species, age, furColor, isWarmBlooded)
    {
        Breed = breed;
        IsTrained = isTrained;
    }
    
    protected Dog(Dog source) : base(source)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
            
        Breed = source.Breed;
        IsTrained = source.IsTrained;
    }

    // MyClone вызывает конструктор, который вызывает базовый конструктор
    public new Dog MyClone()
    {
        return new Dog(this);
    }
    
    public new object Clone()
    {
        return MyClone();
    } 

    public override string ToString()
    {
        return $"{base.ToString()}, Breed={Breed}, IsTrained={IsTrained}";
    }
}