using MediatR;

namespace CSharpApp.Application.Categories.Queries.GetCategory;

public record GetCategoryQuery : IRequest<Category?>
{
    public int Id { get; init; }
}