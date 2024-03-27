using Artificial.Scrum.Master.Infrastructure;
using Artificial.Scrum.Master.ScrumProjectIntegration;

var builder = WebApplication.CreateBuilder(args);

var sqlConnectionString = builder.Configuration.GetConnectionString("MS-SQL")
                          ?? throw new ArgumentNullException("MS-SQL");

builder.Services.AddControllers();
builder.Services.AddInfrastructure(sqlConnectionString);

builder.Services.AddScrumProjectIntegration(
    builder.Configuration.GetSection("ScrumManagementService"));

var app = builder.Build();

app.UseScrumProjectIntegration();


app.MapGet("/", () => "Hello World!");

app.RegisterScrumProjectIntegrationEndpoints();

app.Run();
