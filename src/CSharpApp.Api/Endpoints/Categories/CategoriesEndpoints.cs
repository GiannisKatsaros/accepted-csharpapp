namespace CSharpApp.Api.Endpoints.Categories;

public static class CategoriesEndpoints
{
    public static IEndpointRouteBuilder MapCategoriesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapCreateCategoryEndpoint();
        endpoints.MapGetCategoriesEndpoint();
        
        return endpoints;
    }
}