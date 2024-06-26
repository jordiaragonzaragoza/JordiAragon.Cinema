﻿namespace JordiAragon.Cinema.Reservation.Auditorium.Infrastructure.EntityFramework
{
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public sealed class AuditoriumConfiguration : BaseAggregateRootTypeConfiguration<Auditorium, AuditoriumId>
    {
        public override void Configure(EntityTypeBuilder<Auditorium> builder)
        {
            this.ConfigureAuditoriumsTable(builder);

            ConfigureAuditoriumsShowtimeIdsTable(builder);

            ConfigureAuditoriumsSeatsTable(builder);
        }

        private static void ConfigureAuditoriumsShowtimeIdsTable(EntityTypeBuilder<Auditorium> builder)
        {
            builder.OwnsMany(auditorium => auditorium.Showtimes, sib =>
            {
                sib.ToTable("AuditoriumsShowtimeIds");

                sib.WithOwner().HasForeignKey(nameof(AuditoriumId));

                sib.Property(showtimeId => showtimeId.Value)
                .ValueGeneratedNever()
                .HasColumnName(nameof(ShowtimeId));
            });

            builder.Metadata.FindNavigation(nameof(Auditorium.Showtimes))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }

        private static void ConfigureAuditoriumsSeatsTable(EntityTypeBuilder<Auditorium> builder)
        {
            builder.OwnsMany(auditorium => auditorium.Seats, sb =>
            {
                sb.ToTable("AuditoriumsSeats");

                sb.WithOwner().HasForeignKey(nameof(AuditoriumId));

                sb.HasKey(nameof(Seat.Id), nameof(AuditoriumId));

                sb.Property(seat => seat.Id)
                .HasColumnName(nameof(Seat.Id))
                .ValueGeneratedNever()
                .HasConversion(id => id.Value, value => SeatId.Create(value));
            });

            builder.Metadata.FindNavigation(nameof(Auditorium.Seats))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }

        private void ConfigureAuditoriumsTable(EntityTypeBuilder<Auditorium> builder)
        {
            builder.ToTable("Auditoriums");

            base.Configure(builder);

            builder.Property(auditorium => auditorium.Id)
                .ValueGeneratedNever()
                .HasConversion(id => id.Value, value => AuditoriumId.Create(value));
        }
    }
}