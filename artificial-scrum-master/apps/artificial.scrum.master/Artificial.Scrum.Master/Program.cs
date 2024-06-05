using Artificial.Scrum.Master.EditTextSuggestions;
using Artificial.Scrum.Master.EstimationPoker;
using Artificial.Scrum.Master.Infrastructure;
using Artificial.Scrum.Master.ScrumProjectIntegration;
using Artificial.Scrum.Master.UserSettings;
using Artificial.Scrum.Master.User;
using Artificial.Scrum.Master.Retrospectives;
using Artificial.Scrum.Master.SharedKernel;
using Artificial.Scrum.Master.TaskGeneration;
using Artificial.Scrum.Master.Prioritization;

var builder = WebApplication.CreateBuilder(args);

var sqlConnectionString = builder.Configuration.GetConnectionString("MS-SQL")
                          ?? throw new ArgumentNullException("MS-SQL");

builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddInfrastructure(sqlConnectionString);
builder.Services.AddUserSettingsModule();
builder.Services.AddScrumIntegrationModule(
    builder.Configuration.GetSection("ScrumManagementService"));
builder.Services.AddEstimationPokerModule();
builder.Services.AddEditSuggestionsModule();
builder.Services.AddPrioritySuggestionsModule();
builder.Services.AddUserModule(builder.Configuration);
builder.Services.AddRetrospectives();
builder.Services.AddSharedKernel();
builder.Services.AddTaskGenerationModule();

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
app.UseScrumProjectIntegration();
app.UseEditSuggestionsModule();
app.UsePrioritySuggestionsModule();

app.RegisterUserSettingsEndpoints();
app.RegisterEstimationPokerEndpoints();
app.RegisterScrumIntegrationEndpoints();
app.RegisterEditSuggestionEndpoints();
app.RegisterPrioritySuggestionsEndpoints();
app.RegisterUserEndpoints();
app.RegisterRetrospectivesEndpoints();
app.RegisterTaskGenerationEndpoints();
app.MapControllerRoute(name: "default", "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
