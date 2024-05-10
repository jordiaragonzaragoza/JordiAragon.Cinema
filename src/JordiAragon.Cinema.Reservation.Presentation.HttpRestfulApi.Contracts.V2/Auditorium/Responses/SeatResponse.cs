﻿namespace JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Auditorium.Responses
{
    using System;

    public sealed record class SeatResponse(Guid Id, int Row, int SeatNumber);
}