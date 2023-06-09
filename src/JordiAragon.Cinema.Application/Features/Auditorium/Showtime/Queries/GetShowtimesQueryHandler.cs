﻿namespace JordiAragon.Cinema.Application.Features.Auditorium.Showtime.Queries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.Cinema.Application.Contracts.Features.Auditorium.Showtime.Queries;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate.Specifications;
    using JordiAragon.Cinema.Domain.MovieAggregate;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class GetShowtimesQueryHandler : IQueryHandler<GetShowtimesQuery, IEnumerable<ShowtimeOutputDto>>
    {
        private readonly IReadRepository<Auditorium> auditoriumRepository;
        private readonly IReadRepository<Movie> movieRepository;

        public GetShowtimesQueryHandler(
            IReadRepository<Auditorium> auditoriumRepository,
            IReadRepository<Movie> movieRepository)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
            this.movieRepository = Guard.Against.Null(movieRepository, nameof(movieRepository));
        }

        public async Task<Result<IEnumerable<ShowtimeOutputDto>>> Handle(GetShowtimesQuery request, CancellationToken cancellationToken)
        {
            var existingAuditorium = await this.auditoriumRepository.FirstOrDefaultAsync(new AuditoriumWithShowtimesByIdSpec(AuditoriumId.Create(request.AuditoriumId)), cancellationToken);
            if (existingAuditorium is null)
            {
                return Result.NotFound($"{nameof(Auditorium)}: {request.AuditoriumId} not found.");
            }

            if (!existingAuditorium.Showtimes.Any())
            {
                return Result.NotFound($"There is not any {nameof(Showtime)} avaliable for {nameof(Auditorium)} {request.AuditoriumId}.");
            }

            var showtimeOutputDtos = new List<ShowtimeOutputDto>();

            foreach (var showtime in existingAuditorium.Showtimes)
            {
                var movie = await this.movieRepository.GetByIdAsync(showtime.MovieId, cancellationToken);
                if (movie is null)
                {
                    return Result.NotFound($"{nameof(Movie)}: {showtime.MovieId} not found.");
                }

                showtimeOutputDtos.Add(new ShowtimeOutputDto(showtime.Id.Value, movie.Title, showtime.SessionDateOnUtc, existingAuditorium.Id.Value));
            }

            return Result.Success(showtimeOutputDtos.AsEnumerable());
        }
    }
}