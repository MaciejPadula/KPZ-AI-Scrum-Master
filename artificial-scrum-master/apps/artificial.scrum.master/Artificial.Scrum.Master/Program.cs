using Artificial.Scrum.Master.Infrastructure;
using Artificial.Scrum.Master.ScrumProjectIntegration;
using Artificial.Scrum.Master.UserSettings;
using Artificial.Scrum.Master.User;

var builder = WebApplication.CreateBuilder(args);

var sqlConnectionString = builder.Configuration.GetConnectionString("MS-SQL")
  ?? throw new ArgumentNullException("MS-SQL");

builder.Services.AddControllers();
builder.Services.AddInfrastructure(sqlConnectionString);
builder.Services.AddUserSettingsModule();
builder.Services.AddScrumIntegrationModule(
    builder.Configuration.GetSection("ScrumManagementService"));

builder.Services.AddUserModule(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.RegisterUserSettingsEndpoints();
app.UseScrumProjectIntegration();
app.RegisterScrumIntegrationEndpoints();
app.RegisterUserEndpoints();
app.MapControllerRoute(name: "default", "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
