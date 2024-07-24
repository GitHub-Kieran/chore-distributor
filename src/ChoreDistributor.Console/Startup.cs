using ChoreDistributor.Business;
using ChoreDistributor.Data;
using Microsoft.Extensions.DependencyInjection;

namespace ChoreDistributor.Console
{
    internal static class Startup
    {
        internal static IServiceProvider RegisterServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddBusiness();
            serviceCollection.AddData();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
