﻿namespace JordiAragon.Cinema.Application.Features.Showtime.Commands.ReserveSeats
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragon.Cinema.Application.Contracts.Features.Auditorium.Queries;
    using JordiAragon.Cinema.Application.Contracts.Features.Showtime.Commands;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate.Specifications;
    using JordiAragon.Cinema.Domain.MovieAggregate;
    using JordiAragon.Cinema.Domain.MovieAggregate.Specifications;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate.Specifications;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using Volo.Abp.Guids;

    public class ReserveSeatsCommandHandler : ICommandHandler<ReserveSeatsCommand, TicketOutputDto>
    {
        private readonly IReadRepository<Auditorium> auditoriumReadRepository;
        private readonly IGuidGenerator guidGenerator;
        private readonly IMapper mapper;
        private readonly IDateTime dateTime;
        private readonly IReadRepository<Movie> movieReadRepository;
        private readonly IRepository<Showtime> showtimeRepository;

        public ReserveSeatsCommandHandler(
            IReadRepository<Auditorium> auditoriumReadRepository,
            IReadRepository<Movie> movieReadRepository,
            IRepository<Showtime> showtimeRepository,
            IMapper mapper,
            IGuidGenerator guidGenerator,
            IDateTime dateTime)
        {
            this.auditoriumReadRepository = Guard.Against.Null(auditoriumReadRepository, nameof(auditoriumReadRepository));
            this.movieReadRepository = Guard.Against.Null(movieReadRepository, nameof(movieReadRepository));
            this.showtimeRepository = Guard.Against.Null(showtimeRepository, nameof(showtimeRepository));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
            this.guidGenerator = Guard.Against.Null(guidGenerator, nameof(guidGenerator));
            this.dateTime = Guard.Against.Null(dateTime, nameof(dateTime));
        }

        public async Task<Result<TicketOutputDto>> Handle(ReserveSeatsCommand request, CancellationToken cancellationToken)
        {
            var existingShowtime = await this.showtimeRepository.FirstOrDefaultAsync(new ShowtimeByIdSpec(ShowtimeId.Create(request.ShowtimeId)), cancellationToken);
            if (existingShowtime is null)
            {
                return Result.NotFound($"{nameof(Showtime)}: {request.ShowtimeId} not found.");
            }

            var existingAuditorium = await this.auditoriumReadRepository.FirstOrDefaultAsync(new AuditoriumByIdSpec(existingShowtime.AuditoriumId), cancellationToken);
            if (existingAuditorium is null)
            {
                return Result.NotFound($"{nameof(Auditorium)}: {existingShowtime.AuditoriumId} not found.");
            }

            // Make the reserve.
            var desiredSeatsIds = this.mapper.Map<IEnumerable<SeatId>>(request.SeatsIds);

            var newticket = ShowtimeManager.ReserveSeats(
                existingAuditorium,
                existingShowtime,
                desiredSeatsIds,
                TicketId.Create(this.guidGenerator.Create()),
                this.dateTime.UtcNow);

            await this.showtimeRepository.UpdateAsync(existingShowtime, cancellationToken);

            // Prepare OutputDto.
            var existingmovie = await this.movieReadRepository.FirstOrDefaultAsync(new MovieByIdSpec(existingShowtime.MovieId), cancellationToken);
            if (existingmovie is null)
            {
                return Result.NotFound($"{nameof(Movie)}: {existingShowtime.MovieId} not found.");
            }

            var seats = existingAuditorium.Seats.Where(seat => desiredSeatsIds.Contains(seat.Id));

            var ticketOutputDto = new TicketOutputDto(
                newticket.Id.Value,
                existingShowtime.SessionDateOnUtc,
                existingAuditorium.Id.Value,
                existingmovie.Title,
                this.mapper.Map<IEnumerable<SeatOutputDto>>(seats));

            return Result.Success(ticketOutputDto);
        }
    }
}