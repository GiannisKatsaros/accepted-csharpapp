using System.Net;
using System.Net.Http.Json;
using MediatR;

namespace CSharpApp.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product?>
{
    private readonly HttpClient _httpClient;
    private readonly RestApiSettings _restApiSettings;
    private readonly ILogger<CreateProductCommandHandler> _logger;

    public CreateProductCommandHandler(IOptions<RestApiSettings> restApiSettings, ILogger<CreateProductCommandHandler> logger)
    {
        _restApiSettings = restApiSettings.Value;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_restApiSettings.BaseUrl!)
        };
        _logger = logger;
    }

    public async Task<Product?> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var body = JsonContent.Create(request);
            var response = await _httpClient.PostAsync(_restApiSettings.Products, body, cancellationToken);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<Product>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error creating product using API");
            throw;
        }
    }
}