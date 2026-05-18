using CSharpApp.Application.Products.Queries.GetProduct;
using CSharpApp.Core.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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
    
    private static async Task<Results<Ok<Product>, NotFound, ProblemHttpResult>> GetProduct(int id, [FromServices] ISender sender)
    {
        try
        {
            var product = await sender.Send(new GetProductQuery{Id = id}).ConfigureAwait(false);
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