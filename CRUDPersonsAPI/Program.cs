using CRUDPersonsAPI.databasecontext;
using CRUDPersonsAPI.Filters;
using CRUDPersonsAPI.Services;
using Microsoft.AspNetCore.Builder;
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

//swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<VerifySession2>>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new VerifySession2(logger,""));
});

builder.Services.AddTransient<JwtService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseCors("seguro");

// Configure the HTTP request pipeline.

 


app.Run();
 