using MyFlix.Catalog.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAppConections()
    .AddUseCases()
    .AddAndConfigureControllers();

var app = builder.Build();
app.UseDocumentation();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
public partial class Program { }