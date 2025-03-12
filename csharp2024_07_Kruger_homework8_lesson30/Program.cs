namespace charp2024_07_Kruger_homework_30;

public abstract class Program
{
    public static void Main(string[] args)
    {
        //  недостаток ICloneable в том, что он возвращает слишком самый верхнеуровневый тип - object
        
        // создаем инстанс каждого класса
        Animal animal = new Animal("Generic Animal", 5);
        Mammal mammal = new Mammal("Cat", 3, "Orange", true);
        Dog dog = new Dog("Dog", 2, "Brown", true, "Golden Retriever", true);
        
        // клонируем каждый инстанс
        Animal clonedAnimal = animal.MyClone();
        Mammal clonedMammal = mammal.MyClone();
        Dog clonedDog = dog.MyClone();
        
        // демонстрируем что это независимые копии
        Console.WriteLine("Original objects:");
        Console.WriteLine(animal);
        Console.WriteLine(mammal);
        Console.WriteLine(dog);
        
        Console.WriteLine("\nCloned objects:");
        Console.WriteLine(clonedAnimal);
        Console.WriteLine(clonedMammal);
        Console.WriteLine(clonedDog);
        
        // модицифируем клонированные объекты
        clonedAnimal.Age = 10;
        clonedMammal.FurColor = "Black";
        clonedDog.Breed = "Labrador";
        
        Console.WriteLine("\nAfter modification:");
        Console.WriteLine($"Original animal: {animal}");
        Console.WriteLine($"Cloned animal: {clonedAnimal}");
        
        Console.WriteLine($"Original mammal: {mammal}");
        Console.WriteLine($"Cloned mammal: {clonedMammal}");
        
        Console.WriteLine($"Original dog: {dog}");
        Console.WriteLine($"Cloned dog: {clonedDog}");
    }
}