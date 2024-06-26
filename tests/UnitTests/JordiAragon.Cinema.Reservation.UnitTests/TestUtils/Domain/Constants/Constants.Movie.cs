namespace JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Domain
{
    using System;
    using JordiAragon.Cinema.Reservation.Movie.Domain;

    public static partial class Constants
    {
        public static class Movie
        {
            public const string Title = "Inception";
            public static readonly MovieId Id = MovieId.Create(Guid.NewGuid());
            public static readonly TimeSpan Runtime = TimeSpan.FromHours(2) + TimeSpan.FromMinutes(28);
            public static readonly StartingPeriod StartingPeriod = StartingPeriod.Create(new DateTimeOffset(DateTimeOffset.UtcNow.AddYears(1).Year, 1, 1, 1, 1, 1, 1, TimeSpan.Zero));
            public static readonly EndOfPeriod EndOfPeriod = EndOfPeriod.Create(DateTimeOffset.UtcNow.AddYears(2));
            public static readonly ExhibitionPeriod ExhibitionPeriod = ExhibitionPeriod.Create(
                    StartingPeriod,
                    EndOfPeriod,
                    Runtime);
        }
    }
}