using CSharpApp.Core.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpApp.Api.Endpoints.Products;

public static class GetProductEndpoint
{
    public static IEndpointRouteBuilder MapGetProductEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGet("api/v{version:apiVersion}/getproducts/{id}", GetProduct)
            .WithName("GetProduct")
            .HasApiVersion(1.0);
        
        return endpoints;
    }
    
    private static async Task<Results<Ok<Product>, NotFound, ProblemHttpResult>> GetProduct(int id, IProductsService productsService)
    {
        try
        {
            var product = await productsService.GetProduct(id);
            return product is not null
                ? TypedResults.Ok(product)
                : TypedResults.NotFound();
        }
        catch
        {
            return TypedResults.Problem($"Unexpected error fetching product {id}.");
        }
    }
}