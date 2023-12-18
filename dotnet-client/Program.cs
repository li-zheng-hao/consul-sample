using Consul;
using ConsulDemo;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddConsul(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/ping", () => "pong")
    .WithOpenApi();

app.MapGet("/services", async (context ) =>
{
    var lists=await context.RequestServices.GetService<ConsulClient>()!.Health.Service("demo-service");
    await context.Response.WriteAsJsonAsync(lists);
}).WithOpenApi();;

app.Run();

