namespace Eutonies.DNS.Services;
public interface ICloudflareDnsService 
{
    Task UpdateDnsSettings(string publicIp);

}