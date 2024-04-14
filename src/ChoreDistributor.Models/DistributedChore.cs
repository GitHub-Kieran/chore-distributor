namespace ChoreDistributor.Models
{
    public class DistributedChore
    {
        public Person Person { get; }
        public Chore Chore { get; }
        public DistributedChore(Person person, Chore chore)
        {
            Person = person;
            Chore = chore;
        }
    }
}