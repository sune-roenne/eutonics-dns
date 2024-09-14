using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder
   .AddConfiguration()
   .AddHttpClients();

var app = builder.Build();

var cloudflareService = app.Services.GetRequiredService<ICloudflareDnsService>();
await cloudflareService.UpdateDnsSettings("");

app.Run();
