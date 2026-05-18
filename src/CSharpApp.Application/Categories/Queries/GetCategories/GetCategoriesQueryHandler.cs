using CSharpApp.Application.Interfaces;
using MediatR;

namespace CSharpApp.Application.Categories.Queries.GetCategories;

public class GetCategoriesQueryHandler(IExternalApiClient httpClient, IOptions<RestApiSettings> restApiSettings, ILogger<GetCategoriesQueryHandler> logger) : IRequestHandler<GetCategoriesQuery, IReadOnlyCollection<Category>>
{
    private readonly RestApiSettings _restApiSettings = restApiSettings.Value;
    
    public async Task<IReadOnlyCollection<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.Get(_restApiSettings.Categories, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            var res = JsonSerializer.Deserialize<List<Category>>(content);

            return res.AsReadOnly();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching categories from API");
            throw;
        }
    }
}