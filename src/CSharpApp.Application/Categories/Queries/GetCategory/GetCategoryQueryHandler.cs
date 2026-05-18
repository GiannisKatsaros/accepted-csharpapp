using System.Net;
using CSharpApp.Application.Interfaces;
using CSharpApp.Application.Products.Commands.CreateProduct;
using MediatR;

namespace CSharpApp.Application.Categories.Queries.GetCategory;

public class GetCategoryQueryHandler(IExternalApiClient httpClient, IOptions<RestApiSettings> restApiSettings, ILogger<GetCategoryQueryHandler> logger) : IRequestHandler<GetCategoryQuery, Category?>
{
    private readonly RestApiSettings _restApiSettings = restApiSettings.Value;
    
    public async Task<Category?> Handle(GetCategoryQuery request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.Get($"{_restApiSettings.Categories}/{request.Id}", cancellationToken).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return null;
            }
        
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            return JsonSerializer.Deserialize<Category>(content);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled error fetching category {Id} from API", request.Id);
            throw;
        }
    }
}