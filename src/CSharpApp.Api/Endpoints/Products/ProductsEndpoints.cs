namespace CSharpApp.Api.Endpoints.Products;

public static class ProductsEndpoints
{
    public static void MapProductsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapCreateProductEndpoint();
        endpoints.MapGetProductsEndpoint();
        endpoints.MapGetProductEndpoint();
    }
}