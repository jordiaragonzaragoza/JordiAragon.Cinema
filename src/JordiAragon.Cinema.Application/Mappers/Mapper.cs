﻿namespace JordiAragon.Cinema.Application.Mappers
{
    using AutoMapper;
    using JordiAragon.Cinema.Application.Features.Auditorium.Queries;
    using JordiAragon.Cinema.Application.Features.Movie.Queries;

    public class Mapper : Profile
    {
        public Mapper()
        {
            this.MapAuditorium();
            this.MapMovie();
        }
    }
}
