﻿namespace JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.V2.Showtime
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration;
    using JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.Common;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class ReserveSeatsTests : BaseHttpRestfulApiFunctionalTests
    {
        public ReserveSeatsTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task CreateTicket_WhenHavingValidArguments_ShouldCreateRequiredTicket()
        {
            // Arrange
            var showtimeId = SeedData.ExampleShowtime.Id;

            var routeAvailableSeats = $"api/v2/{GetAvailableSeats.Route}";
            routeAvailableSeats = routeAvailableSeats.Replace("{showtimeId}", showtimeId.ToString());

            var availableSeatsResponse = await this.Fixture.HttpClient.GetAndDeserializeAsync<IEnumerable<SeatResponse>>(routeAvailableSeats, this.OutputHelper);

            var seatsIds = availableSeatsResponse.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3).Select(seat => seat.Id);

            var reserveSeatsRequest = new ReserveSeatsRequest(showtimeId, seatsIds);
            var reserveSeatsContent = StringContentHelpers.FromModelAsJson(reserveSeatsRequest);

            var reserveSeatsRoute = $"api/v2/{ReserveSeats.Route}";
            reserveSeatsRoute = reserveSeatsRoute.Replace("{showtimeId}", showtimeId.ToString());

            // Act
            var ticketResponse = await this.Fixture.HttpClient.PostAndDeserializeAsync<TicketResponse>(reserveSeatsRoute, reserveSeatsContent, this.OutputHelper);

            // Required to satisfy eventual consistency on projections.
            await AddEventualConsistencyDelayAsync();

            var availableSeatsAfterReservation = await this.Fixture.HttpClient.GetAndDeserializeAsync<IEnumerable<SeatResponse>>(routeAvailableSeats, this.OutputHelper);

            // Assert
            ticketResponse.SessionDateOnUtc.Should()
                .Be(SeedData.ExampleShowtime.SessionDateOnUtc);

            ticketResponse.AuditoriumName.Should()
                .Be(SeedData.ExampleAuditorium.Name);

            ticketResponse.MovieTitle.Should()
                .Be(SeedData.ExampleMovie.Title);

            ticketResponse.Seats.Select(seatResponse => seatResponse.Id).Should()
                .Contain(seatsIds);

            ticketResponse.IsPurchased.Should().BeFalse();

            availableSeatsAfterReservation.Should().NotContain(ticketResponse.Seats);
        }
    }
}