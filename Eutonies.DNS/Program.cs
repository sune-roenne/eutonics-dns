using Eutonies.DNS;
using Eutonies.DNS.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder
   .AddConfiguration()
   .AddHttpClients();

var app = builder.Build();

var ipFinder = app.Services.GetRequiredService<IPublicIpAddressFinder>();
var ip = await ipFinder.FindPublicIp();

var cloudflareService = app.Services.GetRequiredService<ICloudflareDnsService>();
await cloudflareService.UpdateDnsSettings(ip);

app.Run();
