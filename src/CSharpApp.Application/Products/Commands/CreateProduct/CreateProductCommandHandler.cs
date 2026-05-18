using CSharpApp.Application.Interfaces;
using MediatR;

namespace CSharpApp.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler(IExternalApiClient httpClient, IOptions<RestApiSettings> restApiSettings, ILogger<CreateProductCommandHandler> logger) : IRequestHandler<CreateProductCommand, Product?>
{
    private readonly RestApiSettings _restApiSettings = restApiSettings.Value;

    public async Task<Product?> Handle(CreateProductCommand request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.Post(_restApiSettings.Products, request, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            return JsonSerializer.Deserialize<Product>(content);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled error creating product using API");
            throw;
        }
    }
}