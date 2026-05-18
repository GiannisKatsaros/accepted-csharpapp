using CSharpApp.Application.Products.Queries.GetProducts;
using CSharpApp.Core.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CSharpApp.Api.Endpoints.Products;

public static class GetProductsEndpoint
{
    public static IEndpointRouteBuilder MapGetProductsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
                .MapGet("api/v{version:apiVersion}/products", GetProducts)
                .WithName("GetProducts")
                .HasApiVersion(1.0);
        
        return endpoints;
    }
    
    private static async Task<Results<Ok<IReadOnlyCollection<Product>>, ProblemHttpResult>> GetProducts([FromServices] ISender sender, CancellationToken cancellationToken = default)
    {
        try
        {
            var products = await sender.Send(new GetProductsQuery(), cancellationToken).ConfigureAwait(false);
            return TypedResults.Ok(products);
        }
        catch
        {
            return TypedResults.Problem("Unexpected error fetching products.");
        }
    }
}