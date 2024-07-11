namespace ChoreDistributor.Business.Factories
{
    public class RandomFactory : IRandomFactory
    {
        public Random Create()
        {
            return new Random();
        }
    }
}
