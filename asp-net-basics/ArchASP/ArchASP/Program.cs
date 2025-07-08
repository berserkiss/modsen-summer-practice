var builder = WebApplication.CreateBuilder(args);


if (builder.Environment.IsDevelopment())
{
    builder.Services.AddTransient<IService, DevelopmentService>();
    Console.WriteLine("Регистрация сервисов для Development");
}
else
{
    builder.Services.AddTransient<IService, ProductionService>();
    Console.WriteLine("Регистрация сервисов для Production");
}

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


app.Use(async (context, next) =>
{
    Console.WriteLine($"[LOG] {context.Request.Method} {context.Request.Path}");
    await next();
});


app.MapGet("/", () => "Добро пожаловать в мое ASP.NET приложение!");


app.MapGet("/appname", (IConfiguration config) => 
{
    var appName = config["AppName"];
    return $"App Name: {appName}";
});

app.Run();


public interface IService {}
public class DevelopmentService : IService {}
public class ProductionService : IService {}