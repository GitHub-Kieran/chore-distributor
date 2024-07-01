using ChoreDistributor.Business.Extensions;
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
        public IDictionary<Person, IList<Chore>> Distribute(IList<Person> people, IList<Chore> chores)
        {
            var distributedChores = new Dictionary<Person, IList<Chore>>();

            var random = new Random(); // TODO: Make random testable by using an interface for normal and seeded versions (unit testing)
            
            var total = chores.Sum(c => c.Weighting);

            var averageDistribtion = total / people.Count;

            var closestMatches = new List<List<Chore>>();
                
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

                closestMatches.Add(bestCombination);
            }

            // TODO: change this so that bestCombination is added to distributedChores like
            // distributedChores.Add(people[i], bestCombination); and randomise distributedChores instead
            var distributing = true;
            var peopleCopy = new List<Person>(people);
            while (distributing)
            {
                var peopleIndex = random.Next(0, peopleCopy.Count -1);
                var person = peopleCopy[peopleIndex];
                peopleCopy.RemoveAt(peopleIndex);

                var closestMatchesIndex = random.Next(0, closestMatches.Count - 1); 
                var closestMatch = closestMatches[closestMatchesIndex];
                closestMatches.RemoveAt(closestMatchesIndex);

                distributedChores.Add(person, closestMatch);

                if (closestMatches.Count == 0)
                {
                    distributing = false;
                }
            }

            return distributedChores;
        }
    }
}
