using ChoreDistributor.Business;
using ChoreDistributor.Business.Factories;
using ChoreDistributor.Data;
using Microsoft.Extensions.DependencyInjection;

namespace ChoreDistributor.Console
{
    internal static class Startup
    {
        internal static IServiceProvider RegisterServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddKeyedSingleton<IChoreDistribution, RandomDistribution>(ChoreDistributors.Random);
            serviceCollection.AddKeyedSingleton<IChoreDistribution, LinearDistribution>(ChoreDistributors.Linear);
            serviceCollection.AddKeyedSingleton<IChoreDistribution, EqualDistribution>(ChoreDistributors.Equal);
            serviceCollection.AddKeyedSingleton<IChoreDistribution, IncomeDistribution>(ChoreDistributors.Income);
            serviceCollection.AddSingleton<IChoreSearcher, ChoreSearcher>();
            serviceCollection.AddSingleton<IRandomFactory, RandomFactory>();

            serviceCollection.AddSingleton<IChoreRepository, ChoreRepository>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
