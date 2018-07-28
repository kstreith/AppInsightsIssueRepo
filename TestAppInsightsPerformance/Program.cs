using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace TestAppInsightsPerformance
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights();
            var host = builder.Build();
            host.Run();
        }            
    }
}
