﻿namespace JordiAragon.Cinema.Application.Contracts.Features.Showtime.Commands
{
    using System;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public record class PurchaseSeatsCommand(Guid ShowtimeId, Guid TicketId) : ICommand;
}