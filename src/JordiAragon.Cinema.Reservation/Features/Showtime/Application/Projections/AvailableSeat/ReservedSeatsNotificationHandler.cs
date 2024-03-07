﻿namespace JordiAragon.Cinema.Reservation.Showtime.Application.Projections.AvailableSeat
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Notifications;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using MediatR;

    public sealed class ReservedSeatsNotificationHandler : INotificationHandler<ReservedSeatsNotification>
    {
        private readonly IRangeableRepository<AvailableSeatReadModel, Guid> availableReadModelRepository;
        private readonly ISpecificationReadRepository<AvailableSeatReadModel, Guid> availableReadModelSpecificationRepository;

        public ReservedSeatsNotificationHandler(
            ISpecificationReadRepository<AvailableSeatReadModel, Guid> availableReadModelSpecificationRepository,
            IRangeableRepository<AvailableSeatReadModel, Guid> availableReadModelRepository)
        {
            this.availableReadModelRepository = Guard.Against.Null(availableReadModelRepository, nameof(availableReadModelRepository));
            this.availableReadModelSpecificationRepository = Guard.Against.Null(availableReadModelSpecificationRepository, nameof(availableReadModelSpecificationRepository));
        }

        public async Task Handle(ReservedSeatsNotification notification, CancellationToken cancellationToken)
        {
            var @event = notification.Event;

            var availableSeats = await this.availableReadModelSpecificationRepository.ListAsync(new GetAvailableSeatsByShowtimeIdAndSeatIdsSpec(@event.ShowtimeId, @event.SeatIds), cancellationToken);

            await this.availableReadModelRepository.DeleteRangeAsync(availableSeats, cancellationToken);
        }
    }
}