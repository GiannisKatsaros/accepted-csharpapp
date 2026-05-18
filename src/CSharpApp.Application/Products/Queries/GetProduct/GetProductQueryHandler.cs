using System.Net;
using CSharpApp.Application.Interfaces;
using MediatR;

namespace CSharpApp.Application.Products.Queries.GetProduct;

public class GetProductQueryHandler(IExternalApiClient httpClient, IOptions<RestApiSettings> restApiSettings, ILogger<GetProductQueryHandler> logger) : IRequestHandler<GetProductQuery, Product?>
{
    private readonly RestApiSettings _restApiSettings = restApiSettings.Value;
    
    public async Task<Product?> Handle(GetProductQuery request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.Get($"{_restApiSettings.Products}/{request.Id}", cancellationToken)
                .ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return null;
            }
        
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            return JsonSerializer.Deserialize<Product>(content);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled error fetching product {Id} from API", request.Id);
            throw;
        }
    }
}