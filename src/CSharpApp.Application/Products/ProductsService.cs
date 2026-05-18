using System.Net;

namespace CSharpApp.Application.Products;

public class ProductsService : IProductsService
{
    private readonly HttpClient _httpClient;
    private readonly RestApiSettings _restApiSettings;
    private readonly ILogger<ProductsService> _logger;

    public ProductsService(IOptions<RestApiSettings> restApiSettings, 
        ILogger<ProductsService> logger)
    {
        _restApiSettings = restApiSettings.Value;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_restApiSettings.BaseUrl!)
        };
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<Product>> GetProducts()
    {
        try
        {
            var response = await _httpClient.GetAsync(_restApiSettings.Products);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var res = JsonSerializer.Deserialize<List<Product>>(content);

            return res.AsReadOnly();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching products from API");
            throw;
        }
    }

    public async Task<Product?> GetProduct(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_restApiSettings.Products}/{id}");
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return null;
            }
        
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Product>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error fetching product {Id} from API", id);
            throw;
        }
        
    }
}