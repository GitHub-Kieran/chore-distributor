using ChoreDistributor.Models;

namespace ChoreDistributor.Business
{
    public sealed class RandomDistribution : IChoreDistribution
    {
        // TODO: Person is duplicated. Remove the duplication that occurs when creating a distributed chore for memory efficiency, and displaying ease
        // e.g. DistributedChores(Person, Chores) or dictionary instead
        public IList<DistributedChore> Distribute(IList<Person> people, IList<Chore> chores)
        {
            var distributedChores = new List<DistributedChore>();
            var random = new Random();

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

                var distributedChore = new DistributedChore(person, chore);
                distributedChores.Add(distributedChore);                               

                if (chores.Count == 0)
                {
                    distributing = false;
                }
            }

            return distributedChores;
        }
    }
}
