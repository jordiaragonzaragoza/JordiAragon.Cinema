﻿namespace JordiAragon.Cinema.Reservation.Movie.Application.EventHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.Repositories;

    using NotFoundException = JordiAragon.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class ShowtimeCanceledEventHandler : IEventHandler<ShowtimeCanceledEvent>
    {
        private readonly IRepository<Movie, MovieId> movieRepository;

        public ShowtimeCanceledEventHandler(
            IRepository<Movie, MovieId> movieRepository)
        {
            this.movieRepository = Guard.Against.Null(movieRepository, nameof(movieRepository));
        }

        public async Task Handle(ShowtimeCanceledEvent notification, CancellationToken cancellationToken)
        {
            Guard.Against.Null(notification, nameof(notification));

            var existingMovie = await this.movieRepository.GetByIdAsync(MovieId.Create(notification.MovieId), cancellationToken)
                                    ?? throw new NotFoundException(nameof(Movie), notification.MovieId.ToString());

            existingMovie.RemoveActiveShowtime(ShowtimeId.Create(notification.AggregateId));

            await this.movieRepository.UpdateAsync(existingMovie, cancellationToken);
        }
    }
}