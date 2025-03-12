using charp2024_07_Kruger_homework_30;

namespace csharp2024_07_Kruger_homework8_lesson30.Test;
using Xunit;

public class AnimalTests
{
    [Fact]
    public void Animal_Clone_CreatesIndependentCopy()
    {
        // Arrange
        var original = new Animal("Tiger", 5);

        // Act
        var clone = original.MyClone();
        clone.Species = "Lion";
        clone.Age = 8;

        // Assert
        Assert.Equal("Tiger", original.Species);
        Assert.Equal(5, original.Age);
        Assert.Equal("Lion", clone.Species);
        Assert.Equal(8, clone.Age);
        Assert.NotSame(original, clone);
    }

    [Fact]
    public void Mammal_Clone_CreatesIndependentCopy()
    {
        // Arrange
        var original = new Mammal("Bear", 10, "Brown", true);

        // Act
        var clone = original.MyClone();
        clone.Species = "Polar Bear";
        clone.FurColor = "White";

        // Assert
        Assert.Equal("Bear", original.Species);
        Assert.Equal("Brown", original.FurColor);
        Assert.Equal("Polar Bear", clone.Species);
        Assert.Equal("White", clone.FurColor);
        Assert.NotSame(original, clone);
    }

    [Fact]
    public void Dog_Clone_CreatesIndependentCopy()
    {
        // Arrange
        var original = new Dog("Canine", 3, "Black", true, "German Shepherd", true);

        // Act
        var clone = original.MyClone();
        clone.Breed = "Husky";
        clone.IsTrained = false;

        // Assert
        Assert.Equal("German Shepherd", original.Breed);
        Assert.True(original.IsTrained);
        Assert.Equal("Husky", clone.Breed);
        Assert.False(clone.IsTrained);
        Assert.NotSame(original, clone);
    }
}