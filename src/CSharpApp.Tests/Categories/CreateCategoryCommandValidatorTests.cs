using CSharpApp.Application.Categories.Commands.CreateCategory;

namespace CSharpApp.Tests.Categories;

public class CreateCategoryCommandValidatorTests
{
    private readonly CreateCategoryCommandValidator _validator = new();

    private static CreateCategoryCommand Valid() => new()
    {
        Name = "Books",
        Image = "https://api.lorem.space/image/book?w=150&h=220"
    };

    [Fact]
    public void Validate_Passes_WhenAllFieldsValid()
    {
        var result = _validator.Validate(Valid());
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_Fails_WhenNameEmpty()
    {
        var result = _validator.Validate(Valid() with { Name = "" });
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateCategoryCommand.Name));
    }

    [Fact]
    public void Validate_Fails_WhenImageNotUrl()
    {
        var result = _validator.Validate(Valid() with { Image = "not-a-url" });
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateCategoryCommand.Image));
    }

    [Fact]
    public void Validate_Fails_WhenImageEmpty()
    {
        var result = _validator.Validate(Valid() with { Image = "" });
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateCategoryCommand.Image));
    }
}