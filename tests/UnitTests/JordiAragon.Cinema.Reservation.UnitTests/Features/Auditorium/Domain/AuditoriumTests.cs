﻿namespace JordiAragon.Cinema.Reservation.UnitTests.Auditorium.Domain
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain.Events;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Domain;
    using JordiAragon.SharedKernel.Domain.Exceptions;
    using Xunit;

    public class AuditoriumTests
    {
        public static IEnumerable<object[]> InvalidArgumentsCreateAuditorium()
        {
            var argument1Values = new object[] { 0, 10 };
            var argument2Values = new object[] { 0, 10 };

            foreach (var arg1 in argument1Values)
            {
                foreach (var arg2 in argument2Values)
                {
                    if (arg1.Equals(10) &&
                        arg2.Equals(10))
                    {
                        continue;
                    }

                    yield return new object[] { arg1, arg2 };
                }
            }
        }

        [Fact]
        public void CreateAuditorium_WhenHavingCorrectArguments_ShouldCreateAuditoriumAndAddAuditoriumCreatedEvent()
        {
            // Arrange
            var id = Constants.Auditorium.Id;
            ushort rows = Constants.Auditorium.Rows;
            ushort seatsPerRow = Constants.Auditorium.SeatsPerRow;

            // Act
            var auditorium = Auditorium.Create(id, rows, seatsPerRow);

            // Assert
            auditorium.Should().NotBeNull();
            auditorium.Id.Should().Be(id);
            auditorium.Rows.Should().Be(rows);
            auditorium.SeatsPerRow.Should().Be(seatsPerRow);
            auditorium.Seats.Should().HaveCount(rows * seatsPerRow);

            auditorium.Events.Should()
                              .ContainSingle(x => x is AuditoriumCreatedEvent)
                              .Which.Should().BeOfType<AuditoriumCreatedEvent>()
                              .Which.Should().Match<AuditoriumCreatedEvent>(e =>
                                                                            e.AuditoriumId == id &&
                                                                            e.Rows == rows &&
                                                                            e.SeatsPerRow == seatsPerRow);
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsCreateAuditorium))]
        public void CreateAuditorium_WhenHavingInCorrectRowsSeatsArguments_ShouldThrowInvalidAggregateStateException(
            ushort rows,
            ushort seatsPerRow)
        {
            // Arrange
            var id = Constants.Auditorium.Id;

            // Act
            Func<Auditorium> auditorium = () => Auditorium.Create(id, rows, seatsPerRow);

            // Assert
            auditorium.Should().Throw<InvalidAggregateStateException<Auditorium, AuditoriumId>>();
        }

        [Fact]
        public void CreateAuditorium_WhenHavingInCorrectAuditoriumIdArgument_ShouldThrowArgumentNullException()
        {
            // Arrange
            AuditoriumId id = null;
            ushort rows = Constants.Auditorium.Rows;
            ushort seatsPerRow = Constants.Auditorium.SeatsPerRow;

            // Act
            Func<Auditorium> auditorium = () => Auditorium.Create(id, rows, seatsPerRow);

            // Assert
            auditorium.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddShowtimeToAuditorium_WhenShowtimeIdIsValid_ShouldAddShowtimeIdAndAddShowtimeAddedEvent()
        {
            // Arrange.
            var auditorium = CreateAuditoriumUtils.Create();
            var showtimeId = Constants.Showtime.Id;

            // Act.
            auditorium.AddShowtime(showtimeId);

            // Assert.
            auditorium.Showtimes.Should()
                          .Contain(showtimeId)
                          .And
                          .HaveCount(1);

            auditorium.Events.Should()
                              .ContainSingle(x => x is ShowtimeAddedEvent)
                              .Which.Should().BeOfType<ShowtimeAddedEvent>()
                              .Which.Should().Match<ShowtimeAddedEvent>(e =>
                                                                            e.AuditoriumId == auditorium.Id &&
                                                                            e.ShowtimeId == showtimeId);
        }

        [Fact]
        public void AddShowtimeToAuditorium_WhenShowtimeIdIsInvalid_ShouldThrowArgumentNullException()
        {
            // Arrange.
            var auditorium = CreateAuditoriumUtils.Create();
            ShowtimeId showtimeId = null;

            // Act.
            Action addShowtime = () => auditorium.AddShowtime(showtimeId);

            // Assert.
            addShowtime.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void RemoveShowtimeToAuditorium_WhenShowtimeIdIsValid_ShouldAddShowtimeIdAndAddShowtimeRemovedEvent()
        {
            // Arrange.
            var auditorium = CreateAuditoriumUtils.Create();
            var showtimeId = Constants.Showtime.Id;
            auditorium.AddShowtime(showtimeId);

            // Act.
            auditorium.RemoveShowtime(showtimeId);

            // Assert.
            auditorium.Showtimes.Should()
                          .NotContain(showtimeId)
                          .And
                          .HaveCount(0);

            auditorium.Events.Should()
                              .ContainSingle(x => x is ShowtimeRemovedEvent)
                              .Which.Should().BeOfType<ShowtimeRemovedEvent>()
                              .Which.Should().Match<ShowtimeRemovedEvent>(e =>
                                                                            e.AuditoriumId == auditorium.Id &&
                                                                            e.ShowtimeId == showtimeId);
        }

        [Fact]
        public void RemoveShowtimeToAuditorium_WhenShowtimeIdIsInvalid_ShouldThrowArgumentNullException()
        {
            // Arrange.
            var auditorium = CreateAuditoriumUtils.Create();
            ShowtimeId showtimeId = null;

            // Act.
            Action removeShowtime = () => auditorium.RemoveShowtime(showtimeId);

            // Assert.
            removeShowtime.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void RemoveShowtimeToAuditorium_WhenShowtimeIdIsNotPresent_ShouldThrowNotFoundException()
        {
            // Arrange
            var auditorium = CreateAuditoriumUtils.Create();
            var showtimeId = Constants.Showtime.Id;

            // Act.
            Action removeShowtime = () => auditorium.RemoveShowtime(showtimeId);

            // Assert.
            removeShowtime.Should().Throw<NotFoundException>();
        }
    }
}