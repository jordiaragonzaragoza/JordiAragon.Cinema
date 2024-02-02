﻿namespace JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public record class GetShowtimesQuery(Guid AuditoriumId, Guid? MovieId, DateTime? StartTimeOnUtc, DateTime? EndTimeOnUtc) : IQuery<IEnumerable<ShowtimeReadModel>>, ICacheRequest
    {
        public string CacheKey => $"{ShowtimeConstants.CachePrefix}_{this.AuditoriumId}_{this.MovieId}_{this.StartTimeOnUtc}_{this.EndTimeOnUtc}";

        public TimeSpan? AbsoluteExpirationInSeconds { get; }
    }
}