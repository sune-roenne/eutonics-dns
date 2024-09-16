
using Eutonies.DNS.Services;

internal class DnsRecordUpdater : IDnsRecordUpdater
{
    private DateTime? _lastUpdate; 
    private readonly IServiceScopeFactory _scopeFactory;

    public DnsRecordUpdater(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public DateTime? LastUpdate => _lastUpdate;

    public async Task UpdateIpRecord()
    {
        using var scope = _scopeFactory.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DnsRecordUpdater>>();
        var ipFinder = scope.ServiceProvider.GetRequiredService<IPublicIpAddressFinder>();
        var publicIp = await ipFinder.FindPublicIp();
        logger.LogInformation($"Found IP: {publicIp}");
        var updater = scope.ServiceProvider.GetRequiredService<ICloudflareDnsService>();
        await updater.UpdateDnsSettings(publicIp);
        logger.LogInformation($"Finished updating of DNS record");

    }
}