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

            var enrichedPeople = new List<EnrichedPerson>();
            foreach (var person in people) // TODO: Do we need this loop?
            {
                var percentageContribution = person.Income / totalIncome;
                var choreContribution = totalWeight * (1 - percentageContribution);

                if (percentageContribution * people.Count == 1)
                {
                    // Its equal and we need to randomise closest matches
                }

                enrichedPeople.Add(new EnrichedPerson(person, choreContribution));
            }
            var closestMatches = new List<List<Chore>>();

            // TODO: refactor into EqualDistribution code
            var remainingChores = chores;
            for (int i = 0; i < enrichedPeople.Count; i++)
            {
                var index = 0;
                var indexes = remainingChores.Select(c => index++).ToList();

                var linqCombinations = GetCombinations(indexes);

                var bestCombination = new List<Chore>();
                var minDifference = float.MaxValue;

                foreach (var combination in linqCombinations)
                {
                    var combinationWeight = combination.Sum(index => remainingChores[index].Weighting);
                    var difference = Math.Abs(combinationWeight - enrichedPeople[i].ChoreContribution);

                    if (difference <= minDifference)
                    {
                        minDifference = difference;
                        bestCombination = combination.Select(index => remainingChores[index]).ToList();
                    }
                }                             
                remainingChores = remainingChores.Except(bestCombination).ToList();

                distributedChores.Add(enrichedPeople[i].Person, bestCombination);
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

        private class EnrichedPerson
        {
            public Person Person { get; }
            public float ChoreContribution { get; set; }

            public EnrichedPerson(Person person, float choreContribution)
            {
                Person = person;
                ChoreContribution = choreContribution;
            }
        }
    }
}
