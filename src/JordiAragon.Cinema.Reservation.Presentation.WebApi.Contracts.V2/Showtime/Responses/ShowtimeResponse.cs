﻿namespace JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Responses
{
    using System;

    public record class ShowtimeResponse(Guid Id, string MovieTitle, DateTimeOffset SessionDateOnUtc, Guid AuditoriumId);
}