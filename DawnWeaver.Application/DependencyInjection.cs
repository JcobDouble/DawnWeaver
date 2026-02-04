using System.Reflection;
using DawnWeaver.Application.Common.Behaviours;
using DawnWeaver.Application.Services;
using DawnWeaver.Application.Services.TimeManagement;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace DawnWeaver.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        services.AddScoped<GetAllEventsService>();
        services.AddScoped<TimeProposalService>();
        services.AddScoped<CollisionService>();
        
        services.AddTransient(typeof(IRequestPreProcessor<>), typeof(LoggingBehaviour<>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        
        return services;
    }
}