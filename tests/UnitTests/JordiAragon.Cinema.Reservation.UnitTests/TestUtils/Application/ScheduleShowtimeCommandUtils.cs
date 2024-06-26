﻿namespace JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Application
{
    using System;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Domain;

    public static class ScheduleShowtimeCommandUtils
    {
        public static ScheduleShowtimeCommand CreateCommand()
            => new(Constants.Auditorium.Id, Constants.Movie.Id, DateTimeOffset.UtcNow.AddYears(1));
    }
}