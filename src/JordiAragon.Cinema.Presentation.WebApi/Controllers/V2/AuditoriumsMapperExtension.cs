﻿namespace JordiAragon.Cinema.Presentation.WebApi.Controllers.V2
{
    using System.Collections.Generic;
    using Ardalis.Result;
    using JordiAragon.Cinema.Application.Contracts.Auditorium.Queries;
    using JordiAragon.Cinema.Application.Contracts.Showtime.Commands;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Presentation.WebApi.Mappers.V2;

    public static class AuditoriumsMapperExtension
    {
        public static void MapAuditorium(this Mapper maps)
        {
            // Requests to queries or commands.
            maps.CreateMap<CreateShowtimeRequest, CreateShowtimeCommand>();

            // OutputDtos to responses.
            maps.CreateMap<SeatOutputDto, SeatResponse>();
            maps.CreateMap<Result<IEnumerable<SeatOutputDto>>, Result<IEnumerable<SeatResponse>>>();
            maps.CreateMap<AuditoriumOutputDto, AuditoriumResponse>();
            maps.CreateMap<Result<IEnumerable<AuditoriumOutputDto>>, Result<IEnumerable<AuditoriumResponse>>>();
        }
    }
}