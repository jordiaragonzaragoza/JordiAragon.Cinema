﻿namespace JordiAragon.Cinema.Reservation.Auditorium.Presentation.HttpRestfulApi.V1
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Showtime.Responses;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.SharedKernel.Application.Contracts;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    public sealed class GetShowtimes : Endpoint<GetShowtimesRequest, IEnumerable<ShowtimeResponse>>
    {
        private readonly IQueryBus queryBus;

        public GetShowtimes(IQueryBus queryBus)
        {
            this.queryBus = Guard.Against.Null(queryBus, nameof(queryBus));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Get("auditoriums/{auditoriumId}/showtimes");
            this.Version(1, 2);
            this.Summary(summary =>
            {
                summary.Summary = "Gets a list of all Showtimes";
                summary.Description = "Gets a list of all Showtimes";
            });
        }

        public async override Task HandleAsync(GetShowtimesRequest req, CancellationToken ct)
        {
            Guard.Against.Null(req, nameof(req));

            var query = new GetShowtimesQuery(
                req.AuditoriumId,
                AuditoriumName: null,
                MovieId: null,
                MovieTitle: null,
                StartTimeOnUtc: null,
                EndTimeOnUtc: null,
                PageNumber: 1,
                PageSize: 0);

            var resultOutputDto = await this.queryBus.SendAsync(query, ct);

            var resultResponse = MapToResultResponse(resultOutputDto);

            await this.SendResponseAsync(resultResponse, ct);
        }

        private static Result<IEnumerable<ShowtimeResponse>> MapToResultResponse(Result<PaginatedCollectionOutputDto<ShowtimeReadModel>> resultOutputDto)
        {
            Guard.Against.Null(resultOutputDto, nameof(resultOutputDto));

            if (!resultOutputDto.IsSuccess)
            {
                return Result<IEnumerable<ShowtimeResponse>>.Error(resultOutputDto.Errors.ToArray());
            }

            var paginatedCollection = resultOutputDto.Value;
            var showtimeResponses = paginatedCollection.Items
                .Select(showtime => new ShowtimeResponse(
                    showtime.Id,
                    showtime.MovieTitle,
                    showtime.SessionDateOnUtc,
                    showtime.AuditoriumId));

            return Result<IEnumerable<ShowtimeResponse>>.Success(showtimeResponses);
        }
    }
}