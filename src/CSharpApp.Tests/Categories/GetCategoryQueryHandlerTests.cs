using CSharpApp.Application.Categories.Queries.GetCategory;

namespace CSharpApp.Tests.Categories;

public class GetCategoryQueryHandlerTests
{
    private readonly IExternalApiClient _apiClient = Substitute.For<IExternalApiClient>();
    private readonly IOptions<RestApiSettings> _settings = Options.Create(new RestApiSettings { Categories = "categories" });

    private GetCategoryQueryHandler CreateHandler() =>
        new(_apiClient, _settings, NullLogger<GetCategoryQueryHandler>.Instance);

    private static HttpResponseMessage Ok(string json) =>
        new(HttpStatusCode.OK) { Content = new StringContent(json, Encoding.UTF8, "application/json") };

    [Fact]
    public async Task Handle_ReturnsCategory_WhenIdExists()
    {
        var json = JsonSerializer.Serialize(new Category { Id = 5, Name = "Books" });
        _apiClient.Get(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(Ok(json));

        var result = await CreateHandler().Handle(new GetCategoryQuery { Id = 5 }, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(5, result.Id);
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenApiReturnsBadRequest()
    {
        _apiClient.Get(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new HttpResponseMessage(HttpStatusCode.BadRequest));

        var result = await CreateHandler().Handle(new GetCategoryQuery { Id = 99 }, CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_Throws_WhenApiReturnsServerError()
    {
        _apiClient.Get(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new HttpResponseMessage(HttpStatusCode.InternalServerError));

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            CreateHandler().Handle(new GetCategoryQuery { Id = 1 }, CancellationToken.None));
    }
}