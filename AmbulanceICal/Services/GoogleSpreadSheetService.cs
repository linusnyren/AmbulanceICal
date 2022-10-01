using System;
using AmbulanceICal.Configuration;
using Microsoft.Extensions.Options;

namespace AmbulanceICal.Services
{
    public interface IGoogleSpreadSheetService
    {
        Task<string> GetSpreadSheetAsync();
    }
    public class GoogleSpreadSheetService : IGoogleSpreadSheetService
	{
        private readonly HttpClient httpClient;
        private readonly IOptions<SpreadSheetOptions> options;

        public GoogleSpreadSheetService(HttpClient httpClient, IOptions<SpreadSheetOptions> options)
		{
            this.httpClient = httpClient;
            this.options = options;
            this.httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
        }

        public async Task<string> GetSpreadSheetAsync()
        {
            var uri = $"tq?tqx=out:json&sheet={options.Value.SpreadSheetName}";
            var res = await httpClient.GetAsync(uri);
            res.EnsureSuccessStatusCode();

            return await res.Content.ReadAsStringAsync();
        }
    }
}

