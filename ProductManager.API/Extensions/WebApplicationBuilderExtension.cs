using Serilog;

namespace ProductManager.API.Extensions;

public static class WebApplicationBuilderExtension
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Host.UseSerilog(((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration)
            ));
    }
}
