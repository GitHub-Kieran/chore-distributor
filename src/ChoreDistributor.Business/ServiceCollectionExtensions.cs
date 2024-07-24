using ChoreDistributor.Business.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace ChoreDistributor.Business
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBusiness(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddKeyedSingleton<IChoreDistribution, RandomDistribution>(ChoreDistributors.Random);
            serviceCollection.AddKeyedSingleton<IChoreDistribution, LinearDistribution>(ChoreDistributors.Linear);
            serviceCollection.AddKeyedSingleton<IChoreDistribution, EqualDistribution>(ChoreDistributors.Equal);
            serviceCollection.AddKeyedSingleton<IChoreDistribution, IncomeDistribution>(ChoreDistributors.Income);
            serviceCollection.AddSingleton<IChoreSearcher, ChoreSearcher>();
            serviceCollection.AddSingleton<IRandomFactory, RandomFactory>();
        }
    }
}
