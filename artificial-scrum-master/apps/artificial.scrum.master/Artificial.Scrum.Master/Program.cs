using Artificial.Scrum.Master.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var sqlConnectionString = builder.Configuration.GetConnectionString("MS-SQL")
  ?? throw new ArgumentNullException("MS-SQL");

builder.Services.AddControllers();
builder.Services.AddInfrastructure(sqlConnectionString);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
