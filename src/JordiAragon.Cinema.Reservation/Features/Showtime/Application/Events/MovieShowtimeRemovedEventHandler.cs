﻿namespace JordiAragon.Cinema.Reservation.Showtime.Application.Events
{
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.Cinema.Reservation.Movie.Domain.Events;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class MovieShowtimeRemovedEventHandler : INotificationHandler<ShowtimeRemovedEvent>
    {
        private readonly ILogger<MovieShowtimeRemovedEventHandler> logger;

        public MovieShowtimeRemovedEventHandler(ILogger<MovieShowtimeRemovedEventHandler> logger)
        {
            this.logger = logger;
        }

        public Task Handle(ShowtimeRemovedEvent @event, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Handled Domain Event: {DomainEvent}", @event.GetType().Name);

            ////throw new InvalidOperationException($"{nameof(MovieShowtimeRemovedEventHandler)} has crashed.");
            return Task.CompletedTask;
        }
    }
}