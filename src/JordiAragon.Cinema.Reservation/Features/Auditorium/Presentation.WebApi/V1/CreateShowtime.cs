﻿namespace JordiAragon.Cinema.Reservation.Auditorium.Presentation.WebApi.V1
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using MediatR;
    using IMapper = AutoMapper.IMapper;

    public sealed class CreateShowtime : Endpoint<CreateShowtimeRequest, Guid>
    {
        private readonly ISender internalBus;
        private readonly IMapper mapper;

        public CreateShowtime(ISender internalBus, IMapper mapper)
        {
            this.internalBus = Guard.Against.Null(internalBus, nameof(internalBus));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Post("auditoriums/{auditoriumId}/showtimes");
            this.Version(1, 2);
            this.Summary(summary =>
            {
                summary.Summary = "Creates a new Showtime";
                summary.Description = "Creates a new Showtime";
            });
        }

        public async override Task HandleAsync(CreateShowtimeRequest req, CancellationToken ct)
        {
            var command = this.mapper.Map<CreateShowtimeCommand>(req);

            var resultResponse = await this.internalBus.Send(command, ct);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}