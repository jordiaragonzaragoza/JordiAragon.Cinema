﻿namespace JordiAragon.Cinema.Reservation.Movie.Presentation.WebApi.V1
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.Movie.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V1.Movie.Responses;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using MediatR;
    using IMapper = AutoMapper.IMapper;

    public class GetMovies : EndpointWithoutRequest<IEnumerable<MovieResponse>>
    {
        private readonly ISender sender;
        private readonly IMapper mapper;

        public GetMovies(ISender sender, IMapper mapper)
        {
            this.sender = Guard.Against.Null(sender, nameof(sender));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Get("movies");
            this.Version(1);
            this.Summary(summary =>
            {
                summary.Summary = "Gets a list of all Movies";
                summary.Description = "Gets a list of all Movies";
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var resultOutputDto = await this.sender.Send(new GetMoviesQuery(), ct);

            var resultResponse = this.mapper.Map<Result<IEnumerable<MovieResponse>>>(resultOutputDto);
            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}