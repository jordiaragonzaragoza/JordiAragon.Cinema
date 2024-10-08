﻿namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework
{
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Auditorium.Infrastructure.EntityFramework;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Infrastructure.EntityFramework;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Infrastructure.EntityFramework;
    using JordiAragon.Cinema.Reservation.User.Domain;
    using JordiAragon.Cinema.Reservation.User.Infrastructure.EntityFramework;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Context;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Interceptors;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public sealed class ReservationBusinessModelContext : BaseBusinessModelContext
    {
        public ReservationBusinessModelContext(
            DbContextOptions<ReservationBusinessModelContext> options,
            ILoggerFactory loggerFactory,
            IHostEnvironment hostEnvironment,
            SoftDeleteEntitySaveChangesInterceptor softDeleteEntitySaveChangesInterceptor)
            : base(options, loggerFactory, hostEnvironment, softDeleteEntitySaveChangesInterceptor)
        {
        }

        public DbSet<User> Users => this.Set<User>();

        public DbSet<Auditorium> Auditoriums => this.Set<Auditorium>();

        public DbSet<Movie> Movies => this.Set<Movie>();

        public DbSet<Showtime> Showtimes => this.Set<Showtime>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Guard.Against.Null(modelBuilder, nameof(modelBuilder));

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new AuditoriumConfiguration());
            modelBuilder.ApplyConfiguration(new MovieConfiguration());
            modelBuilder.ApplyConfiguration(new ShowtimeConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}