namespace ChoreDistributor.Models
{
    public record struct Person(string Name)
    {
        public float Income { get; set; }
    };
}