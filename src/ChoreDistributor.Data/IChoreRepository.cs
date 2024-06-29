using ChoreDistributor.Models;

namespace ChoreDistributor.Data
{
    public interface IChoreRepository
    {
        Task<IList<Chore>> GetChores();

        Task<IList<Person>> GetPeople();

        Task<IList<KeyValuePair<Person, IList<Chore>>>> GetDistributedChores();

        Task AddChore(Chore chore);

        Task AddPerson(Person person);

        Task SaveDistributedChores(IList<KeyValuePair<Person, IList<Chore>>> distributedChores);
    }
}