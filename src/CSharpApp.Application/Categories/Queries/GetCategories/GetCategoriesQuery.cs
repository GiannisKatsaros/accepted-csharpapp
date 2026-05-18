using MediatR;

namespace CSharpApp.Application.Categories.Queries.GetCategories;

public record GetCategoriesQuery() : IRequest<IReadOnlyCollection<Category>>;