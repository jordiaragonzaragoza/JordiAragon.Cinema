﻿namespace JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries
{
    using System;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.SharedKernel.Application.Contracts;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class GetShowtimesQuery(
        Guid? AuditoriumId,
        Guid? MovieId,
        DateTime? StartTimeOnUtc,
        DateTime? EndTimeOnUtc,
        int PageNumber,
        int PageSize)
        : IPaginatedQuery, IQuery<PaginatedCollectionOutputDto<ShowtimeReadModel>>, ICacheRequest
    {
        public string CacheKey
            => $"{ShowtimeConstants.CachePrefix}_{this.AuditoriumId}_{this.MovieId}_{this.StartTimeOnUtc}_{this.EndTimeOnUtc}_{this.PageNumber}_{this.PageSize}";

        public TimeSpan? AbsoluteExpirationInSeconds { get; }
    }
}