using Eutonies.DNS;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSystemd();
builder.Logging.AddLog4Net(log4NetConfigFile: "log4net.config");
builder
   .AddConfiguration()
   .AddHttpClients()
   .AddServices();

var app = builder.Build();

app.Run();
