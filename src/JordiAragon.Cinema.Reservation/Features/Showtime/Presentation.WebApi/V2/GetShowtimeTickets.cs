﻿namespace JordiAragon.Cinema.Reservation.Showtime.Presentation.WebApi.V2
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragon.SharedKernel.Presentation.WebApi.Contracts;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using MediatR;
    using IMapper = AutoMapper.IMapper;

    public class GetShowtimeTickets : Endpoint<GetShowtimeTicketsRequest, PaginatedCollectionResponse<TicketResponse>>
    {
        public const string Route = "showtimes/{showtimeId}/tickets";

        private readonly ISender internalBus;
        private readonly IMapper mapper;

        public GetShowtimeTickets(ISender internalBus, IMapper mapper)
        {
            this.internalBus = Guard.Against.Null(internalBus, nameof(internalBus));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Get(GetShowtimeTickets.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Gets a list of tickets for an exiting showtime";
                summary.Description = "Gets a list of tickets for an exiting showtime";
            });
        }

        public async override Task HandleAsync(GetShowtimeTicketsRequest req, CancellationToken ct)
        {
            var resultOutputDto = await this.internalBus.Send(this.mapper.Map<GetShowtimeTicketsQuery>(req), ct);

            var resultResponse = this.mapper.Map<Result<PaginatedCollectionResponse<TicketResponse>>>(resultOutputDto);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}