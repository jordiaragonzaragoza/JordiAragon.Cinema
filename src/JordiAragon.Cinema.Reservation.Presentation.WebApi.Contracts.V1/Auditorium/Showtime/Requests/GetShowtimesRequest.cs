﻿namespace JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Requests
{
    using System;

    public record class GetShowtimesRequest(Guid AuditoriumId);
}