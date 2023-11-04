﻿namespace JordiAragon.Cinemas.Ticketing.Auditorium.Domain.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public sealed record class ShowtimeRemovedEvent(Guid AuditoriumId, Guid ShowtimeId) : BaseDomainEvent(AuditoriumId);
}