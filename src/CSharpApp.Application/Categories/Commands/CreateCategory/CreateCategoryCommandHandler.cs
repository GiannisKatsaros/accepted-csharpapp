using System.Net.Http.Json;
using MediatR;

namespace CSharpApp.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Category?>
{
    private readonly HttpClient _httpClient;
    private readonly RestApiSettings _restApiSettings;
    private readonly ILogger<CreateCategoryCommandHandler> _logger;

    public CreateCategoryCommandHandler(IOptions<RestApiSettings> restApiSettings, ILogger<CreateCategoryCommandHandler> logger)
    {
        _restApiSettings = restApiSettings.Value;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_restApiSettings.BaseUrl!)
        };
        _logger = logger;
    }
    
    public async Task<Category?> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var body = JsonContent.Create(request);
            var response = await _httpClient.PostAsync(_restApiSettings.Categories, body, cancellationToken);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<Category>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error creating category using API");
            throw;
        }
    }
}