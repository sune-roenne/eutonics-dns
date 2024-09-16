namespace Eutonies.DNS.Configuration;

public class DnsConfiguration 
{
    public CloudflareConfiguration Cloudflare {get; set;}

    public int SecondsBetweenUpdate {get; set;} = 300;

}