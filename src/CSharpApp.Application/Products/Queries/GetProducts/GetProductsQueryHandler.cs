using MediatR;

namespace CSharpApp.Application.Products.Queries.GetProducts;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IReadOnlyCollection<Product>>
{
    private readonly HttpClient _httpClient;
    private readonly RestApiSettings _restApiSettings;
    private readonly ILogger<GetProductsQueryHandler> _logger;

    public GetProductsQueryHandler(IOptions<RestApiSettings> restApiSettings, ILogger<GetProductsQueryHandler> logger)
    {
        _restApiSettings = restApiSettings.Value;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_restApiSettings.BaseUrl!)
        };
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetAsync(_restApiSettings.Products, cancellationToken);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var res = JsonSerializer.Deserialize<List<Product>>(content);

            return res.AsReadOnly();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching products from API");
            throw;
        }
    }
}