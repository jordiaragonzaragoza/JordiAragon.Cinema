﻿namespace JordiAragon.Cinema.Reservation.UnitTests.Movie.Domain
{
    using System;
    using System.Globalization;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using Xunit;

    public sealed class EndOfPeriodTests
    {
        [Fact]
        public void CreateEndOfPeriod_WhenHavingADefaultDateTimeOffset_ShouldThrowArgumentException()
        {
            // Arrange
            var value = default(DateTimeOffset);

            // Act
            Func<EndOfPeriod> endOfPeriod = () => EndOfPeriod.Create(value);

            // Assert
            endOfPeriod.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CreateEndOfPeriod_WhenHavingAValidDateTimeOffset_ShouldReturnEndOfPeriod()
        {
            // Arrange
            var value = DateTimeOffset.UtcNow;

            // Act
            var endOfPeriod = EndOfPeriod.Create(value);

            // Assert
            endOfPeriod.Should().NotBeNull();
        }

        [Fact]
        public void ImplicitConversion_WhenHavingAEndOfPeriod_ShouldReturnDateTimeOffset()
        {
            // Arrange
            var endOfPeriodValue = DateTimeOffset.UtcNow;
            var endOfPeriod = EndOfPeriod.Create(endOfPeriodValue);

            // Act
            DateTimeOffset result = endOfPeriod;

            // Assert
            result.Should().Be(endOfPeriodValue);
        }

        [Fact]
        public void ExplicitConversion_WhenHavingADateTimeOffset_ShouldReturnEndOfPeriod()
        {
            // Arrange
            var dateTimeOffsetValue = DateTimeOffset.UtcNow;

            // Act
            EndOfPeriod endOfPeriod = (EndOfPeriod)dateTimeOffsetValue;

            // Assert
            endOfPeriod.Should().NotBeNull();
            endOfPeriod.Value.Should().Be(dateTimeOffsetValue);
        }

        [Fact]
        public void ToString_ShouldReturnStringRepresentationOfEndOfPeriod()
        {
            // Arrange
            var value = DateTimeOffset.UtcNow;
            var endOfPeriod = EndOfPeriod.Create(value);

            // Act
            var result = endOfPeriod.ToString();

            // Assert
            result.Should().Be(value.ToString(CultureInfo.InvariantCulture));
        }

        [Fact]
        public void Equality_Checks_ShouldWorkAsExpected()
        {
            // Arrange
            var value1 = DateTimeOffset.UtcNow;
            var value2 = value1.AddMinutes(1);

            var endOfPeriod1 = EndOfPeriod.Create(value1);
            var endOfPeriod2 = EndOfPeriod.Create(value1);
            var endOfPeriod3 = EndOfPeriod.Create(value2);

            // Act & Assert
            endOfPeriod1.Should().Be(endOfPeriod2);
            endOfPeriod1.Should().NotBe(endOfPeriod3);
        }
    }
}