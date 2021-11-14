using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LabThree.Data;

public static class DataLayerInstaller
{
    public static IServiceCollection InstallDataLayer(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseNpgsql(config["ConnectionStrings:DbConnection"]);
        });

        return services;
    }
}