using Artificial.Scrum.Master.TokenRefresher;
using Artificial.Scrum.Master.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

var sqlConnectionString = builder.Configuration.GetConnectionString("MS-SQL")
    ?? throw new ArgumentNullException("MS-SQL");

var taigaAddress = builder.Configuration["ScrumManagementService:BaseUrl"]
    ?? throw new ArgumentNullException("ScrumManagementService:BaseUrl");

builder.Logging.AddApplicationInsights();

builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<ITokenRefresherService, TokenRefresherService>(client =>
{
    client.BaseAddress = new Uri(taigaAddress);
});
builder.Services.AddHostedService<TokenRefresherHostedService>();
builder.Services.AddInfrastructure(sqlConnectionString, InfrastructureType.WorkerService);

builder.Services.AddApplicationInsightsTelemetryWorkerService();

var host = builder.Build();
await host.StartAsync();
await host.WaitForShutdownAsync();
