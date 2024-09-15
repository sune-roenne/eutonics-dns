namespace Eutonies.DNS.Services;
internal class PublicIpAddressFinder : IPublicIpAddressFinder 
{
    private const string Url = "https://cloudflare.com/cdn-cgi/trace";

    private readonly HttpClient _client;

    public PublicIpAddressFinder(HttpClient client)
    {
        _client = client;
    }

    public async Task<string> FindPublicIp() 
    {
        var result = await (await _client.GetAsync(Url)).Content.ReadAsStringAsync();
        var lines = result.Split("\n")
           .Select(_ => _.Trim().ToLower())
           .ToList();
        var ipLine = lines
           .First(_ => _.StartsWith("ip"));
        var ip = ipLine.Split("=")[1].Trim();
        return ip;
    }


}