﻿namespace JordiAragon.Cinema.Reservation.Showtime.Presentation.WebApi.V2
{
    using System.Collections.Generic;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.SharedKernel.Application.Contracts;
    using JordiAragon.SharedKernel.Presentation.WebApi.Contracts;

    public class ShowtimesMapper : Profile
    {
        public ShowtimesMapper()
        {
            // Requests to queries or commands.
            this.CreateMap<ReserveSeatsRequest, ReserveSeatsCommand>();
            this.CreateMap<GetShowtimesRequest, GetShowtimesQuery>();
            this.CreateMap<GetShowtimeRequest, GetShowtimeQuery>();

            // OutputDtos to responses.
            this.CreateMap<TicketOutputDto, TicketResponse>();
            this.CreateMap<Result<TicketOutputDto>, Result<TicketResponse>>();

            this.CreateMap<ShowtimeReadModel, ShowtimeResponse>();
            this.CreateMap<Result<ShowtimeReadModel>, Result<ShowtimeResponse>>();

            this.CreateMap<PaginatedCollectionOutputDto<ShowtimeReadModel>, PaginatedCollectionResponse<ShowtimeResponse>>();
            this.CreateMap<Result<PaginatedCollectionOutputDto<ShowtimeReadModel>>, Result<PaginatedCollectionResponse<ShowtimeResponse>>>();

            this.CreateMap<AvailableSeatReadModel, SeatResponse>()
                .ForCtorParam(nameof(AvailableSeatReadModel.Id), opt => opt.MapFrom(src => src.SeatId));

            this.CreateMap<Result<IEnumerable<AvailableSeatReadModel>>, Result<IEnumerable<SeatResponse>>>();
        }
    }
}