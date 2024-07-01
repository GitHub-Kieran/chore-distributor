using ChoreDistributor.Business.Extensions;
using ChoreDistributor.Models;
using System;

namespace ChoreDistributor.Business
{
    public sealed class IncomeDistribution : IChoreDistribution
    {
        public IDictionary<Person, IList<Chore>> Distribute(IList<Person> people, IList<Chore> chores)
        {
            var distributedChores = new Dictionary<Person, IList<Chore>>();

            var totalIncome = people.Sum(p => p.Income);
            var totalWeight = chores.Sum(c => c.Weighting);

            var randomise = false;
            // TODO: refactor into EqualDistribution code
            var remainingChores = chores;
            for (int i = 0; i < people.Count; i++)
            {
                var percentageContribution = people[i].Income / totalIncome;
                var choreContribution = totalWeight * (1 - percentageContribution);

                if (percentageContribution * people.Count == 1)
                {
                    // Each person contributes equally and we need to randomise closest matches
                    randomise = true;
                }

                var index = 0;
                var indexes = remainingChores.Select(c => index++).ToList();

                var linqCombinations = indexes.GetCombinations();

                var bestCombination = new List<Chore>();
                var minDifference = float.MaxValue;

                foreach (var combination in linqCombinations)
                {
                    var combinationWeight = combination.Sum(index => remainingChores[index].Weighting);
                    var difference = Math.Abs(combinationWeight - choreContribution);

                    if (difference <= minDifference)
                    {
                        minDifference = difference;
                        bestCombination = combination.Select(index => remainingChores[index]).ToList();
                    }
                }                             
                remainingChores = remainingChores.Except(bestCombination).ToList();

                distributedChores.Add(people[i], bestCombination);
            }            

            return distributedChores;
        }

    }
}
