using AmbulanceICal.Services;
using Microsoft.AspNetCore.Mvc;

namespace AmbulanceICal.Controllers;

[ApiController]
[Route("[controller]")]
public class SchemaController : ControllerBase
{

    private readonly ILogger<SchemaController> _logger;
    private readonly ISchemaService schemaService;

    public SchemaController(ILogger<SchemaController> logger, ISchemaService schemaService)
    {
        _logger = logger;
        this.schemaService = schemaService;
    }

    [HttpGet("{vehicle}/{team}")]
    public async Task<string> Get(string vehicle, string team)
    {
        return await schemaService.GetSchemaAsync(team, vehicle);
    }
}

