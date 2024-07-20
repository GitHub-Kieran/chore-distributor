using ChoreDistributor.Business.Factories;
using ChoreDistributor.Models;

namespace ChoreDistributor.Business
{
    public sealed class IncomeDistribution : IChoreDistribution
    {
        private readonly IRandomFactory _randomFactory;
        private readonly IChoreSearcher _choreSearcher;

        public IncomeDistribution(IRandomFactory randomFactory, IChoreSearcher choreSearcher)
        {
            _randomFactory = randomFactory;
            _choreSearcher = choreSearcher;
        }

        public IDictionary<Person, IList<Chore>> Distribute(IList<Person> people, IList<Chore> chores)
        {
            var distributedChores = new DistributedChores();

            var totalIncome = people.Sum(p => p.Income);
            var totalWeight = chores.Sum(c => c.Weighting);

            var randomise = false;
            var remainingChores = chores;
            for (int i = 0; i < people.Count; i++)
            {
                var percentageContribution = people[i].Income / totalIncome;
                var choreContribution = totalWeight * (1 - percentageContribution);

                if (percentageContribution * people.Count == 1)
                {
                    // Each person contributes equally so best combinations must be randomised
                    randomise = true;
                }

                var bestCombination = _choreSearcher.FindBestCombinationForWeight(remainingChores, choreContribution);

                remainingChores = remainingChores.Except(bestCombination).ToList();

                distributedChores.Add(people[i], bestCombination);
            }

            if (randomise)
            {
                distributedChores = distributedChores.Randomise(_randomFactory.Create());
            }

            return distributedChores;
        }
    }
}
