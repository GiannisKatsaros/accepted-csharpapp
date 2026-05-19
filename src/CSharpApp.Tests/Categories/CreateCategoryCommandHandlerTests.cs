using CSharpApp.Application.Categories.Commands.CreateCategory;

namespace CSharpApp.Tests.Categories;

public class CreateCategoryCommandHandlerTests
{
    private readonly IExternalApiClient _apiClient = Substitute.For<IExternalApiClient>();
    private readonly IOptions<RestApiSettings> _settings = Options.Create(new RestApiSettings { Categories = "categories" });

    private CreateCategoryCommandHandler CreateHandler() =>
        new(_apiClient, _settings, NullLogger<CreateCategoryCommandHandler>.Instance);

    private static HttpResponseMessage Ok(string json) =>
        new(HttpStatusCode.OK) { Content = new StringContent(json, Encoding.UTF8, "application/json") };

    private static CreateCategoryCommand ValidCommand() => new()
    {
        Name = "Books",
        Image = "https://api.lorem.space/image/book?w=150&h=220"
    };

    [Fact]
    public async Task Handle_ReturnsCategory_WhenApiSucceeds()
    {
        var json = JsonSerializer.Serialize(new Category { Id = 1, Name = "Books" });
        _apiClient.Post(Arg.Any<string>(), Arg.Any<object>(), Arg.Any<CancellationToken>()).Returns(Ok(json));

        var result = await CreateHandler().Handle(ValidCommand(), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Books", result.Name);
    }

    [Fact]
    public async Task Handle_Throws_WhenApiReturnsError()
    {
        _apiClient.Post(Arg.Any<string>(), Arg.Any<object>(), Arg.Any<CancellationToken>())
            .Returns(new HttpResponseMessage(HttpStatusCode.InternalServerError));

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            CreateHandler().Handle(ValidCommand(), CancellationToken.None));
    }
}