using CSharpApp.Application.Interfaces;
using CSharpApp.Infrastructure.Auth;
using CSharpApp.Infrastructure.HttpClients;
using Polly;

namespace CSharpApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection("HttpClientSettings").Get<HttpClientSettings>()!;
        services.ConfigureHttpClientDefaults(builder =>
            builder.AddTransientHttpErrorPolicy(p =>
                p.WaitAndRetryAsync(
                    settings.RetryCount,
                    _ => TimeSpan.FromMilliseconds(settings.SleepDuration))));
        
        services.AddSingleton<TokenStorage>();
        services.AddScoped<AuthService>();
        services.AddHttpClient<EscuelajsHttpClient>();
        services.AddScoped<IExternalApiClient, EscuelajsHttpClient>();
        
        return services;
    }
}