namespace JordiAragon.Cinema.Reservation.Common.Presentation.HttpRestfulApi
{
    using System.Globalization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using Serilog.Sinks.Graylog;
    using Serilog.Sinks.Graylog.Core.Transport;

    public static class ConfigureHostBuilder
    {
        public static IHostBuilder AddHostBuilderConfigurations(this IHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog(ConfigureSerilog);
            /*hostBuilder.UseDefaultServiceProvider(options =>
            {
                options.ValidateOnBuild = true;
            });*/

            return hostBuilder;
        }

        private static void ConfigureSerilog(
            HostBuilderContext context,
            LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentName()
                .Enrich.WithMachineName()
                .Enrich.WithThreadName()
                .Enrich.WithThreadId()
                .AddConsoleLogger(context.Configuration)
                .AddGraylogLogger(context.Configuration);
        }

        private static LoggerConfiguration AddConsoleLogger(
           this LoggerConfiguration loggerConfiguration,
           IConfiguration configuration)
        {
            var serilogConsoleOptions = new SerilogConsoleOptions();
            configuration.GetSection(SerilogConsoleOptions.Section).Bind(serilogConsoleOptions);

            if (serilogConsoleOptions.Enabled)
            {
                loggerConfiguration.WriteTo.Async(loggerSinkConfiguration =>
                loggerSinkConfiguration.Console(
                    restrictedToMinimumLevel: serilogConsoleOptions.MinimumLevel,
                    formatProvider: CultureInfo.InvariantCulture))
                    .Destructure.ToMaximumStringLength(int.MaxValue);
            }

            return loggerConfiguration;
        }

        private static LoggerConfiguration AddGraylogLogger(
           this LoggerConfiguration loggerConfiguration,
           IConfiguration configuration)
        {
            var serilogGraylogOptions = new SerilogGraylogOptions();
            configuration.GetSection(SerilogGraylogOptions.Section).Bind(serilogGraylogOptions);

            if (serilogGraylogOptions.Enabled)
            {
                loggerConfiguration.WriteTo.Async(loggerSinkConfiguration => loggerSinkConfiguration.Graylog(
                    hostnameOrAddress: serilogGraylogOptions.Host,
                    port: serilogGraylogOptions.Port,
                    transportType: TransportType.Udp,
                    minimumLogEventLevel: serilogGraylogOptions.MinimumLevel)).Destructure.ToMaximumStringLength(int.MaxValue);
            }

            return loggerConfiguration;
        }
    }
}