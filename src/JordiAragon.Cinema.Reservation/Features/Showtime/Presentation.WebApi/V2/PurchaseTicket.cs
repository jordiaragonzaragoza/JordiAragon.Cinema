﻿namespace JordiAragon.Cinema.Reservation.Showtime.Presentation.WebApi.V2
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using MediatR;
    using IMapper = AutoMapper.IMapper;

    public class PurchaseTicket : Endpoint<PurchaseTicketRequest, TicketResponse>
    {
        public const string Route = "showtimes/{showtimeId}/tickets/{ticketId}/purchase";

        private readonly ISender sender;
        private readonly IMapper mapper;

        public PurchaseTicket(ISender sender, IMapper mapper)
        {
            this.sender = Guard.Against.Null(sender, nameof(sender));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Patch(PurchaseTicket.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Purchase a reservation for an existing Showtime";
                summary.Description = "Purchase a reservation for an existing Showtime";
            });
        }

        public async override Task HandleAsync(PurchaseTicketRequest req, CancellationToken ct)
        {
            var resultOutputDto = await this.sender.Send(new PurchaseTicketCommand(req.ShowtimeId, req.TicketId), ct);

            var resultResponse = this.mapper.Map<Result<TicketResponse>>(resultOutputDto);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}