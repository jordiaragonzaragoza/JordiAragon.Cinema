﻿namespace JordiAragon.Cinema.Reservation.User.Presentation.HttpRestfulApi.V2
{
    using System.Collections.Generic;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.User.Requests;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.User.Responses;
    using JordiAragon.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.User.Application.Contracts.ReadModels;

    public sealed class UsersMapper : Profile
    {
        public UsersMapper()
        {
            // Requests to queries or commands.
            this.CreateMap<UserTicketRequest, GetUserTicketQuery>();
            this.CreateMap<UserTicketsRequest, GetUserTicketsQuery>();

            // OutputDtos to responses.
            this.CreateMap<UserOutputDto, UserResponse>();
            this.CreateMap<Result<IEnumerable<UserOutputDto>>, Result<IEnumerable<UserResponse>>>();
        }
    }
}