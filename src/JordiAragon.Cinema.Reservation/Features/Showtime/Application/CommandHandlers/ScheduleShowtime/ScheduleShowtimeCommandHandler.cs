﻿namespace JordiAragon.Cinema.Reservation.Showtime.Application.CommandHandlers.ScheduleShowtime
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.SharedKernel.Application.Commands;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.Repositories;

    public sealed class ScheduleShowtimeCommandHandler : BaseCommandHandler<ScheduleShowtimeCommand, Guid>
    {
        private readonly IReadRepository<Auditorium, AuditoriumId> auditoriumRepository;
        private readonly IReadRepository<Movie, MovieId> movieRepository;
        private readonly IRepository<Showtime, ShowtimeId> showtimeRepository;
        private readonly IIdGenerator guidGenerator;

        public ScheduleShowtimeCommandHandler(
            IReadRepository<Auditorium, AuditoriumId> auditoriumRepository,
            IReadRepository<Movie, MovieId> movieRepository,
            IRepository<Showtime, ShowtimeId> showtimeRepository,
            IIdGenerator guidGenerator)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
            this.movieRepository = Guard.Against.Null(movieRepository, nameof(movieRepository));
            this.showtimeRepository = Guard.Against.Null(showtimeRepository, nameof(showtimeRepository));
            this.guidGenerator = Guard.Against.Null(guidGenerator, nameof(guidGenerator));
        }

        public override async Task<Result<Guid>> Handle(ScheduleShowtimeCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));

            var existingAuditorium = await this.auditoriumRepository.GetByIdAsync(AuditoriumId.Create(request.AuditoriumId), cancellationToken);
            if (existingAuditorium is null)
            {
                return Result.NotFound($"{nameof(Auditorium)}: {request.AuditoriumId} not found.");
            }

            var existingMovie = await this.movieRepository.GetByIdAsync(MovieId.Create(request.MovieId), cancellationToken);
            if (existingMovie is null)
            {
                return Result.NotFound($"{nameof(Movie)}: {request.MovieId} not found.");
            }

            // TODO: This is temporal. A scheduler manager with business rules is required.
            var newShowtime = Showtime.Schedule(
                ShowtimeId.Create(this.guidGenerator.Create()),
                MovieId.Create(existingMovie.Id),
                request.SessionDateOnUtc,
                AuditoriumId.Create(existingAuditorium.Id));

            await this.showtimeRepository.AddAsync(newShowtime, cancellationToken);

            return Result.Success(newShowtime.Id.Value);
        }
    }
}