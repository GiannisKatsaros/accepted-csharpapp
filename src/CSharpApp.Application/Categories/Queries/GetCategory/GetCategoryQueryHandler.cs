using System.Net;
using MediatR;

namespace CSharpApp.Application.Categories.Queries.GetCategory;

public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, Category?>
{
    private readonly HttpClient _httpClient;
    private readonly RestApiSettings _restApiSettings;
    private readonly ILogger<GetCategoryQueryHandler> _logger;

    public GetCategoryQueryHandler(IOptions<RestApiSettings> restApiSettings, ILogger<GetCategoryQueryHandler> logger)
    {
        _restApiSettings = restApiSettings.Value;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_restApiSettings.BaseUrl!)
        };
        _logger = logger;
    }
    
    public async Task<Category?> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_restApiSettings.Categories}/{request.Id}", cancellationToken);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return null;
            }
        
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<Category>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error fetching category {Id} from API", request.Id);
            throw;
        }
    }
}