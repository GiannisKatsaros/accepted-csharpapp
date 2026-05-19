using CSharpApp.Application.Products.Queries.GetProduct;

namespace CSharpApp.Tests.Products;

public class GetProductQueryHandlerTests
{
    private readonly IExternalApiClient _apiClient = Substitute.For<IExternalApiClient>();
    private readonly IOptions<RestApiSettings> _settings = Options.Create(new RestApiSettings { Products = "products" });

    private GetProductQueryHandler CreateHandler() =>
        new(_apiClient, _settings, NullLogger<GetProductQueryHandler>.Instance);

    private static HttpResponseMessage Ok(string json) =>
        new(HttpStatusCode.OK) { Content = new StringContent(json, Encoding.UTF8, "application/json") };

    [Fact]
    public async Task Handle_ReturnsProduct_WhenIdExists()
    {
        var json = JsonSerializer.Serialize(new Product { Id = 42, Title = "Widget" });
        _apiClient.Get(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(Ok(json));

        var result = await CreateHandler().Handle(new GetProductQuery { Id = 42 }, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(42, result.Id);
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenApiReturnsBadRequest()
    {
        _apiClient.Get(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new HttpResponseMessage(HttpStatusCode.BadRequest));

        var result = await CreateHandler().Handle(new GetProductQuery { Id = 99 }, CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_Throws_WhenApiReturnsServerError()
    {
        _apiClient.Get(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new HttpResponseMessage(HttpStatusCode.InternalServerError));

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            CreateHandler().Handle(new GetProductQuery { Id = 1 }, CancellationToken.None));
    }
}