﻿namespace JordiAragon.Cinema.Reservation.Movie.Application.Queries
{
    using System;
    using AutoMapper;
    using JordiAragon.Cinema.Reservation.Movie.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Movie.Domain;

    public class MovieMapper : Profile
    {
        public MovieMapper()
        {
            this.CreateMap<MovieId, Guid>()
                .ConvertUsing(src => src.Value);

            this.CreateMap<Movie, MovieOutputDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value));
        }
    }
}