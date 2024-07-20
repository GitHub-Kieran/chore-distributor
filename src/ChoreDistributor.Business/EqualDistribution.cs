using ChoreDistributor.Business.Factories;
using ChoreDistributor.Models;

namespace ChoreDistributor.Business
{
    /// <summary>
    /// Find the fairest combination of chores for each person, based on the weight of each chore.
    /// For example, given 5 chores with a combined weight of 38, and 2 people, each person would recieve
    /// a combination of chores with a summed weight closest to 19.
    /// </summary>
    public sealed class EqualDistribution : IChoreDistribution
    {
        private readonly IRandomFactory _randomFactory;
        private readonly IChoreSearcher _choreSearcher;

        public EqualDistribution(IRandomFactory randomFactory, IChoreSearcher choreSearcher)
        {
            _randomFactory = randomFactory;
            _choreSearcher = choreSearcher;
        }

        public IDictionary<Person, IList<Chore>> Distribute(IList<Person> people, IList<Chore> chores)
        {
            var distributedChores = new DistributedChores();

            var totalWeight = chores.Sum(c => c.Weighting);
            var averageDistribtion = totalWeight / people.Count;

            var remainingChores = chores;
            for (int i = 0; i < people.Count; i++)
            {
                var bestCombination = _choreSearcher.FindBestCombinationForWeight(remainingChores, averageDistribtion);

                remainingChores = remainingChores.Except(bestCombination).ToList();

                distributedChores.Add(people[i], bestCombination);
            }

            return distributedChores.Randomise(_randomFactory.Create());
        }
    }
}
