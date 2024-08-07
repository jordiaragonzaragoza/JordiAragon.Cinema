﻿namespace JordiAragon.Cinema.Reservation.Movie.Application.CommandHanders.AddMovie
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.Cinema.Reservation.Movie.Application.Contracts.Commands;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.SharedKernel.Application.Commands;
    using JordiAragon.SharedKernel.Contracts.Repositories;

    public sealed class AddMovieCommandHandler : BaseCommandHandler<AddMovieCommand>
    {
        private readonly IRepository<Movie, MovieId> movieRepository;

        public AddMovieCommandHandler(IRepository<Movie, MovieId> movieRepository)
        {
            this.movieRepository = Guard.Against.Null(movieRepository, nameof(movieRepository));
        }

        public override async Task<Result> Handle(AddMovieCommand command, CancellationToken cancellationToken)
        {
            // TODO: There cannot be two movies with the same title, runtime and exhibitionPeriod.
            // Check will be done via domain service.
            var newMovie = Movie.Add(
                id: MovieId.Create(command.MovieId),
                title: command.Title,
                runtime: Runtime.Create(command.Runtime),
                exhibitionPeriod: ExhibitionPeriod.Create(
                    StartingPeriod.Create(command.StartingPeriod),
                    EndOfPeriod.Create(command.EndOfPeriod),
                    Runtime.Create(command.Runtime)));

            await this.movieRepository.AddAsync(newMovie, cancellationToken);

            return Result.Success();
        }
    }
}