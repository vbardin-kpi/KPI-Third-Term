using LabThree.Api.Dal;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

namespace LabThree.Api
{
    internal class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(opt => { opt.SerializerSettings.Formatting = Formatting.Indented; });

            services
                .AddDbContext<ApiDbContext>(opt =>
                {
                    opt.UseNpgsql(Configuration["ConnectionStrings:DbConnection"]);
                })
                .AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder appBuilder, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                appBuilder.UseDeveloperExceptionPage();
                appBuilder.UseSwagger();
                appBuilder.UseSwaggerUI(conf => { conf.SwaggerEndpoint("/swagger/v1/swagger.json", "Lab Three API"); });
            }

            appBuilder.UseHttpsRedirection()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(enp => enp.MapControllers());
        }
    }
}