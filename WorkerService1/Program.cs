using Serilog.Events;
using Serilog;

namespace WorkerService1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(@"D:\desk\testc\testserv\LogFile.txt")
                .CreateLogger();

            try
            {
                Log.Information("Starting up the service");
                CreateHostBuilder(args).Build().Run();
                return;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "There was a problem starting the service");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
            .UseWindowsService()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<Worker>();
            })
            .UseSerilog(); // ������ ������ ������� ��� ����� � ��������� �����
        }

        //        IHost host = Host.CreateDefaultBuilder(args)
        //    .UseWindowsService()
        //    .ConfigureServices(services =>
        //    {
        //        services.AddHostedService<Worker>();
        //    })

        //    .Build();

        //host.Run();
    }
}