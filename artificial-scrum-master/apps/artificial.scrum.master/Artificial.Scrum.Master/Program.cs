using Artificial.Scrum.Master.Infrastructure;
using Artificial.Scrum.Master.Middleware;
using Artificial.Scrum.Master.ScrumProjectIntegration;

var builder = WebApplication.CreateBuilder(args);

var sqlConnectionString = builder.Configuration.GetConnectionString("MS-SQL")
                          ?? throw new ArgumentNullException("MS-SQL");

builder.Services.AddControllers();
builder.Services.AddInfrastructure(sqlConnectionString);

// Scrum Project Service Integration
builder.Services.AddScrumProjectIntegration(
    builder.Configuration.GetSection("ScrumManagementService"));

// Middleware
builder.Services.AddScoped<ScrumProjectIntegrationMiddleware>();

var app = builder.Build();

// Middleware
app.UseMiddleware<ScrumProjectIntegrationMiddleware>();

app.MapGet("/", () => "Hello World!");

app.RegisterScrumProjectIntegrationEndpoints();

app.Run();
