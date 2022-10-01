using AmbulanceICal.Configuration;
using AmbulanceICal.Services;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://*:5000/");

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services
    .Configure<SpreadSheetOptions>(builder.Configuration.GetSection("SpreadSheetOptions"));

// Add services to the container.
builder.Services
    .AddTransient<ISchemaService, SchemaService>()
    .AddTransient<IICsService, IcsService>();

builder.Services
    .AddHttpClient<IGoogleSpreadSheetService, GoogleSpreadSheetService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

