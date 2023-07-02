﻿namespace JordiAragon.Cinema.Domain.MovieAggregate.Events
{
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;
    using JordiAragon.SharedKernel.Domain.Events;

    public record class ShowtimeAddedEvent(ShowtimeId ShowtimeId) : BaseDomainEvent;
}