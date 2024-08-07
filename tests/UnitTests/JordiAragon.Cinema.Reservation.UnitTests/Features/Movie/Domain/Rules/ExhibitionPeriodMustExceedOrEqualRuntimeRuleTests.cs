﻿namespace JordiAragon.Cinema.Reservation.UnitTests.Movie.Domain.Rules
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Domain.Rules;
    using JordiAragon.Cinema.Reservation.TestUtilities.Domain;
    using Xunit;

    public sealed class ExhibitionPeriodMustExceedOrEqualRuntimeRuleTests
    {
        public static IEnumerable<object[]> InvalidArgumentsConstructorConstructorExhibitionPeriodMustExceedOrEqualRuntimeRule()
        {
            var exhibitionPeriod = Constants.Movie.ExhibitionPeriod;
            var runtime = Constants.Movie.Runtime;

            var exhibitionPeriodValues = new object[] { default!, exhibitionPeriod };
            var runtimeValues = new object[] { default!, runtime };

            foreach (var exhibitionPeriodValue in exhibitionPeriodValues)
            {
                foreach (var runtimeValue in runtimeValues)
                {
                    if (exhibitionPeriodValue != null && exhibitionPeriodValue.Equals(exhibitionPeriod) &&
                        runtimeValue != null && runtimeValue.Equals(runtime))
                    {
                        continue;
                    }

                    yield return new object[] { exhibitionPeriodValue!, runtimeValue! };
                }
            }
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsConstructorConstructorExhibitionPeriodMustExceedOrEqualRuntimeRule))]
        public void ConstructorExhibitionPeriodMustExceedOrEqualRuntimeRule_WhenHavingInvalidArguments_ShouldThrowArgumentException(
            ExhibitionPeriod exhibitionPeriod,
            Runtime runtime)
        {
            // Act
            Func<ExhibitionPeriodMustExceedOrEqualRuntimeRule> constructor = () => new ExhibitionPeriodMustExceedOrEqualRuntimeRule(exhibitionPeriod, runtime);

            // Assert
            constructor.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ConstructorExhibitionPeriodMustExceedOrEqualRuntimeRule_WhenHavingAValidArguments_ShouldReturnExhibitionPeriodMustExceedOrEqualRuntimeRule()
        {
            // Arrange
            var exhibitionPeriod = Constants.Movie.ExhibitionPeriod;
            var runtime = Constants.Movie.Runtime;

            // Act
            var rule = new ExhibitionPeriodMustExceedOrEqualRuntimeRule(exhibitionPeriod, runtime);

            // Assert
            rule.Should().NotBeNull();
        }

        [Fact]
        public void IsBroken_WhenHavingBiggerEndOfPeriodThanStartingPeriod_ShouldBeFalse()
        {
            // Arrange
            var startingPeriod = StartingPeriod.Create(new DateTimeOffset(DateTimeOffset.UtcNow.AddYears(1).Year, 1, 1, 1, 1, 1, 1, TimeSpan.Zero));
            var endOfPeriod = EndOfPeriod.Create(DateTimeOffset.UtcNow.AddYears(2));
            var runtime = Runtime.Create(TimeSpan.FromHours(2) + TimeSpan.FromMinutes(28));

            var exhibitionPeriod = ExhibitionPeriod.Create(
                    startingPeriod,
                    endOfPeriod,
                    runtime);

            // Act
            var rule = new ExhibitionPeriodMustExceedOrEqualRuntimeRule(exhibitionPeriod, runtime);

            // Assert
            rule.IsBroken().Should().Be(false);
        }
    }
}