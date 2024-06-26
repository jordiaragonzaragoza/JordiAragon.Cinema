﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Migrations.BusinessModelStore
{
    /// <inheritdoc />
    public partial class InitialBusinessModelStoreMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Auditoriums",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rows = table.Column<int>(type: "int", nullable: false),
                    SeatsPerRow = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditoriums", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdempotentConsumers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsumerFullName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdempotentConsumers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Runtime = table.Column<TimeSpan>(type: "time", nullable: false),
                    StartingExhibitionPeriodOnUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    EndOfExhibitionPeriodOnUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateOccurredOnUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateProcessedOnUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Showtimes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovieId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SessionDateOnUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    AuditoriumId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsEnded = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Showtimes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditoriumsSeats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuditoriumId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Row = table.Column<short>(type: "smallint", nullable: false),
                    SeatNumber = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriumsSeats", x => new { x.Id, x.AuditoriumId });
                    table.ForeignKey(
                        name: "FK_AuditoriumsSeats_Auditoriums_AuditoriumId",
                        column: x => x.AuditoriumId,
                        principalTable: "Auditoriums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditoriumsShowtimeIds",
                columns: table => new
                {
                    AuditoriumId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShowtimeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriumsShowtimeIds", x => new { x.AuditoriumId, x.Id });
                    table.ForeignKey(
                        name: "FK_AuditoriumsShowtimeIds_Auditoriums_AuditoriumId",
                        column: x => x.AuditoriumId,
                        principalTable: "Auditoriums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MoviesShowtimeIds",
                columns: table => new
                {
                    MovieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShowtimeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviesShowtimeIds", x => new { x.MovieId, x.Id });
                    table.ForeignKey(
                        name: "FK_MoviesShowtimeIds_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShowtimesTickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShowtimeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTimeOnUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsPurchased = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowtimesTickets", x => new { x.Id, x.ShowtimeId });
                    table.ForeignKey(
                        name: "FK_ShowtimesTickets_Showtimes_ShowtimeId",
                        column: x => x.ShowtimeId,
                        principalTable: "Showtimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShowtimeTicketSeatIds",
                columns: table => new
                {
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShowtimeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowtimeTicketSeatIds", x => new { x.TicketId, x.ShowtimeId, x.Id });
                    table.ForeignKey(
                        name: "FK_ShowtimeTicketSeatIds_ShowtimesTickets_TicketId_ShowtimeId",
                        columns: x => new { x.TicketId, x.ShowtimeId },
                        principalTable: "ShowtimesTickets",
                        principalColumns: new[] { "Id", "ShowtimeId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriumsSeats_AuditoriumId",
                table: "AuditoriumsSeats",
                column: "AuditoriumId");

            migrationBuilder.CreateIndex(
                name: "IX_ShowtimesTickets_ShowtimeId",
                table: "ShowtimesTickets",
                column: "ShowtimeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditoriumsSeats");

            migrationBuilder.DropTable(
                name: "AuditoriumsShowtimeIds");

            migrationBuilder.DropTable(
                name: "IdempotentConsumers");

            migrationBuilder.DropTable(
                name: "MoviesShowtimeIds");

            migrationBuilder.DropTable(
                name: "OutboxMessages");

            migrationBuilder.DropTable(
                name: "ShowtimeTicketSeatIds");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Auditoriums");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "ShowtimesTickets");

            migrationBuilder.DropTable(
                name: "Showtimes");
        }
    }
}
