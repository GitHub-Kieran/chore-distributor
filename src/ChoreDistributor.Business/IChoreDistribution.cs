using ChoreDistributor.Models;

namespace ChoreDistributor.Business
{
    public interface IChoreDistribution
    {
        IDictionary<Person, IList<Chore>> Distribute(IList<Person> people, IList<Chore> chores);
    }
}