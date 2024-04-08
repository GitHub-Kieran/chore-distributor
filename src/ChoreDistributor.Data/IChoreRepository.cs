using ChoreDistributor.Models;

namespace ChoreDistributor.Data
{
    public interface IChoreRepository
    {
        Task<IList<Chore>> GetChores();

        Task<IList<Person>> GetPeople();

        Task<IList<DistributedChore>> GetDistributedChore();

        Task AddChore(Chore chore);

        Task AddPerson(Person person);

        Task SaveDistributedChores(IList<DistributedChore> distributedChores);
    }
}