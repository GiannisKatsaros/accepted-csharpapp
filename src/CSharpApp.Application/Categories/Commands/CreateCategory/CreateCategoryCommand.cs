using MediatR;

namespace CSharpApp.Application.Categories.Commands.CreateCategory;

public record CreateCategoryCommand : IRequest<Category?>
{
    public string Name { get; set; } = null!;
    public string Image { get; set; } = null!;
}