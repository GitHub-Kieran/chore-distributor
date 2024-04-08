using ChoreDistributor.Models;

namespace ChoreDistributor.Business
{
    public class LinearDistribution : IChoreDistribution
    {
        public IList<DistributedChore> Distribute(IList<Person> people, IList<Chore> chores)
        {
            var distributedChores = new List<DistributedChore>();

            var total = chores.Sum(c => c.Weighting);

            var averageDistribtion = total / people.Count;

            var x = 0;

            foreach (var p in people)
            {
                var personTotalWeight = 0f;

                for (var j = x; j < chores.Count; ++j)
                {
                    var c = chores[j];
                    var weight = c.Weighting;
                    personTotalWeight += weight;

                    var distributedChore = new DistributedChore(p, c);

                    distributedChores.Add(distributedChore);

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