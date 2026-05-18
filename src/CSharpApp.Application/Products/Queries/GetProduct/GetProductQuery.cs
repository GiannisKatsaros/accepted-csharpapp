using MediatR;

namespace CSharpApp.Application.Products.Queries.GetProduct;

public record GetProductQuery : IRequest<Product?>
{
    public int Id { get; init; }
}