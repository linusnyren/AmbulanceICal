using System;
using System.Text.Json;
using AmbulanceICal.Mappers;
using AmbulanceICal.Models;

namespace AmbulanceICal.Services
{
    public interface ISchemaService
    {
        Task<string> GetSchemaAsync(string sheet, string team, string vehicle);
    }

    public class SchemaService : ISchemaService
	{
        private readonly IGoogleSpreadSheetService spreadSheetService;
        private readonly IICsService icsService;
        private readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public SchemaService(IGoogleSpreadSheetService spreadSheetService, IICsService icsService)
		{
            this.spreadSheetService = spreadSheetService;
            this.icsService = icsService;
        }

        public async Task<string> GetSchemaAsync(string sheet, string team, string vehicle)
        {
            var spreadSheet = await spreadSheetService.GetSpreadSheetAsync(sheet);
            var cleanedResponse = CleanResponse(spreadSheet);
            var model = JsonSerializer.Deserialize<GoogleSpreadSheetResponse>(cleanedResponse, serializerOptions);

            var schemaModels = SpreadSheetMapper.Map(model!, team, vehicle);
            schemaModels = FilterWeeks(schemaModels);

            return icsService.GenerateIcs(schemaModels);
        }

        private List<SchemaModel> FilterWeeks(List<SchemaModel> schemaModels)
        {
            var lastWeek = (DateTime.Now.DayOfYear / 7) - 1;
            return schemaModels.Where(x => x.Week >= lastWeek).ToList();
        }

        private string CleanResponse(string spreadSheet)
        {
            var valuesToReplace = new List<string>
            {
                "\n",
                "/*O_o*/",
                "google.visualization.Query.setResponse(",
                ");"
            };

            string cleaned = spreadSheet;
            foreach(var value in valuesToReplace)
            {
                cleaned = cleaned.Replace(value, "");
            }

            return cleaned;
        }
    }
}

