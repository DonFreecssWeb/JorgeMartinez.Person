using CRUDPersonsAPI.Datos;
using CRUDPersonsAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSQL"));
});

builder.Services.AddScoped<PersonService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "seguro", builder =>
    {
        builder.SetIsOriginAllowed( origin => new Uri(origin).Host == "127.0.0.1")
        .AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();

app.UseCors("seguro");

// Configure the HTTP request pipeline.

app.MapGet("/", (PersonService service) =>
{    

    return Results.Json(new {data = service.GetAllPersons() });
});



app.Run();
 