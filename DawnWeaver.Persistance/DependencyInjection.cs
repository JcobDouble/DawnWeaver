using DawnWeaver.Application.Common.Interfaces;
using DawnWeaver.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DawnWeaver.Persistance;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
                               configuration.GetConnectionString("AppDatabase");
        
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
            connectionString));
        
        services.AddScoped<IAppDbContext, AppDbContext>();
        
        return services;
    }
}