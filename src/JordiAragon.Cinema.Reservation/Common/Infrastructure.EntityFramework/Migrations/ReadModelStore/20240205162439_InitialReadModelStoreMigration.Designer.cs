﻿// <auto-generated />
using System;
using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Migrations.ReadModelStore
{
    [DbContext(typeof(ReservationReadModelContext))]
    [Migration("20240205162439_InitialReadModelStoreMigration")]
    partial class InitialReadModelStoreMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels.ShowtimeReadModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AuditoriumId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AuditoriumName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MovieTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("SessionDateOnUtc")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("Showtimes");
                });
#pragma warning restore 612, 618
        }
    }
}