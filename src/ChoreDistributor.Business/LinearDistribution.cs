using ChoreDistributor.Models;

namespace ChoreDistributor.Business
{
    /// <summary>
    /// Loop through all the chores in order and assign them to people in order of occurance.
    /// This method of chore distribution can result in unfair distribution if the chores are not already ordered in the preffered way.
    /// </summary>
    public sealed class LinearDistribution : IChoreDistribution
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