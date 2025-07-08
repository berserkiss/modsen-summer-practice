using Microsoft.EntityFrameworkCore;
using EntityFramework.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();
app.Run();