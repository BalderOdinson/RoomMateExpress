using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace RoomMateExpressWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .UseSerilog((context, logger) =>
                {
                    var cnnstr = context.Configuration.GetConnectionString("DefaultConnection");

                    logger.MinimumLevel.Error()
                        .Enrich.FromLogContext()
                        .WriteTo.MSSqlServer(
                            connectionString: cnnstr,
                            tableName: "Errors",
                            autoCreateSqlTable: true);
                })
                .Build()
                .Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder()
                .ConfigureAppConfiguration((ctx, cfg) =>
                {
                    cfg.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("config.json", true)
                        .AddEnvironmentVariables();
                })
                .ConfigureLogging((ctx, logging) => { })
                .UseStartup<Startup>()
                .UseSetting("DesignTime", "true")
                .Build();
        }
    }
}
