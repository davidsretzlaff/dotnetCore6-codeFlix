using CodeFlix.Catalog.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddUseCases()
    .AddAndConfigureControllers();

var app = builder.Build();
app.UseDocumentation();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }