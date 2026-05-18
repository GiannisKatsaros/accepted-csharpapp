using CSharpApp.Application.Categories.Queries.GetCategories;
using CSharpApp.Core.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CSharpApp.Api.Endpoints.Categories;

public static class GetCategoriesEndpoint
{
    public static IEndpointRouteBuilder MapGetCategoriesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGet("api/v{version:apiVersion}/categories", GetCategories)
            .WithName("GetCategories")
            .HasApiVersion(1.0);
        
        return endpoints;
    }

    private static async Task<Results<Ok<IReadOnlyCollection<Category>>, ProblemHttpResult>> GetCategories([FromServices] ISender sender, CancellationToken cancellationToken = default)
    {
        try
        {
            var categories = await sender.Send(new GetCategoriesQuery(), cancellationToken);
            return TypedResults.Ok(categories);
        }
        catch
        {
            return TypedResults.Problem("Unexpected error fetching categories.");
        }
    }
}