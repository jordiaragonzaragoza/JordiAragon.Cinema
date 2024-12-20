﻿namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserTicket
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Application.Validators;

    public sealed class GetUserTicketQueryValidator : BaseValidator<GetUserTicketQuery>
    {
        public GetUserTicketQueryValidator()
        {
            this.RuleFor(x => x.UserId)
              .NotEmpty().WithMessage("UserId is required.");

            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");

            this.RuleFor(x => x.TicketId)
              .NotEmpty().WithMessage("TicketId is required.");
        }
    }
}