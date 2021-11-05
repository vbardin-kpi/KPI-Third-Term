using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace LabThree.Api
{
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(wb =>
                {
                    wb.UseStartup<Startup>();
                });
    }
}
