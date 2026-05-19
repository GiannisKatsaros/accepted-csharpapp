using CSharpApp.Application.Products.Queries.GetProducts;

namespace CSharpApp.Tests.Products;

public class GetProductsQueryHandlerTests
{
    private readonly IExternalApiClient _apiClient = Substitute.For<IExternalApiClient>();
    private readonly IOptions<RestApiSettings> _settings = Options.Create(new RestApiSettings { Products = "products" });

    private GetProductsQueryHandler CreateHandler() =>
        new(_apiClient, _settings, NullLogger<GetProductsQueryHandler>.Instance);

    private static HttpResponseMessage Ok(string json) =>
        new(HttpStatusCode.OK) { Content = new StringContent(json, Encoding.UTF8, "application/json") };

    [Fact]
    public async Task Handle_ReturnsProducts_WhenApiSucceeds()
    {
        var json = JsonSerializer.Serialize(new[]
        {
            new Product { Id = 1, Title = "Widget" },
            new Product { Id = 2, Title = "Gadget" }
        });
        _apiClient.Get(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(Ok(json));

        var result = await CreateHandler().Handle(new GetProductsQuery(), CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Equal(1, result.First().Id);
    }

    [Fact]
    public async Task Handle_ReturnsEmpty_WhenApiReturnsEmptyArray()
    {
        _apiClient.Get(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(Ok("[]"));

        var result = await CreateHandler().Handle(new GetProductsQuery(), CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_Throws_WhenApiReturnsError()
    {
        _apiClient.Get(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new HttpResponseMessage(HttpStatusCode.InternalServerError));

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            CreateHandler().Handle(new GetProductsQuery(), CancellationToken.None));
    }
}