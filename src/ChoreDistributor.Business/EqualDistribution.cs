using ChoreDistributor.Models;

namespace ChoreDistributor.Business
{
    public sealed class EqualDistribution : IChoreDistribution
    {
        public IDictionary<Person, IList<Chore>> Distribute(IList<Person> people, IList<Chore> chores)
        {
            var distributedChores = new Dictionary<Person, IList<Chore>>();

            var random = new Random();
            
            var total = chores.Sum(c => c.Weighting);

            var averageDistribtion = total / people.Count;

            var closestMatches = new Dictionary<int, List<Chore>>();
                
            var remainingChores = chores;
            for (int i = 0; i < people.Count; i++)
            {
                var index = 0;
                var indexes = remainingChores.Select(c => index++).ToList();

                var linqCombinations = GetCombinations(indexes);

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

                closestMatches.Add(i, bestCombination);

                remainingChores = remainingChores.Except(bestCombination).ToList();
            }
            // TODO: Add class descriptions
            // TODO: Add readme descriptions
            // TODO: Randomise
            for (int i = 0; i < people.Count; i++)
            {
                distributedChores.Add(people[i] , closestMatches[i]);
            }

            return distributedChores;
        }

        /// <summary>
        /// We need to try every combination of chores to find the fairest combinations
        /// The array [ 0, 1, 2, 3, 4 ] representing chore indexes becomes something like the following:
        /// [ [0], [1], [2], [0, 1], [0, 2], [0, 3], [0, 4], [1, 2], [1, 3], [1, 4], [0, 1, 2], [0, 1, 3], [0, 1, 4], [1, 2, 3], [1, 2, 4], [0, 1, 2, 3] ]   
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elements"></param>
        /// <returns>IEnumerable<IEnumerable<T>></returns>
        public static IEnumerable<IEnumerable<T>> GetCombinations<T>(IList<T> elements)
        {
            return Enumerable.Range(0, 1 << elements.Count)
                .Select(m => Enumerable.Range(0, elements.Count)
                    .Where(i => (m & (1 << i)) != 0)
                    .Select(i => elements[i]));
        }
    }
}
