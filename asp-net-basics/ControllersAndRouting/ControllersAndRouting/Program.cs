var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();


app.MapGet("/", () => "Products API is running. Use /api/products to access products.");


app.MapControllers();

app.Run();