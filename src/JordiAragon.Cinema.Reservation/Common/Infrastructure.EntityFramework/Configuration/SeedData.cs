﻿namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public static class SeedData
    {
        public static readonly Movie ExampleMovie =
            Movie.Create(
                id: MovieId.Create(new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6")),
                title: "Inception",
                runtime: TimeSpan.FromHours(2) + TimeSpan.FromMinutes(28),
                exhibitionPeriod: ExhibitionPeriod.Create(
                    StartingPeriod.Create(new DateTimeOffset(2023, 1, 1, 1, 1, 1, TimeSpan.Zero)),
                    EndOfPeriod.Create(DateTime.UtcNow.AddYears(1)),
                    TimeSpan.FromHours(2) + TimeSpan.FromMinutes(28)));

        public static readonly Auditorium ExampleAuditorium =
            Auditorium.Create(
                id: AuditoriumId.Create(new Guid("c91aa0e0-9bc0-4db3-805c-23e3d8eabf53")),
                rows: 28,
                seatsPerRow: 22);

        public static readonly Showtime ExampleShowtime =
            Showtime.Create(
                id: ShowtimeId.Create(new Guid("89b073a7-cfcf-4f2a-b01b-4c7f71a0563b")),
                movieId: MovieId.Create(ExampleMovie.Id),
                sessionDateOnUtc: new DateTime(2023, 1, 1, 1, 1, 1, 1, DateTimeKind.Utc),
                auditoriumId: AuditoriumId.Create(ExampleAuditorium.Id));

        public static void Initialize(WebApplication app)
        {
            using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<ReservationContext>();
            try
            {
                PopulateTestData(context);
            }
            catch (Exception exception)
            {
                app.Logger.LogError(exception, "An error occurred seeding the database with test data. Error: {exceptionMessage}", exception.Message);
            }
        }

        public static void PopulateTestData(ReservationContext context)
        {
            if (context.Database.IsSqlServer())
            {
                context.Database.Migrate();
                context.Database.EnsureCreated();
            }

            if (HasAnyData(context))
            {
                return;
            }

            SetPreconfiguredData(context);
        }

        private static bool HasAnyData(ReservationContext context)
        {
            var dbSets = context.GetType().GetProperties()
                                           .Where(p => p.PropertyType.IsGenericType
                                                    && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

            foreach (var dbSetProperty in dbSets)
            {
                var dbSet = (IEnumerable<object>)dbSetProperty.GetValue(context);

                if (dbSet.Any())
                {
                    return true;
                }
            }

            return false;
        }

        private static void SetPreconfiguredData(ReservationContext context)
        {
            context.Movies.Add(ExampleMovie);

            context.Auditoriums.Add(ExampleAuditorium);

            context.Showtimes.Add(ExampleShowtime);

            ExampleAuditorium.AddShowtime(ShowtimeId.Create(ExampleShowtime.Id));

            ExampleMovie.AddShowtime(ShowtimeId.Create(ExampleShowtime.Id));

            context.SaveChanges();
        }
    }
}