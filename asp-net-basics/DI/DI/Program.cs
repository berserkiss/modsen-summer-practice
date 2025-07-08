using Microsoft.Extensions.DependencyInjection.Extensions;
using DI;  

var builder = WebApplication.CreateBuilder(args);

builder.Services.TryAddTransient<ITransientGreetingService, GreetingService>();
builder.Services.TryAddScoped<IScopedGreetingService, GreetingService>();
builder.Services.TryAddSingleton<ISingletonGreetingService, GreetingService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();


app.UseAuthorization();

// для теста
app.MapGet("/greet/{name}", (
    string name,
    ITransientGreetingService transientService,
    IScopedGreetingService scopedService,
    ISingletonGreetingService singletonService) =>
{
    var result = new
    {
        Transient = transientService.Greet(name),
        Scoped = scopedService.Greet(name),
        Singleton = singletonService.Greet(name)
    };
    return Results.Ok(result);
});

app.MapControllers();
app.Run();