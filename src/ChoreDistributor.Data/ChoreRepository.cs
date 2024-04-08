using ChoreDistributor.Models;
using System.Text.Json;

namespace ChoreDistributor.Data
{
    public class ChoreRepository : IChoreRepository
    {
        public async Task<IList<Chore>> GetChores()
        {
            var chores = new List<Chore>();
            await using (var readStream = File.Open("chores.json", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                await foreach (var item in JsonSerializer.DeserializeAsyncEnumerable<Chore>(readStream))
                {
                    chores.Add(item);
                }
            }
            return chores;
        }

        public async Task<IList<Person>> GetPeople()
        {
            var person = new List<Person>();
            await using (var readStream = File.Open("people.json", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                await foreach (var item in JsonSerializer.DeserializeAsyncEnumerable<Person>(readStream))
                {
                    person.Add(item);
                }
            }
            return person;
        }

        public async Task<IList<DistributedChore>> GetDistributedChore()
        {
            var distributedChores = new List<DistributedChore>();
            await using (var readStream = File.Open("distributedChores.json", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                await foreach (var item in JsonSerializer.DeserializeAsyncEnumerable<DistributedChore>(readStream))
                {
                    distributedChores.Add(item ?? throw new NullReferenceException(nameof(item)));
                }
            }
            return distributedChores;
        }

        public async Task AddChore(Chore chore)
        {
            var chores = await GetChores();
            chores.Add(chore);

            await using (var fs = File.Open("chores.json", FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                await JsonSerializer.SerializeAsync(fs, chores);
            }
        }

        public async Task AddPerson(Person person)
        {
            var people = await GetPeople();
            people.Add(person);

            await using (var fs = File.Open("people.json", FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                await JsonSerializer.SerializeAsync(fs, people);
            }
        }

        public async Task SaveDistributedChores(IList<DistributedChore> distributedChores)
        {
            await using (var fs = File.Open("distributedChores.json", FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                await JsonSerializer.SerializeAsync(fs, distributedChores);
            }
        }
    }
}
