﻿namespace JordiAragon.Cinemas.Ticketing.Showtime.Domain.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public sealed record class ExpiredReservedSeatsEvent(Guid ShowtimeId, Guid TicketId) : BaseDomainEvent(ShowtimeId);
}