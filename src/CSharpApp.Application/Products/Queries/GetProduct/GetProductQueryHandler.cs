using System.Net;
using MediatR;

namespace CSharpApp.Application.Products.Queries.GetProduct;

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Product?>
{
    private readonly HttpClient _httpClient;
    private readonly RestApiSettings _restApiSettings;
    private readonly ILogger<GetProductQueryHandler> _logger;

    public GetProductQueryHandler(IOptions<RestApiSettings> restApiSettings, ILogger<GetProductQueryHandler> logger)
    {
        _restApiSettings = restApiSettings.Value;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_restApiSettings.BaseUrl!)
        };
        _logger = logger;
    }
    
    public async Task<Product?> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_restApiSettings.Products}/{request.Id}", cancellationToken);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return null;
            }
        
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<Product>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error fetching product {Id} from API", request.Id);
            throw;
        }
    }
}