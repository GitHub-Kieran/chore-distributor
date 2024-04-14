using ChoreDistributor.Models;

namespace ChoreDistributor.Business
{
    public interface IChoreDistribution
    {
        IList<DistributedChore> Distribute(IList<Person> people, IList<Chore> chores);
    }
}