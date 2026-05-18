using CSharpApp.Application.Interfaces;
using MediatR;

namespace CSharpApp.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler(IExternalApiClient httpClient, IOptions<RestApiSettings> restApiSettings, ILogger<CreateCategoryCommandHandler> logger) : IRequestHandler<CreateCategoryCommand, Category?>
{
    private readonly RestApiSettings _restApiSettings = restApiSettings.Value;
    
    public async Task<Category?> Handle(CreateCategoryCommand request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.Post(_restApiSettings.Categories, request, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            return JsonSerializer.Deserialize<Category>(content);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled error creating category using API");
            throw;
        }
    }
}