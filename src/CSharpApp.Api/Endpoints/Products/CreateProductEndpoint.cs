using CSharpApp.Application.Products.Commands.CreateProduct;
using CSharpApp.Core.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpApp.Api.Endpoints.Products;

public static class CreateProductEndpoint
{
    public static IEndpointRouteBuilder MapCreateProductEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapPost("api/v{version:apiVersion}/products", CreateProduct)
            .WithName("CreateProduct")
            .HasApiVersion(1.0);
        
        return endpoints;
    }

    private static async Task<Results<Ok<Product>, ProblemHttpResult>> CreateProduct(CreateProductCommand request, ISender sender, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await sender.Send(request, cancellationToken).ConfigureAwait(false);
            return result is not null
                ? TypedResults.Ok(result)
                : TypedResults.Problem("Error creating product");
        }
        catch
        {
            return TypedResults.Problem("Error creating product");
        }
    }
}