using MediatR;

namespace CSharpApp.Application.Products.Commands.CreateProduct;

public record CreateProductCommand : IRequest<Product?>
{
    public string Title { get; init; } = null!; 
    public int Price { get; init; }
    public string Description { get; init; } = null!;
    public int CategoryId { get; init; }
    public List<string> Images { get; init; } = [];
}