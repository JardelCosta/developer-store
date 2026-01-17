using Api.Extensions;
using Application;
using Persistence;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGenWithAuth();
builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices();
builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

var app = builder.Build();

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Developer Sales API");
    });
}

app.UseHttpsRedirection();

app.UseCustomExceptionHandler();
app.UseHttpsRedirection();

app.Run();
