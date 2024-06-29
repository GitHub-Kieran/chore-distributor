using ChoreDistributor.Models;
using System;

namespace ChoreDistributor.Business
{
    /// <summary>
    /// Go through chores linearly until the distributed chores total weight for each person
    /// meets or overtakes the average expected weight
    /// </summary>
    public class LinearDistribution : IChoreDistribution
    {
        public IDictionary<Person, IList<Chore>> Distribute(IList<Person> people, IList<Chore> chores)
        {
            var distributedChores = new Dictionary<Person, IList<Chore>>();

            var total = chores.Sum(c => c.Weighting);

            var averageDistribtion = total / people.Count;

            var x = 0;

            foreach (var person in people)
            {
                var personTotalWeight = 0f;

                for (var j = x; j < chores.Count; ++j)
                {
                    var chore = chores[j];
                    var weight = chore.Weighting;
                    personTotalWeight += weight;

                    if (distributedChores.ContainsKey(person))
                    {
                        distributedChores[person].Add(chore);
                    }
                    else
                    {
                        distributedChores.Add(person, new List<Chore> { chore });
                    }

                    if (personTotalWeight >= averageDistribtion)
                    {
                        x = ++j;
                        break;
                    }
                }
            }

            return distributedChores;
        }
    }
}