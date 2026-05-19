using CSharpApp.Application.Categories.Queries.GetCategories;

namespace CSharpApp.Tests.Categories;

public class GetCategoriesQueryHandlerTests
{
    private readonly IExternalApiClient _apiClient = Substitute.For<IExternalApiClient>();
    private readonly IOptions<RestApiSettings> _settings = Options.Create(new RestApiSettings { Categories = "categories" });

    private GetCategoriesQueryHandler CreateHandler() =>
        new(_apiClient, _settings, NullLogger<GetCategoriesQueryHandler>.Instance);

    private static HttpResponseMessage Ok(string json) =>
        new(HttpStatusCode.OK) { Content = new StringContent(json, Encoding.UTF8, "application/json") };

    [Fact]
    public async Task Handle_ReturnsCategories_WhenApiSucceeds()
    {
        var json = JsonSerializer.Serialize(new[]
        {
            new Category { Id = 1, Name = "Books" },
            new Category { Id = 2, Name = "Pencils" }
        });
        _apiClient.Get(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(Ok(json));

        var result = await CreateHandler().Handle(new GetCategoriesQuery(), CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Equal(1, result.First().Id);
    }

    [Fact]
    public async Task Handle_ReturnsEmpty_WhenApiReturnsEmptyArray()
    {
        _apiClient.Get(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(Ok("[]"));

        var result = await CreateHandler().Handle(new GetCategoriesQuery(), CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_Throws_WhenApiReturnsError()
    {
        _apiClient.Get(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new HttpResponseMessage(HttpStatusCode.InternalServerError));

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            CreateHandler().Handle(new GetCategoriesQuery(), CancellationToken.None));
    }
}