namespace Eutonies.DNS.Configuration;

public class CloudflareConfiguration 
{
    public string BaseUrl {get; set;}
    public string ZoneId {get; set;}
    public string AccountId {get; set;}
    public string ClientKey {get; set;}
    public string EditZoneDNSToken { get; set; }
}