using Eutonies.DNS.Configuration;
using Eutonies.DNS.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Eutonies.DNS;

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
            .ConfigurePrimaryHttpMessageHandler(ConfigureCloudflarePrimaryMessageHandler);
        builder.Services.AddHttpClient<IPublicIpAddressFinder, PublicIpAddressFinder>();    
        return builder;
    }

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder) 
    {
        builder.Services.AddSingleton<IDnsRecordUpdater, DnsRecordUpdater>();
        builder.Services.AddHostedService<DnsUpdaterBackgroundWorker>();
        return builder;
    }



    private static void ConfigureCloudflareClient(IServiceProvider services, HttpClient client)
    {
        var conf = services.GetRequiredService<IOptions<DnsConfiguration>>().Value;
        client.BaseAddress = new System.Uri($"{conf.Cloudflare.BaseUrl}/{conf.Cloudflare.ZoneId}/");
        //client.BaseAddress = new System.Uri($"{conf.Cloudflare.BaseUrl}");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {conf.Cloudflare.EditZoneDNSToken}");
        //client.DefaultRequestHeaders.Add("X-Auth-Key", $"{conf.Cloudflare.ClientKey}");
        //client.DefaultRequestHeaders.Add("ContentType", $"application/json");
    }

    private static void ConfigureCloudflarePrimaryMessageHandler(HttpMessageHandler handler, IServiceProvider services)
    {
    }


}