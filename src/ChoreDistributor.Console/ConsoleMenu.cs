using ChoreDistributor.Business;
using ChoreDistributor.Business.Factories;
using ChoreDistributor.Data;
using ChoreDistributor.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ChoreDistributor.Console
{
    using System;

    internal static class ConsoleMenu
    {
        private static readonly string __optionsMenu =
            @$"Select an option:

            1 --> Add chores
            2 --> Add people
            3 --> Distribute chores
            4 --> Display chores
            5 --> Display people
            6 --> Display distributed chores";

        private static readonly Dictionary<char, Action> __options = [];

        private static void InitialiseOptions(IServiceProvider serviceProvider)
        {
            var choreRepository = serviceProvider.GetRequiredService<IChoreRepository>();
            __options['1'] = async () => 
            {
                Console.WriteLine();
                Console.WriteLine("Enter chore name:");
                var choreName = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine("Enter chore weighting:");
                var choreWeighting = Console.ReadLine();
                Console.WriteLine();

                var chore = new Chore(choreName ?? string.Empty, float.Parse(choreWeighting ?? "0"));
                await choreRepository.AddChore(chore);
            };
            __options['2'] = async () =>
            {
                Console.WriteLine();
                Console.WriteLine("Enter person name:");
                var personName = Console.ReadLine();
                var person = new Person(personName ?? string.Empty);

                await choreRepository.AddPerson(person);
            };
            __options['3'] = async () =>
            {
                var chores = await choreRepository.GetChores();
                var people = await choreRepository.GetPeople();

                var choreDistribution = serviceProvider.GetRequiredKeyedService<IChoreDistribution>(ChoreDistributors.Income);

                var distributedChores = choreDistribution.Distribute(people, chores);

                await choreRepository.SaveDistributedChores(distributedChores.ToList());

                Console.WriteLine();
                Console.WriteLine("Chores distributed.");
            };
            __options['4'] = async () =>
            {
                Console.WriteLine();

                foreach (var c in await choreRepository.GetChores())
                {
                    Console.WriteLine(c.Name);
                    Console.WriteLine(c.Weighting);
                }

            };
            __options['5'] = async () =>
            {
                Console.WriteLine();

                foreach (var p in await choreRepository.GetPeople())
                {
                    Console.WriteLine(p.Name);
                }

            };
            __options['6'] = async () =>
            {
                Console.WriteLine();

                var dcs = await choreRepository.GetDistributedChores();

                foreach (var dc in dcs)
                {
                    Console.WriteLine($"Person : '{dc.Key.Name}'");
                    foreach (var chore in dc.Value)
                    {
                        Console.WriteLine($"Chore : '{chore.Name}' -- Weight: '{chore.Weighting}'");
                    }
                }
            };
        }

        public static void PrintOptionsMenu()
        {
            Console.WriteLine();
            Console.WriteLine(__optionsMenu);
        }

        public static void Run(IServiceProvider serviceProvider)
        {
            InitialiseOptions(serviceProvider);

            var isRunning = true;
            while (isRunning)
            {
                var optionKey = Console.ReadKey();
                if (__options.TryGetValue(optionKey.KeyChar, out Action option))
                {
                    option();

                    Console.WriteLine();
                    Console.WriteLine("Do you want to select another option? (y/n)");

                    var chooseAgain = Console.ReadKey();
                    if (chooseAgain.KeyChar != 'y')
                    {
                        isRunning = false;
                    }

                    PrintOptionsMenu();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid selection.");
                }
            }
        }
    }
}
