﻿namespace JordiAragon.Cinema.Reservation.Movie.Application.EventHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Events;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using MediatR;
    using NotFoundException = JordiAragon.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class ShowtimeCanceledEventHandler : INotificationHandler<ShowtimeCanceledEvent>
    {
        private readonly IRepository<Movie, MovieId> movieRepository;

        public ShowtimeCanceledEventHandler(
            IRepository<Movie, MovieId> movieRepository)
        {
            this.movieRepository = Guard.Against.Null(movieRepository, nameof(movieRepository));
        }

        public async Task Handle(ShowtimeCanceledEvent @event, CancellationToken cancellationToken)
        {
            var existingMovie = await this.movieRepository.GetByIdAsync(MovieId.Create(@event.MovieId), cancellationToken)
                                    ?? throw new NotFoundException(nameof(Movie), @event.MovieId.ToString());

            existingMovie.RemoveShowtime(ShowtimeId.Create(@event.ShowtimeId));

            await this.movieRepository.UpdateAsync(existingMovie, cancellationToken);
        }
    }
}