using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using ProductManager.Application.Users;

namespace ProductManager.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;

        services.AddMediatR(options => options.RegisterServicesFromAssembly(applicationAssembly));

        services.AddLogging();
        
        services.AddAutoMapper(applicationAssembly);
        
        services.AddValidatorsFromAssembly(applicationAssembly)
            .AddFluentValidationAutoValidation();
        
        services.AddFluentValidationRulesToSwagger();
        
        services.AddScoped<IUserContext, UserContext>();
        services.AddHttpContextAccessor();
    }
}
