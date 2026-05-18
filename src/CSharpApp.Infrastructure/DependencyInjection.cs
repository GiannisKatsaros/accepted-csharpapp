using CSharpApp.Application.Interfaces;
using CSharpApp.Infrastructure.Auth;
using CSharpApp.Infrastructure.HttpClients;

namespace CSharpApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<TokenStorage>();
        services.AddScoped<AuthService>();
        services.AddHttpClient<EscuelajsHttpClient>();
        services.AddScoped<IExternalApiClient, EscuelajsHttpClient>();
        
        return services;
    }
}