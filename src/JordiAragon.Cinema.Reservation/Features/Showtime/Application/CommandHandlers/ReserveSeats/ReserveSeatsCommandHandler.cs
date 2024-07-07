﻿namespace JordiAragon.Cinema.Reservation.Showtime.Application.CommandHandlers.ReserveSeats
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.Cinema.Reservation.User.Domain;
    using JordiAragon.SharedKernel.Application.Commands;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public sealed class ReserveSeatsCommandHandler : BaseCommandHandler<ReserveSeatsCommand, TicketOutputDto>
    {
        private readonly IRepository<Showtime, ShowtimeId> showtimeRepository;
        private readonly IReadRepository<User, UserId> userRepository;
        private readonly IIdGenerator guidGenerator;
        private readonly IMapper mapper;
        private readonly IDateTime dateTime;
        private readonly IReservationManager showtimeManager;
        private readonly IReadRepository<Movie, MovieId> movieRepository;
        private readonly IReadRepository<Auditorium, AuditoriumId> auditoriumRepository;

        public ReserveSeatsCommandHandler(
            IRepository<Showtime, ShowtimeId> showtimeRepository,
            IReadRepository<User, UserId> userRepository,
            IReservationManager showtimeManager,
            IMapper mapper,
            IIdGenerator guidGenerator,
            IDateTime dateTime,
            IReadRepository<Movie, MovieId> movieRepository,
            IReadRepository<Auditorium, AuditoriumId> auditoriumRepository)
        {
            this.showtimeRepository = Guard.Against.Null(showtimeRepository, nameof(showtimeRepository));
            this.userRepository = Guard.Against.Null(userRepository, nameof(userRepository));
            this.showtimeManager = Guard.Against.Null(showtimeManager, nameof(showtimeManager));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
            this.guidGenerator = Guard.Against.Null(guidGenerator, nameof(guidGenerator));
            this.dateTime = Guard.Against.Null(dateTime, nameof(dateTime));
            this.movieRepository = Guard.Against.Null(movieRepository, nameof(movieRepository));
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
        }

        public override async Task<Result<TicketOutputDto>> Handle(ReserveSeatsCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await this.userRepository.GetByIdAsync(UserId.Create(request.UserId), cancellationToken);
            if (existingUser is null)
            {
                return Result.NotFound($"{nameof(User)}: {request.UserId} not found.");
            }

            var existingShowtime = await this.showtimeRepository.GetByIdAsync(ShowtimeId.Create(request.ShowtimeId), cancellationToken);
            if (existingShowtime is null)
            {
                return Result.NotFound($"{nameof(Showtime)}: {request.ShowtimeId} not found.");
            }

            var desiredSeatsIds = this.mapper.Map<IEnumerable<SeatId>>(request.SeatsIds);

            // Make the reserve.
            var newticket = await this.showtimeManager.ReserveSeatsAsync(
                existingShowtime,
                desiredSeatsIds,
                TicketId.Create(this.guidGenerator.Create()),
                UserId.Create(request.UserId),
                this.dateTime.UtcNow,
                cancellationToken);

            await this.showtimeRepository.UpdateAsync(existingShowtime, cancellationToken);

            // Prepare command response to avoid delays on eventual consistency.
            // This may change if asynchronous commands are used.
            var existingMovie = await this.movieRepository.GetByIdAsync(existingShowtime.MovieId, cancellationToken);
            if (existingMovie is null)
            {
                throw new NotFoundException(nameof(Movie), existingShowtime.MovieId.ToString());
            }

            var existingAuditorium = await this.auditoriumRepository.GetByIdAsync(existingShowtime.AuditoriumId, cancellationToken);
            if (existingAuditorium is null)
            {
                throw new NotFoundException(nameof(Auditorium), existingShowtime.AuditoriumId.ToString());
            }

            var seats = existingAuditorium.Seats.Where(seat => desiredSeatsIds.Contains(seat.Id));

            var seatsOutputDto = seats.Select(seat
                => new SeatOutputDto(seat.Id, seat.Row, seat.SeatNumber));

            var ticketOutputDto = new TicketOutputDto(
                newticket.Id,
                request.UserId,
                existingShowtime.Id,
                existingShowtime.SessionDateOnUtc,
                existingAuditorium.Name,
                existingMovie.Title,
                seatsOutputDto,
                newticket.IsPurchased);

            return Result.Success(ticketOutputDto);
        }
    }
}