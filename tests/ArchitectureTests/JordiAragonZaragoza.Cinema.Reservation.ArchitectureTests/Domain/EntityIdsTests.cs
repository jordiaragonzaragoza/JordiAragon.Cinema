﻿namespace JordiAragonZaragoza.Cinema.Reservation.ArchitectureTests.Domain
{
    using System;
    using System.Reflection;
    using FluentAssertions;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;
    using NetArchTest.Rules;
    using Xunit;

    public sealed class EntityIdsTests
    {
        private readonly Assembly assembly = AssemblyReference.Assembly;

        [Fact]
        public void EntityIds_Should_Be_Sealed()
        {
            // Act.
            var testResult = Types
                .InAssembly(this.assembly)
                .That()
                .Inherit(typeof(BaseEntityId<>))
                .Should()
                .BeSealed()
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void EntityIds_Should_EndingWith_Event()
        {
            // Act.
            var testResult = Types
                .InAssembly(this.assembly)
                .That()
                .Inherit(typeof(BaseEntityId<>))
                .Should()
                .HaveNameEndingWith("Id", StringComparison.Ordinal)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }
    }
}