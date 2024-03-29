﻿namespace JordiAragon.Cinema.Reservation.Showtime.Presentation.WebApi.V2
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using MediatR;

    public class DeleteShowtime : Endpoint<DeleteShowtimeRequest>
    {
        public const string Route = "showtimes/{showtimeId}";

        private readonly ISender sender;

        public DeleteShowtime(ISender sender)
        {
            this.sender = Guard.Against.Null(sender, nameof(sender));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Delete(DeleteShowtime.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Deletes an existing Showtime";
                summary.Description = "Deletes an existing Showtime";
            });
        }

        public async override Task HandleAsync(DeleteShowtimeRequest req, CancellationToken ct)
        {
            var resultResponse = await this.sender.Send(new DeleteShowtimeCommand(req.ShowtimeId), ct);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}