using ChoreDistributor.Business.Factories;
using ChoreDistributor.Models;

namespace ChoreDistributor.Business
{
    /// <summary>
    /// Randomly choose chores for people. There is no consideration of chore weight.
    /// </summary>
    internal sealed class RandomDistribution : IChoreDistribution
    {
        private readonly IRandomFactory _randomFactory;

        public RandomDistribution(IRandomFactory randomFactory)
        {
            _randomFactory = randomFactory;
        }

        public IDictionary<Person, IList<Chore>> Distribute(IList<Person> people, IList<Chore> chores)
        {
            var distributedChores = new Dictionary<Person, IList<Chore>>();
            var random = _randomFactory.Create();

            var currentPeople = new List<Person>();
            var distributing = true;
            while (distributing)
            {
                if (currentPeople.Count == 0)
                {
                    currentPeople = new List<Person>(people);
                }

                var peopleIndex = random.Next(0, currentPeople.Count);
                var person = currentPeople[peopleIndex];
                currentPeople.RemoveAt(peopleIndex);

                var choreIndex = random.Next(0, chores.Count);
                var chore = chores[choreIndex];
                chores.RemoveAt(choreIndex);

                if (distributedChores.ContainsKey(person))
                {
                    distributedChores[person].Add(chore);
                }
                else
                {
                    distributedChores.Add(person, [chore]);
                }              

                if (chores.Count == 0)
                {
                    distributing = false;
                }
            }

            return distributedChores;
        }
    }
}
