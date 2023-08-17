﻿namespace JordiAragon.Cinema.Presentation.WebApi.FunctionalTests.Common
{
    using System.Net.Http;
    using Ardalis.GuardClauses;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Xunit;
    using Xunit.Abstractions;

    [Collection(nameof(SharedTestCollection))]
    public abstract class BaseWebApiFunctionalTests
    {
        protected BaseWebApiFunctionalTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
        {
            Guard.Against.Null(fixture, nameof(fixture));
            this.OutputHelper = Guard.Against.Null(outputHelper, nameof(outputHelper));

            this.HttpClient = fixture.CustomApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });
        }

        protected ITestOutputHelper OutputHelper { get; private init; }

        protected HttpClient HttpClient { get; private init; }

        protected abstract string ControllerBasePath { get; }
    }
}