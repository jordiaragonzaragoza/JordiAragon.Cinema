﻿namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.User.Domain;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Seeds the data base. This file should only be used in the development environment.
    /// </summary>
    public static class SeedData
    {
        public static readonly Movie ExampleMovie =
            Movie.Add(
                id: MovieId.Create(new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6")),
                title: "Inception",
                runtime: Runtime.Create(TimeSpan.FromHours(2) + TimeSpan.FromMinutes(28)),
                exhibitionPeriod: ExhibitionPeriod.Create(
                    StartingPeriod.Create(new DateTimeOffset(DateTimeOffset.UtcNow.AddYears(1).Year, 1, 1, 1, 1, 1, TimeSpan.Zero)),
                    EndOfPeriod.Create(DateTimeOffset.UtcNow.AddYears(2)),
                    Runtime.Create(TimeSpan.FromHours(2) + TimeSpan.FromMinutes(28))));

        public static readonly Auditorium ExampleAuditorium =
            Auditorium.Create(
                id: AuditoriumId.Create(new Guid("c91aa0e0-9bc0-4db3-805c-23e3d8eabf53")),
                name: "Auditorium One",
                rows: Rows.Create(10),
                seatsPerRow: SeatsPerRow.Create(10));

        public static readonly User ExampleUser =
            User.Create(
                id: UserId.Create(new Guid("08ffddf5-3826-483f-a806-b3144477c7e8")));

        public static void Initialize(WebApplication app, bool isDevelopment)
        {
            if (!isDevelopment)
            {
                return;
            }

            Guard.Against.Null(app, nameof(app));

            using var writeScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var readScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

            using var writeContext = writeScope.ServiceProvider.GetRequiredService<ReservationBusinessModelContext>();
            using var readContext = readScope.ServiceProvider.GetRequiredService<ReservationReadModelContext>();

            try
            {
                PopulateBusinessModelTestData(writeContext);
                PopulateReadModelTestData(readContext);
            }
            catch (Exception exception)
            {
                app.Logger.LogError(exception, "An error occurred seeding the database with test data. Error: {ExceptionMessage}", exception.Message);

                throw;
            }
        }

        private static void PopulateBusinessModelTestData(ReservationBusinessModelContext context)
        {
            if (HasAnyData(context))
            {
                return;
            }

            context.Movies.Add(ExampleMovie);

            context.Auditoriums.Add(ExampleAuditorium);

            context.Users.Add(ExampleUser);

            context.SaveChanges();
        }

        private static void PopulateReadModelTestData(ReservationReadModelContext context)
        {
            // Intentionally empty. Will be used to populate test data on read model.
        }

        private static bool HasAnyData(DbContext context)
        {
            var dbSets = context.GetType().GetProperties()
                                           .Where(p => p.PropertyType.IsGenericType
                                                    && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

            foreach (var dbSetProperty in dbSets)
            {
                var dbSet = dbSetProperty.GetValue(context) as IEnumerable<object>;

                if (dbSet != null && dbSet.Any())
                {
                    return true;
                }
            }

            return false;
        }
    }
}