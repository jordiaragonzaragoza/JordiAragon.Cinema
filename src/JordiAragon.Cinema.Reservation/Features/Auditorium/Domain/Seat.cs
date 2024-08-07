﻿namespace JordiAragon.Cinema.Reservation.Auditorium.Domain
{
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.Entities;

    public sealed class Seat : BaseEntity<SeatId>
    {
        private Seat(
            SeatId id,
            Row row,
            SeatNumber seatNumber)
            : base(id)
        {
            this.Row = Guard.Against.Default(row, nameof(row));
            this.SeatNumber = Guard.Against.Default(seatNumber, nameof(seatNumber));
        }

        // Required by EF
        private Seat()
        {
        }

        public Row Row { get; private set; } = default!;

        public SeatNumber SeatNumber { get; private set; } = default!;

        internal static Seat Create(
            SeatId seatId,
            Row row,
            SeatNumber seatNumber)
        {
            return new Seat(seatId, row, seatNumber);
        }
    }
}