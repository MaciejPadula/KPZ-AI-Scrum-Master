using Artificial.Scrum.Master.Infrastructure;
using Artificial.Scrum.Master.UserSettings;

var builder = WebApplication.CreateBuilder(args);

var sqlConnectionString = builder.Configuration.GetConnectionString("MS-SQL")
  ?? throw new ArgumentNullException("MS-SQL");

builder.Services.AddControllers();
builder.Services.AddInfrastructure(sqlConnectionString);
builder.Services.AddUserSettings();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.RegisterUserSettingsEndpoints();

app.Run();
