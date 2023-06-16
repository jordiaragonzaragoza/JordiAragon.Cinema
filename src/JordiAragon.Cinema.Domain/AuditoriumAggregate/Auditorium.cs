﻿namespace JordiAragon.Cinema.Domain.AuditoriumAggregate
{
    using System.Collections.Generic;
    using System.Linq;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate.Events;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Entities;

    public class Auditorium : BaseAuditableEntity<AuditoriumId>, IAggregateRoot
    {
        private readonly List<ShowtimeId> showtimes = new();
        private readonly List<Seat> seats;

        public Auditorium(
            AuditoriumId id,
            IEnumerable<Seat> seats)
            : this(id)
        {
            this.seats = Guard.Against.NullOrEmpty(seats, nameof(seats)).ToList();
        }

        private Auditorium(
            AuditoriumId id)
            : base(id)
        {
            this.RegisterDomainEvent(new AuditoriumCreatedEvent(this.Id, this.Seats));
        }

        public IEnumerable<ShowtimeId> Showtimes => this.showtimes.AsReadOnly();

        public IEnumerable<Seat> Seats => this.seats.AsReadOnly();

        public static Auditorium Create(
            AuditoriumId id,
            IEnumerable<Seat> seats)
        {
            return new Auditorium(id, seats);
        }

        public void AddShowtime(ShowtimeId showtimeId)
        {
            this.showtimes.Add(showtimeId);

            this.RegisterDomainEvent(new ShowtimeAddedEvent(showtimeId));
        }
    }
}