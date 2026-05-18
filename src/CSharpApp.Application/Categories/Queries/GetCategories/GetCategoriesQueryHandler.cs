using MediatR;

namespace CSharpApp.Application.Categories.Queries.GetCategories;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IReadOnlyCollection<Category>>
{
    private readonly HttpClient _httpClient;
    private readonly RestApiSettings _restApiSettings;
    private readonly ILogger<GetCategoriesQueryHandler> _logger;

    public GetCategoriesQueryHandler(IOptions<RestApiSettings> restApiSettings, ILogger<GetCategoriesQueryHandler> logger)
    {
        _restApiSettings = restApiSettings.Value;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_restApiSettings.BaseUrl!)
        };
        _logger = logger;
    }
    
    public async Task<IReadOnlyCollection<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetAsync(_restApiSettings.Categories, cancellationToken);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var res = JsonSerializer.Deserialize<List<Category>>(content);

            return res.AsReadOnly();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching categories from API");
            throw;
        }
    }
}