﻿namespace JordiAragon.Cinema.Reservation.Showtime.Application.Commands.PurchaseTicket
{
    using FluentValidation;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Application.Validators;

    public class PurchaseTicketCommandValidator : BaseValidator<PurchaseTicketCommand>
    {
        public PurchaseTicketCommandValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
             .NotEmpty().WithMessage("ShowtimeId is required.");

            this.RuleFor(x => x.TicketId)
             .NotEmpty().WithMessage("TicketId is required.");
        }
    }
}