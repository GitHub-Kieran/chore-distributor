using ChoreDistributor.Business.Extensions;
using ChoreDistributor.Models;

namespace ChoreDistributor.Business
{
    internal sealed class ChoreSearcher : IChoreSearcher
    {
        public IList<Chore> FindBestCombinationForWeight(IList<Chore> chores, float choreContributionWeight)
        {
            var index = 0;
            var indexes = chores.Select(c => index++).ToList();

            var linqCombinations = indexes.GetCombinations();

            var bestCombination = new List<Chore>();
            var minDifference = float.MaxValue;

            foreach (var combination in linqCombinations)
            {
                var combinationWeight = combination.Sum(index => chores[index].Weighting);
                var difference = Math.Abs(combinationWeight - choreContributionWeight);

                if (difference <= minDifference)
                {
                    minDifference = difference;
                    bestCombination = combination.Select(index => chores[index]).ToList();
                }
            }
            return bestCombination;
        }
    }
}
