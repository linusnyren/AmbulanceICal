using System;
using AmbulanceICal.Configuration;
using Microsoft.Extensions.Options;

namespace AmbulanceICal.Services
{
    public interface IGoogleSpreadSheetService
    {
        Task<string> GetSpreadSheetAsync(string sheet);
    }
    public class GoogleSpreadSheetService : IGoogleSpreadSheetService
	{
        private readonly HttpClient httpClient;

        public GoogleSpreadSheetService(HttpClient httpClient, IOptions<SpreadSheetOptions> options)
		{
            this.httpClient = httpClient;
            this.httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
        }

        public async Task<string> GetSpreadSheetAsync(string sheet)
        {
            var uri = $"tq?tqx=out:json&gid={sheet}";
            var res = await httpClient.GetAsync(uri);
            res.EnsureSuccessStatusCode();

            return await res.Content.ReadAsStringAsync();
        }
    }
}

