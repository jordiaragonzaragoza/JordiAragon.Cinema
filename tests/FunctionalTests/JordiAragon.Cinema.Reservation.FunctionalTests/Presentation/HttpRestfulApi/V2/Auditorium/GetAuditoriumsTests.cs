﻿namespace JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.V2.Auditorium
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Auditorium.Presentation.HttpRestfulApi.V2;
    using JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.Common;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Auditorium.Responses;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class GetAuditoriumsTests : BaseHttpRestfulApiFunctionalTests
    {
        public GetAuditoriumsTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task GetAllAuditoriums_WhenHavingValidUrl_ShouldReturnOneAuditorium()
        {
            // Arrange
            var url = $"api/v2/{GetAuditoriums.Route}";

            // Act
            var response = await this.Fixture.HttpClient.GetAndDeserializeAsync<IEnumerable<AuditoriumResponse>>(url, this.OutputHelper);

            // Assert
            response.Should()
                .NotBeNullOrEmpty()
                .And
                .HaveCount(1);
        }
    }
}