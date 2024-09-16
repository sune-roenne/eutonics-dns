
using Eutonies.DNS.Configuration;
using Microsoft.Extensions.Options;

public class DnsUpdaterBackgroundWorker : BackgroundService
{

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly DnsConfiguration _config;
    private readonly TimeSpan _waitTime;

    public DnsUpdaterBackgroundWorker(IServiceScopeFactory scopeFactory, IOptions<DnsConfiguration> conf)
    {
        _scopeFactory = scopeFactory;
        _config = conf.Value;
        _waitTime = TimeSpan.FromSeconds(_config.SecondsBetweenUpdate);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested) {
            using var scope = _scopeFactory.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<DnsUpdaterBackgroundWorker>>();
            try {
                var dnsUpdater = scope.ServiceProvider.GetRequiredService<IDnsRecordUpdater>();
                await dnsUpdater.UpdateIpRecord();
                await Task.Delay(_waitTime);
            }
            catch(Exception e) {
                logger.LogError(e, "During update of DNS records");
                await Task.Delay(_waitTime * 2);
            }

        }
    }
}