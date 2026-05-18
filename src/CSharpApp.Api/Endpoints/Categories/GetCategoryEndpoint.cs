using CSharpApp.Application.Categories.Queries.GetCategory;
using CSharpApp.Core.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CSharpApp.Api.Endpoints.Categories;

public static class GetCategoryEndpoint
{
    public static IEndpointRouteBuilder MapGetCategoryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGet("api/v{version:apiVersion}/categories/{id}", GetCategory)
            .WithName("GetCategory")
            .HasApiVersion(1.0);
        
        return endpoints;
    }

    private static async Task<Results<Ok<Category>, NotFound, ProblemHttpResult>> GetCategory(int id, [FromServices] ISender sender, CancellationToken cancellationToken = default)
    {
        try
        {
            var product = await sender.Send(new GetCategoryQuery{Id = id}, cancellationToken).ConfigureAwait(false);
            return product is not null
                ? TypedResults.Ok(product)
                : TypedResults.NotFound();
        }
        catch
        {
            return TypedResults.Problem($"Unexpected error fetching category {id}.");
        }
    }
}