using CSharpApp.Application.Products.Commands.CreateProduct;

namespace CSharpApp.Tests.Products;

public class CreateProductCommandValidatorTests
{
    private readonly CreateProductCommandValidator _validator = new();

    private static CreateProductCommand Valid() => new()
    {
        Title = "Widget",
        Price = 10,
        Description = "A widget",
        CategoryId = 1,
        Images = ["https://example.com/img.jpg"]
    };

    [Fact]
    public void Validate_Passes_WhenAllFieldsValid()
    {
        var result = _validator.Validate(Valid());
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_Fails_WhenTitleEmpty()
    {
        var result = _validator.Validate(Valid() with { Title = "" });
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateProductCommand.Title));
    }

    [Fact]
    public void Validate_Fails_WhenPriceZero()
    {
        var result = _validator.Validate(Valid() with { Price = 0 });
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateProductCommand.Price));
    }

    [Fact]
    public void Validate_Fails_WhenCategoryIdZero()
    {
        var result = _validator.Validate(Valid() with { CategoryId = 0 });
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateProductCommand.CategoryId));
    }

    [Fact]
    public void Validate_Fails_WhenImagesEmpty()
    {
        var result = _validator.Validate(Valid() with { Images = [] });
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateProductCommand.Images));
    }

    [Fact]
    public void Validate_Fails_WhenImageNotUrl()
    {
        var result = _validator.Validate(Valid() with { Images = ["not-a-url"] });
        Assert.Contains(result.Errors, e => e.PropertyName.StartsWith("Images"));
    }
}