﻿namespace JordiAragon.Cinema.Domain.UnitTests.AuditoriumAggregate
{
    using System;
    using FluentAssertions;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using Xunit;

    public class AuditoriumIdTests
    {
        [Fact]
        public void CreateAuditoriumId_WhenHavingAnEmptyGuid_ShouldThrowArgumentException()
        {
            // Arrange
            var id = Guid.Empty;

            // Act
            Func<AuditoriumId> auditoriumId = () => AuditoriumId.Create(id);

            // Assert
            auditoriumId.Should().Throw<ArgumentException>();
        }
    }
}