
internal class CloudflareDnsService : ICloudflareDnsService
{

    private readonly HttpClient _client;

    public CloudflareDnsService(HttpClient client)
    {
        _client = client;
    }

    public async Task UpdateDnsSettings(string publicIp)
    {
        _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        var result = await _client.GetAsync("dns_records");
        var tessa = "{https://api.cloudflare.com/client/v4/zones/2ec7cec85244ca284bd257036167bad9/dns_records}";
    }



}