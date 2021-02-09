using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Serilog;
using log4net;
using log4net.Config;

namespace dotnet_log_benchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            // CreateHostBuilder(args).Build().Run();
            // CreateHostBuilderNlog(args).Build().Run();
            // CreateHostBuilderSerilog(args).Build().Run();
            CreateHostBuilderLog4Net(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        // .ConfigureLogging(logging =>
        // {
        //     logging.ClearProviders();
        //     logging.SetMinimumLevel(LogLevel.Debug);
        // })
        // .UseNLog();


        public static IHostBuilder CreateHostBuilderNlog(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Debug);
                })
                .UseNLog();

        public static IHostBuilder CreateHostBuilderSerilog(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .CaptureStartupErrors(true)
                        .ConfigureAppConfiguration(config =>
                        {
                            // Used for local settings like connection strings.
                            config.AddJsonFile("appsettings.Development.json", optional: true);
                        })
                        .UseSerilog((hostingContext, loggerConfiguration) =>
                        {
                            loggerConfiguration
                                .ReadFrom.Configuration(hostingContext.Configuration)
                                .Enrich.FromLogContext()
                                .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
                                .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment);
                        });
                });
        
        public static IHostBuilder CreateHostBuilderLog4Net(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseLog4Net();
        
    }
    
    public static class Log4NetExtensions
    {
        public static IHostBuilder UseLog4Net(this IHostBuilder hostBuilder)
        {
            var log4netRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(log4netRepository, new FileInfo("log4net.config"));

            return hostBuilder;
        }
    }
}
