namespace CSharpApp.Api.Endpoints.Products;

public static class ProductsEndpoints
{
    public static IEndpointRouteBuilder MapProductsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapCreateProductEndpoint();
        endpoints.MapGetProductsEndpoint();
        endpoints.MapGetProductEndpoint();
        
        return endpoints;
    }
}