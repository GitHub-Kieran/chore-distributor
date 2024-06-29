using ChoreDistributor.Business;
using ChoreDistributor.Data;
using ChoreDistributor.Models;

var options =
    @$"Select an option:

    1 --> Add chores
    2 --> Add people
    3 --> Distribute chores
    4 --> Display chores
    5 --> Display people
    6 --> Display distributed chores";


var isRunning = true;
while (isRunning)
{
    Console.WriteLine(options);

    var option = Console.ReadKey();
    var repo = new ChoreRepository();

    switch (option.KeyChar)
    {
        case '1':
            Console.WriteLine();
            Console.WriteLine("Enter chore name:");
            var choreName = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Enter chore weighting:");
            var choreWeighting = Console.ReadLine();
            Console.WriteLine();

            var chore = new Chore(choreName ?? string.Empty, float.Parse(choreWeighting ?? "0"));
            await repo.AddChore(chore);

            break;
        case '2':
            Console.WriteLine();
            Console.WriteLine("Enter person name:");
            var personName = Console.ReadLine();
            var person = new Person(personName ?? string.Empty);

            await repo.AddPerson(person);

            break;
        case '3':

            var chores = await repo.GetChores();
            var people = await repo.GetPeople();

            var choreDistribution = new RandomDistribution();

            var distributedChores = choreDistribution.Distribute(people, chores);

            await repo.SaveDistributedChores(distributedChores.ToList());

            Console.WriteLine();
            Console.WriteLine("Chores distributed.");

            break;
        case '4':
            Console.WriteLine();

            foreach (var c in await repo.GetChores())
            {
                Console.WriteLine(c.Name);
                Console.WriteLine(c.Weighting);
            }

            break;
        case '5':
            Console.WriteLine();

            foreach (var p in await repo.GetPeople())
            {
                Console.WriteLine(p.Name);
            }

            break;
        case '6':

            Console.WriteLine();

            var dcs = await repo.GetDistributedChores();

            foreach (var dc in dcs)
            {
                Console.WriteLine(dc.Key.Name);
                foreach (var c in dc.Value)
                {
                    Console.WriteLine(c.Name);
                }
            }

            break;
    }

    Console.WriteLine();
    Console.WriteLine("Do you want to select another option? (y/n)");

    var again = Console.ReadKey();
    if (again.KeyChar != 'y')
    {
        isRunning = false;
    }
}