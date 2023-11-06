﻿namespace JordiAragon.Cinemas.Reservation.Showtime.Application.Queries.GetShowtimes
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.Cinemas.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragon.Cinemas.Reservation.Auditorium.Domain;
    using JordiAragon.Cinemas.Reservation.Movie.Domain;
    using JordiAragon.Cinemas.Reservation.Movie.Domain.Specifications;
    using JordiAragon.Cinemas.Reservation.Showtime.Domain;
    using JordiAragon.Cinemas.Reservation.Showtime.Domain.Specifications;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class GetShowtimesQueryHandler : IQueryHandler<GetShowtimesQuery, IEnumerable<ShowtimeOutputDto>>
    {
        private readonly IReadRepository<Movie> movieRepository;
        private readonly IReadRepository<Showtime> showtimeRepository;

        public GetShowtimesQueryHandler(
            IReadRepository<Movie> movieRepository,
            IReadRepository<Showtime> showtimeRepository)
        {
            this.movieRepository = Guard.Against.Null(movieRepository, nameof(movieRepository));
            this.showtimeRepository = Guard.Against.Null(showtimeRepository, nameof(showtimeRepository));
        }

        public async Task<Result<IEnumerable<ShowtimeOutputDto>>> Handle(GetShowtimesQuery request, CancellationToken cancellationToken)
        {
            var specification = new ShowtimesByAuditoriumIdSpec(
                AuditoriumId.Create(request.AuditoriumId),
                request.StartTimeOnUtc,
                request.EndTimeOnUtc,
                request.MovieId.HasValue ? MovieId.Create(request.MovieId.Value) : null);

            var existingShowtimes = await this.showtimeRepository.ListAsync(specification, cancellationToken);
            if (!existingShowtimes.Any())
            {
                return Result.NotFound($"There is not any {nameof(Showtime)} avaliable.");
            }

            var showtimeOutputDtos = new List<ShowtimeOutputDto>();

            foreach (var showtime in existingShowtimes)
            {
                var movie = await this.movieRepository.FirstOrDefaultAsync(new MovieByIdSpec(showtime.MovieId), cancellationToken);
                if (movie is null)
                {
                    return Result.NotFound($"{nameof(Movie)}: {showtime.MovieId.Value} not found.");
                }

                showtimeOutputDtos.Add(new ShowtimeOutputDto(showtime.Id.Value, movie.Title, showtime.SessionDateOnUtc, request.AuditoriumId));
            }

            return Result.Success(showtimeOutputDtos.AsEnumerable());
        }
    }
}