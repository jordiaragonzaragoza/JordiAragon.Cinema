﻿namespace JordiAragon.Cinema.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Ticket.Requests
{
    using System;
    using System.Collections.Generic;

    public record class CreateTicketRequest(IEnumerable<Guid> SeatsIds);
}