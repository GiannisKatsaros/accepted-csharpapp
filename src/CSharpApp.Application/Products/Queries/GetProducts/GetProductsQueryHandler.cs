using CSharpApp.Application.Interfaces;
using CSharpApp.Application.Products.Queries.GetProduct;
using MediatR;

namespace CSharpApp.Application.Products.Queries.GetProducts;

public class GetProductsQueryHandler(IExternalApiClient httpClient, IOptions<RestApiSettings> restApiSettings, ILogger<GetProductsQueryHandler> logger) : IRequestHandler<GetProductsQuery, IReadOnlyCollection<Product>>
{
    private readonly RestApiSettings _restApiSettings = restApiSettings.Value;

    public async Task<IReadOnlyCollection<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.Get(_restApiSettings.Products, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            var res = JsonSerializer.Deserialize<List<Product>>(content);

            return res.AsReadOnly();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching products from API");
            throw;
        }
    }
}