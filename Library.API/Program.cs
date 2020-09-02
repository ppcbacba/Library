using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;


namespace Library.API
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug() //��С�������λ��Debug�����
                .Enrich.FromLogContext()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) //��Microsoftǰ׺����־����С�������ĳ�Information
                .Enrich.FromLogContext()
                .WriteTo
                .File(Path.Combine(@"logs/log.txt"), rollingInterval: RollingInterval.Day)//����־�����Ŀ��·�����ļ������ɷ�ʽΪÿ������һ���ļ�
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                   webBuilder
                        .UseSerilog()
                        .UseStartup<Startup>();
                });
    }
}