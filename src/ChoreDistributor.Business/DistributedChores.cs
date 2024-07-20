using ChoreDistributor.Models;

namespace ChoreDistributor.Business
{
    internal class DistributedChores : Dictionary<Person, IList<Chore>>, IDictionary<Person, IList<Chore>>
    {
        public DistributedChores Randomise(Random random)
        {
            var shuffledChores = Values.OrderBy(_ => random.Next(0, Values.Count)).ToList();

            for (int i = 0; i < Keys.Count; i++)
            {
                this[Keys.ElementAt(i)] = shuffledChores[i];
            }
            return this;
        }
    }
}
