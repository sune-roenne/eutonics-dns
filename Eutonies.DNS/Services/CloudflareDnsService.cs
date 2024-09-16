using System.Text.Json;
using System.Text.Json.Serialization;
using Eutonies.DNS.Configuration;
using Microsoft.Extensions.Options;

namespace Eutonies.DNS.Services;
internal class CloudflareDnsService : ICloudflareDnsService
{

    private readonly HttpClient _client;
    private readonly ILogger<CloudflareDnsService> _logger;
    private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions {
        DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true
   };

    public CloudflareDnsService(HttpClient client, ILogger<CloudflareDnsService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task UpdateDnsSettings(string publicIp)
    {
        var result = await _client.GetAsync("dns_records");
        result.EnsureSuccessStatusCode();
        var jsonString = await result.Content.ReadAsStringAsync();
        var parsedResult = JsonSerializer.Deserialize<DnsListResult>(jsonString, _serializerOptions)!;
        var currentRecord = parsedResult.Result
           .Where(_ => _.Type == "A")
           .First()!;
        var currentIp = currentRecord.Content.Trim();
        if(currentIp != publicIp) 
        {
            var patchResult = await _client.PatchAsJsonAsync($"dns_records/{currentRecord.Id}",new DnsRecordPatchDocument(publicIp));
          _logger.LogInformation($"Updating IP from: {currentIp} to {publicIp} finished with status code: {patchResult.StatusCode}");

        }
        else {
          _logger.LogInformation("IP Address unchanged... Will do nothing");
        }

    }
    
    private record DnsListResult(
      DnsListRecord[] Result  

    );

    private record DnsListRecord(
        string Id,
        string? Comment,
        string? Name,
        long? Ttl,
        string Content,
        string? Type
    );

    private record DnsRecordPatchDocument(
        string Content
    );


}// 91.101.115.117

/*
{

  "result": [
    {
      "comment": "Domain verification record",

      "name": "example.com",

      "proxied": true,

      "settings": {},

      "tags": [],

      "ttl": 3600,

      "content": "198.51.100.4",

      "type": "A",

      "comment_modified_on": "2024-01-01T05:20:00.12345Z",

      "created_on": "2014-01-01T05:20:00.12345Z",

      "id": "023e105f4ecef8ad9ca31a8372d0c353",

      "meta": {},

      "modified_on": "2014-01-01T05:20:00.12345Z",

      "proxiable": true,

      "tags_modified_on": "2025-01-01T05:20:00.12345Z"

    }

  ]

}


*/

