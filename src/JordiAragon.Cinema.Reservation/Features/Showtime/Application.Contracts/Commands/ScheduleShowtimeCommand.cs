﻿namespace JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands
{
    using System;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class ScheduleShowtimeCommand(Guid AuditoriumId, Guid MovieId, DateTimeOffset SessionDateOnUtc)
        : ICommand<Guid>, IInvalidateCacheRequest
    {
        public string PrefixCacheKey => ShowtimeConstants.CachePrefix;
    }
}