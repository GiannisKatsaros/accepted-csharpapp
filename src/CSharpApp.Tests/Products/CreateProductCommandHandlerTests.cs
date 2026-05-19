using CSharpApp.Application.Products.Commands.CreateProduct;

namespace CSharpApp.Tests.Products;

public class CreateProductCommandHandlerTests
{
    private readonly IExternalApiClient _apiClient = Substitute.For<IExternalApiClient>();
    private readonly IOptions<RestApiSettings> _settings = Options.Create(new RestApiSettings { Products = "products" });

    private CreateProductCommandHandler CreateHandler() =>
        new(_apiClient, _settings, NullLogger<CreateProductCommandHandler>.Instance);

    private static HttpResponseMessage Ok(string json) =>
        new(HttpStatusCode.OK) { Content = new StringContent(json, Encoding.UTF8, "application/json") };

    private static CreateProductCommand ValidCommand() => new()
    {
        Title = "Vibrant Pink Classic Sneakers",
        Price = 84,
        Description = "Step into style with our Vibrant Pink Classic Sneakers! These eye-catching shoes feature a bold pink hue with iconic white detailing, offering a sleek, timeless design. Constructed with durable materials and a comfortable fit, they are perfect for those seeking a pop of color in their everyday footwear. Grab a pair today and add some vibrancy to your step!",
        CategoryId = 4,
        Images = [
            "https://i.imgur.com/mcW42Gi.jpeg",
            "https://i.imgur.com/mhn7qsF.jpeg",
            "https://i.imgur.com/F8vhnFJ.jpeg"]
    };

    [Fact]
    public async Task Handle_ReturnsProduct_WhenApiSucceeds()
    {
        var json = JsonSerializer.Serialize(new Product { Id = 1, Title = "Vibrant Pink Classic Sneakers" });
        _apiClient.Post(Arg.Any<string>(), Arg.Any<object>(), Arg.Any<CancellationToken>()).Returns(Ok(json));

        var result = await CreateHandler().Handle(ValidCommand(), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Vibrant Pink Classic Sneakers", result.Title);
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