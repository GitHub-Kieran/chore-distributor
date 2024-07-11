using ChoreDistributor.Business.Extensions;
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

        public EqualDistribution(IRandomFactory randomFactory)
        {
            _randomFactory = randomFactory;
        }

        public IDictionary<Person, IList<Chore>> Distribute(IList<Person> people, IList<Chore> chores)
        {
            var distributedChores = new Dictionary<Person, IList<Chore>>();

            var totalWeight = chores.Sum(c => c.Weighting);
            var averageDistribtion = totalWeight / people.Count;
                
            var remainingChores = chores;
            for (int i = 0; i < people.Count; i++)
            {
                var index = 0;
                var indexes = remainingChores.Select(c => index++).ToList();

                var linqCombinations = indexes.GetCombinations();

                var bestCombination = new List<Chore>();
                var minDifference = float.MaxValue;

                foreach (var combination in linqCombinations)
                {
                    var combinationWeight = combination.Sum(index => remainingChores[index].Weighting);
                    var difference = Math.Abs(combinationWeight - averageDistribtion);

                    if (difference <= minDifference)
                    {
                        minDifference = difference;
                        bestCombination = combination.Select(index => remainingChores[index]).ToList();
                    }
                }
                remainingChores = remainingChores.Except(bestCombination).ToList();

                distributedChores.Add(people[i], bestCombination);
            }

            if (true)
            {
                var random = _randomFactory.Create();
                var shuffledChores = distributedChores.Values.OrderBy(_ => random.Next(0, distributedChores.Values.Count)).ToList();

                for (int i = 0; i < people.Count; i++)
                {
                    distributedChores[people[i]] = shuffledChores[i];
                }
            }

            return distributedChores;
        }
    }
}
