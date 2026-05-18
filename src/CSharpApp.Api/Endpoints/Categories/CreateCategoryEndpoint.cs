using CSharpApp.Application.Categories.Commands.CreateCategory;
using CSharpApp.Core.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpApp.Api.Endpoints.Categories;

public static class CreateCategoryEndpoint
{
    public static IEndpointRouteBuilder MapCreateCategoryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapPost("api/v{version:apiVersion}/categories", CreateCategory)
            .WithName("CreateCategory")
            .HasApiVersion(1.0);
        
        return endpoints;
    }

    private static async Task<Results<Ok<Category>, ProblemHttpResult>> CreateCategory(CreateCategoryCommand request, ISender sender, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await sender.Send(request, cancellationToken);
            return result is not null
                ? TypedResults.Ok(result)
                : TypedResults.Problem("Error creating category");
        }
        catch
        {
            return TypedResults.Problem("Error creating category");
        }
    }
}