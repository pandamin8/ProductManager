using Microsoft.OpenApi.Models;
using ProductManager.API.Document;
using ProductManager.API.Middlewares;
using Serilog;

namespace ProductManager.API.Extensions;

public static class WebApplicationBuilderExtension
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme()
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });
            
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme, Id = "bearerAuth"
                        },
                    },
                    []
                }
            });
            
            options.DocumentFilter<DocumentEndpointsFilter>();
        });

        builder.Services.AddScoped<ErrorHandlingMiddleware>();
        builder.Services.AddScoped<RequestTimeLoggingMiddleware>();
        
        builder.Host.UseSerilog(((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration)
            ));
    }
}
