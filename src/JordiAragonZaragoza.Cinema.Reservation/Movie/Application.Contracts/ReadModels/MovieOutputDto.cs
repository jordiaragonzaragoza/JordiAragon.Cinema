﻿namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels
{
    using System;

    // TODO: This output dto will be a read model on moving to catalog bounded context.
    public sealed record class MovieOutputDto(Guid Id, string Title, TimeSpan Runtime);
}