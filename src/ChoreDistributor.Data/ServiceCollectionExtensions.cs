using Microsoft.Extensions.DependencyInjection;

namespace ChoreDistributor.Data
{
    public static class ServiceCollectionExtensions
    {
        public static void AddData(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IChoreRepository, ChoreRepository>();
        }
    }
}
