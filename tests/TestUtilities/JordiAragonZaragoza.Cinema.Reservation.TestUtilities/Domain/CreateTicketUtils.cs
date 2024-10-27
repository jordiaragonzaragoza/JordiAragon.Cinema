﻿namespace JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain
{
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;

    public static class CreateTicketUtils
    {
        public static Ticket Create()
            => Ticket.Create(
                Constants.Ticket.Id,
                Constants.Ticket.UserId,
                Constants.Ticket.SeatIds,
                Constants.Ticket.CreatedTimeOnUtc);
    }
}