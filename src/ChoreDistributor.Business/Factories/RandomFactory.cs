namespace ChoreDistributor.Business.Factories
{
    internal sealed class RandomFactory : IRandomFactory
    {
        public Random Create()
        {
            return new Random();
        }
    }
}
