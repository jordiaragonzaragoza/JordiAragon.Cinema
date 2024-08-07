﻿namespace JordiAragon.Cinema.Reservation.Showtime.Domain.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public sealed record class ShowtimeScheduledEvent(
        Guid AggregateId,
        Guid MovieId,
        DateTimeOffset SessionDateOnUtc,
        Guid AuditoriumId)
        : BaseDomainEvent(AggregateId);
}