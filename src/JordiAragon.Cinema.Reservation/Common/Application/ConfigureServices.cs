﻿namespace JordiAragon.Cinema.Reservation.Common.Application
{
    using JordiAragon.Cinema.Reservation.Showtime.Application.BackgroundJobs;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using FluentValidation;
    using Quartz;

    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddAutoMapper(AssemblyReference.Assembly);
            serviceCollection.AddValidatorsFromAssembly(AssemblyReference.Assembly, ServiceLifetime.Singleton);

            serviceCollection.AddQuartz(configure =>
            {
                var expireReservedSeatsJobKey = new JobKey(nameof(ExpireReservedSeatsJob));

                // This Bind is required because AddQuartz dont support IServiceProvider / option pattern.
                var expireReservedSeatsJobOptions = new ExpireReservedSeatsJobOptions();
                configuration.GetSection(ExpireReservedSeatsJobOptions.Section).Bind(expireReservedSeatsJobOptions);

                var expireReservedSeatsIntervalInSeconds = expireReservedSeatsJobOptions.ScheduleIntervalInSeconds;

                configure.AddJob<ExpireReservedSeatsJob>(expireReservedSeatsJobKey)
                .AddTrigger(trigger => trigger.ForJob(expireReservedSeatsJobKey)
                                              .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(expireReservedSeatsIntervalInSeconds)
                                                                                      .RepeatForever()));

                var endShowtimesJobKey = new JobKey(nameof(EndShowtimesJob));

                // This Bind is required because AddQuartz dont support IServiceProvider / option pattern.
                var endShowtimesJobOptions = new EndShowtimesJobOptions();
                configuration.GetSection(EndShowtimesJobOptions.Section).Bind(endShowtimesJobOptions);

                var endShowtimesIntervalInSeconds = endShowtimesJobOptions.ScheduleIntervalInSeconds;

                configure.AddJob<EndShowtimesJob>(endShowtimesJobKey)
                .AddTrigger(trigger => trigger.ForJob(endShowtimesJobKey)
                                              .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(endShowtimesIntervalInSeconds)
                                                                                      .RepeatForever()));
            });

            serviceCollection.AddQuartzHostedService(opt =>
            {
                opt.WaitForJobsToComplete = true;
            });

            return serviceCollection;
        }
    }
}