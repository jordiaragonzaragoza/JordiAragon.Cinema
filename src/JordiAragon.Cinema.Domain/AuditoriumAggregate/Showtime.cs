﻿namespace JordiAragon.Cinema.Domain.AuditoriumAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate.Events;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate.Rules;
    using JordiAragon.Cinema.Domain.MovieAggregate;
    using JordiAragon.SharedKernel.Domain.Entities;
    using NotFoundException = JordiAragon.SharedKernel.Domain.Exceptions.NotFoundException;

    public class Showtime : BaseAuditableEntity<ShowtimeId>
    {
        private readonly List<Ticket> tickets = new();

        private Showtime(
            ShowtimeId id,
            MovieId movieId,
            DateTime sessionDateOnUtc,
            Auditorium auditorium)
            : this(id)
        {
            this.MovieId = Guard.Against.Null(movieId, nameof(movieId));
            this.SessionDateOnUtc = sessionDateOnUtc;
            this.Auditorium = Guard.Against.Null(auditorium, nameof(auditorium));
        }

        // Required by EF.
        private Showtime(
            ShowtimeId id)
            : base(id)
        {
        }

        public MovieId MovieId { get; private set; }

        public DateTime SessionDateOnUtc { get; private set; }

        public Auditorium Auditorium { get; private set; }

        public IEnumerable<Ticket> Tickets => this.tickets.AsReadOnly();

        public static Showtime Create(
            ShowtimeId id,
            MovieId movieId,
            DateTime sessionDateOnUtc,
            Auditorium auditorium)
        {
            return new Showtime(id, movieId, sessionDateOnUtc, auditorium);
        }

        public Ticket ReserveSeats(IEnumerable<SeatId> desiredSeatsIds, TicketId ticketId, DateTime createdTimeOnUtc)
        {
            var desiredSeats = this.Auditorium.Seats.Where(seat => desiredSeatsIds.Contains(seat.Id));

            this.CheckRule(new OnlyContiguousSeatsCanBeReservedRule(desiredSeats));

            var availableSeats = this.AvailableSeats();

            this.CheckRule(new OnlyAvailableSeatsCanBeReservedRule(desiredSeats, availableSeats));

            var ticket = this.AddTicket(ticketId, desiredSeats, createdTimeOnUtc);

            var newItemAddedEvent = new ReservedSeatsEvent(ticket);
            this.RegisterDomainEvent(newItemAddedEvent);

            return ticket;
        }

        public void PurchaseSeats(TicketId ticketId)
        {
            var ticket = this.Tickets.FirstOrDefault(ticket => ticket.Id == ticketId)
                ?? throw new NotFoundException(nameof(Ticket), ticketId.Value);

            this.CheckRule(new OnlyPossibleToPayOncePerTicketRule(ticket));

            ticket.MarkAsPaid();

            var @event = new PurchasedSeatsEvent(ticket);
            this.RegisterDomainEvent(@event);
        }

        public void ExpireReservedSeats(TicketId ticketToRemove)
        {
            Guard.Against.Null(ticketToRemove, nameof(ticketToRemove));

            var existingTicket = this.Tickets.FirstOrDefault(item => item.Id == ticketToRemove)
                                   ?? throw new NotFoundException(nameof(Ticket), ticketToRemove.Value.ToString());

            this.tickets.Remove(existingTicket);

            var @event = new ExpiredReservedSeatsEvent(existingTicket);
            this.RegisterDomainEvent(@event);
        }

        public IEnumerable<Seat> AvailableSeats()
        {
            var reservedSeats = this.ReservedSeats();

            return this.Auditorium.Seats.Except(reservedSeats)
                                        .OrderBy(s => s.Row)
                                        .ThenBy(s => s.SeatNumber);
        }

        private IEnumerable<Seat> ReservedSeats()
        {
            var seatIds = this.Tickets.SelectMany(ticket => ticket.Seats)
                                      .Select(ticketSeat => ticketSeat.SeatId);

            return this.Auditorium.Seats.Where(seat => seatIds.Contains(seat.Id))
                                        .OrderBy(s => s.Row)
                                        .ThenBy(s => s.SeatNumber);
        }

        private Ticket AddTicket(TicketId id, IEnumerable<Seat> seats, DateTime createdTimeOnUtc)
        {
            var newTicket = Ticket.Create(
                 id,
                 this.Id,
                 seats,
                 createdTimeOnUtc);

            this.tickets.Add(newTicket);

            return newTicket;
        }
    }
}