using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

public static class DependencyInjection 
{

    public static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration
          .AddJsonFile("appsettings.json")
          .AddJsonFile("appsettings.local.json", optional : true);
        builder.Services.Configure<DnsConfiguration>(builder.Configuration.GetSection("Dns")); 
        return builder;
    }

    public static WebApplicationBuilder AddHttpClients(this WebApplicationBuilder builder) 
    {
        builder.Services.AddHttpClient<ICloudflareDnsService, CloudflareDnsService>(configureClient: ConfigureCloudflareClient)
            .ConfigurePrimaryHttpMessageHandler(ConfigureCloudflarePrimaryMessageHandler)
        ;
        return builder;
    }



    private static void ConfigureCloudflareClient(IServiceProvider services, HttpClient client)
    {
        var conf = services.GetRequiredService<IOptions<DnsConfiguration>>().Value;
        client.BaseAddress = new Uri($"{conf.Cloudflare.BaseUrl}/{conf.Cloudflare.ZoneId}/");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {conf.Cloudflare.ClientKey}");
        client.DefaultRequestHeaders.Add("X-Auth-Key", $"{conf.Cloudflare.ClientKey}");
        client.DefaultRequestHeaders.Add("ContentType", $"application/json");
    }

    private static void ConfigureCloudflarePrimaryMessageHandler(HttpMessageHandler handler, IServiceProvider services)
    {
    }


}