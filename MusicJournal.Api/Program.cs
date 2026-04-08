using Microsoft.EntityFrameworkCore;
using MusicJournal.Api.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions( o => 
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlite("Data Source=musicjournal.db"));

builder.Services.AddCors(options =>
{
   options.AddPolicy("AllowFrontend", policy =>
   {
       var origins = builder.Configuration
       .GetSection("Cors:AllowedOrigins")
       .Get<string[]>() ?? [];

       policy.WithOrigins(origins)
            .AllowAnyHeader()
            .AllowAnyMethod();
   });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();
app.UseCors("AllowFrontend");
app.MapControllers();
app.Run();


