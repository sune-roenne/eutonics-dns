namespace Eutonies.DNS.Services;
public interface IPublicIpAddressFinder {
    Task<string> FindPublicIp();
}