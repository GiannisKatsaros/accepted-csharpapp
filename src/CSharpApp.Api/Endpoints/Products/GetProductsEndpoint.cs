using CSharpApp.Core.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpApp.Api.Endpoints.Products;

public static class GetProductsEndpoint
{
    public static IEndpointRouteBuilder MapGetProductsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
                .MapGet("api/v{version:apiVersion}/getproducts", GetProducts)
                .WithName("GetProducts")
                .HasApiVersion(1.0);
        
        return endpoints;
    }
    
    private static async Task<Results<Ok<IReadOnlyCollection<Product>>, ProblemHttpResult>> GetProducts(IProductsService productsService)
    {
        try
        {
            var products = await productsService.GetProducts();
            return TypedResults.Ok(products);
        }
        catch
        {
            return TypedResults.Problem("Unexpected error fetching products.");
        }
    }
}