﻿// <auto-generated />
using System;
using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Migrations
{
    [DbContext(typeof(ReservationContext))]
    partial class ReservationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("JordiAragon.Cinema.Reservation.Auditorium.Domain.Auditorium", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Rows")
                        .HasColumnType("int");

                    b.Property<int>("SeatsPerRow")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Auditoriums", (string)null);
                });

            modelBuilder.Entity("JordiAragon.Cinema.Reservation.Movie.Domain.Movie", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<TimeSpan>("Runtime")
                        .HasColumnType("time");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Movies", (string)null);
                });

            modelBuilder.Entity("JordiAragon.Cinema.Reservation.Showtime.Domain.Showtime", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AuditoriumId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("MovieId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("SessionDateOnUtc")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("Showtimes", (string)null);
                });

            modelBuilder.Entity("JordiAragon.SharedKernel.Infrastructure.EntityFramework.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("DateOccurredOnUtc")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DateProcessedOnUtc")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Error")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessages");
                });

            modelBuilder.Entity("JordiAragon.Cinema.Reservation.Auditorium.Domain.Auditorium", b =>
                {
                    b.OwnsMany("JordiAragon.Cinema.Reservation.Showtime.Domain.ShowtimeId", "Showtimes", b1 =>
                        {
                            b1.Property<Guid>("AuditoriumId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("Value")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("ShowtimeId");

                            b1.HasKey("AuditoriumId", "Id");

                            b1.ToTable("AuditoriumsShowtimeIds", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("AuditoriumId");
                        });

                    b.OwnsMany("JordiAragon.Cinema.Reservation.Auditorium.Domain.Seat", "Seats", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("Id");

                            b1.Property<Guid>("AuditoriumId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<short>("Row")
                                .HasColumnType("smallint");

                            b1.Property<short>("SeatNumber")
                                .HasColumnType("smallint");

                            b1.HasKey("Id", "AuditoriumId");

                            b1.HasIndex("AuditoriumId");

                            b1.ToTable("AuditoriumsSeats", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("AuditoriumId");
                        });

                    b.Navigation("Seats");

                    b.Navigation("Showtimes");
                });

            modelBuilder.Entity("JordiAragon.Cinema.Reservation.Movie.Domain.Movie", b =>
                {
                    b.OwnsOne("JordiAragon.Cinema.Reservation.Movie.Domain.ExhibitionPeriod", "ExhibitionPeriod", b1 =>
                        {
                            b1.Property<Guid>("MovieId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTimeOffset?>("EndOfPeriodOnUtc")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("EndOfExhibitionPeriodOnUtc");

                            b1.Property<DateTimeOffset?>("StartingPeriodOnUtc")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("StartingExhibitionPeriodOnUtc");

                            b1.HasKey("MovieId");

                            b1.ToTable("Movies");

                            b1.WithOwner()
                                .HasForeignKey("MovieId");
                        });

                    b.OwnsMany("JordiAragon.Cinema.Reservation.Showtime.Domain.ShowtimeId", "Showtimes", b1 =>
                        {
                            b1.Property<Guid>("MovieId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("Value")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("ShowtimeId");

                            b1.HasKey("MovieId", "Id");

                            b1.ToTable("MoviesShowtimeIds", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("MovieId");
                        });

                    b.Navigation("ExhibitionPeriod")
                        .IsRequired();

                    b.Navigation("Showtimes");
                });

            modelBuilder.Entity("JordiAragon.Cinema.Reservation.Showtime.Domain.Showtime", b =>
                {
                    b.OwnsMany("JordiAragon.Cinema.Reservation.Showtime.Domain.Ticket", "Tickets", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("Id");

                            b1.Property<Guid>("ShowtimeId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTimeOffset>("CreatedTimeOnUtc")
                                .HasColumnType("datetimeoffset");

                            b1.Property<bool>("IsPurchased")
                                .HasColumnType("bit");

                            b1.HasKey("Id", "ShowtimeId");

                            b1.HasIndex("ShowtimeId");

                            b1.ToTable("ShowtimesTickets", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ShowtimeId");

                            b1.OwnsMany("JordiAragon.Cinema.Reservation.Auditorium.Domain.SeatId", "Seats", b2 =>
                                {
                                    b2.Property<Guid>("TicketId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<Guid>("ShowtimeId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("int");

                                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b2.Property<int>("Id"));

                                    b2.Property<Guid>("Value")
                                        .HasColumnType("uniqueidentifier")
                                        .HasColumnName("SeatId");

                                    b2.HasKey("TicketId", "ShowtimeId", "Id");

                                    b2.ToTable("ShowtimeTicketSeatIds", (string)null);

                                    b2.WithOwner()
                                        .HasForeignKey("TicketId", "ShowtimeId");
                                });

                            b1.Navigation("Seats");
                        });

                    b.Navigation("Tickets");
                });
#pragma warning restore 612, 618
        }
    }
}
