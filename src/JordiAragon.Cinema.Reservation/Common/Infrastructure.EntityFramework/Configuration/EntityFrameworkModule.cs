﻿namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration
{
    using System.Reflection;
    using Autofac;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.BusinessModel;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.DataModel;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.ReadModel;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.SharedKernel;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.Repositories;

    public sealed class EntityFrameworkModule : AssemblyModule
    {
        protected override Assembly CurrentAssembly => AssemblyReference.Assembly;

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            RegisterBusinessModelRepositories(builder);
            RegisterReadModelsRepositories(builder);
            RegisterDataModelsRepositories(builder);
        }

        private static void RegisterBusinessModelRepositories(ContainerBuilder builder)
        {
            // TODO: Temporal registration. Remove on event sourcing.
            builder.RegisterType<ReservationRepository<Showtime, ShowtimeId>>()
                    .As<IRepository<Showtime, ShowtimeId>>()
                    .InstancePerLifetimeScope();

            // Write Repositories
            builder.RegisterType<ReservationRepository<Movie, MovieId>>()
                    .As<IRepository<Movie, MovieId>>()
                    .InstancePerLifetimeScope();

            builder.RegisterType<ReservationRepository<Auditorium, AuditoriumId>>()
                    .As<IRepository<Auditorium, AuditoriumId>>()
                    .InstancePerLifetimeScope();

            // Read Repositories
            builder.RegisterGeneric(typeof(ReservationRepository<,>))
                .As(typeof(IReadRepository<,>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReservationRepository<,>))
                .As(typeof(IReadListRepository<,>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReservationRepository<,>))
                .As(typeof(ISpecificationReadRepository<,>))
                .InstancePerLifetimeScope();
        }

        private static void RegisterReadModelsRepositories(ContainerBuilder builder)
        {
            // Write Repositories
            builder.RegisterGeneric(typeof(ReservationReadModelRepository<>))
                .As(typeof(IRepository<,>))
                .InstancePerLifetimeScope();

            // Read Repositories
            builder.RegisterGeneric(typeof(ReservationReadModelRepository<>))
                .As(typeof(IReadRepository<,>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReservationReadModelRepository<>))
                .As(typeof(IReadListRepository<,>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReservationReadModelRepository<>))
                .As(typeof(ISpecificationReadRepository<,>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReservationReadModelRepository<>))
                .As(typeof(IPaginatedSpecificationReadRepository<>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReservationReadModelRepository<>))
                .As(typeof(IRangeableRepository<,>))
                .InstancePerLifetimeScope();
        }

        private static void RegisterDataModelsRepositories(ContainerBuilder builder)
        {
            // Write Repositories
            builder.RegisterGeneric(typeof(ReservationDataModelRepository<>))
                .As(typeof(IRepository<,>))
                .InstancePerLifetimeScope();

            // Read Repositories
            builder.RegisterGeneric(typeof(ReservationDataModelRepository<>))
                .As(typeof(IReadRepository<,>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReservationDataModelRepository<>))
                .As(typeof(IReadListRepository<,>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReservationDataModelRepository<>))
                .As(typeof(ISpecificationReadRepository<,>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReservationDataModelCachedSpecificationRepository<>))
                .As(typeof(ICachedSpecificationRepository<,>))
                .InstancePerLifetimeScope();
        }
    }
}