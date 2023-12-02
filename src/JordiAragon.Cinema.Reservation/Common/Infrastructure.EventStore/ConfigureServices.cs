﻿namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EventStore
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ConfigureServices
    {
        public static IServiceCollection AddEventStoreServices(this IServiceCollection serviceCollection, IConfiguration configuration, string applicationName)
        {
            /*
            var esConnection = EventStoreConnection.Create(
                configuration["eventStore:connectionString"],
                ConnectionSettings.Create().KeepReconnecting(),
                applicationName);
            var store = new EsAggregateStore(esConnection);

            serviceCollection.AddSingleton(esConnection);
            serviceCollection.AddSingleton<IAggregateStore>(store);*/

            return serviceCollection;
        }
    }
}