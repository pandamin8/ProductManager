using ProductManager.API.Extensions;
using ProductManager.API.Middlewares;
using ProductManager.Application.Extensions;
using ProductManager.Domain.Entities;
using ProductManager.Infrastructure.Extensions;
using ProductManager.Infrastructure.Seeders;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();
var scope = app.Services.CreateScope();

var seeder = scope.ServiceProvider.GetRequiredService<IProductManagerSeeder>();
await seeder.Seed();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGroup("/identity").WithTags("Identity").MapIdentityApi<User>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();