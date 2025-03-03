using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using WeatherAPI.Services;

var builder = WebApplication.CreateBuilder(args);

var neo4jConfig = builder.Configuration.GetSection("Neo4j");
builder.Services.AddSingleton(new Neo4jService(
    neo4jConfig["Uri"],
    neo4jConfig["Username"],
    neo4jConfig["Password"]
));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
