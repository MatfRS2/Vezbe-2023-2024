using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Behaviors;

namespace Ordering.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
       // https://learn.microsoft.com/en-us/dotnet/standard/assembly/
       services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
       
       services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
       services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
       return services;
    }
}