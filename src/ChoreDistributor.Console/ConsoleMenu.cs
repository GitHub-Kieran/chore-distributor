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

        private static readonly string __distributionOptionsMenu =
            @$"Select an option:

            1 --> Linear
            2 --> Random
            3 --> Equal
            4 --> Income";

        private static readonly Dictionary<char, Func<Task>> __options = [];
        private static readonly Dictionary<char, ChoreDistributors> __distributionOptions = [];

        private static void InitialiseOptions(IServiceProvider serviceProvider)
        {
            var choreRepository = serviceProvider.GetRequiredService<IChoreRepository>();
            __options['1'] = async () => 
            {
                await Console.Out.WriteLineAsync();
                await Console.Out.WriteLineAsync("Enter chore name:");
                var choreName = await Console.In.ReadLineAsync();

                await Console.Out.WriteLineAsync();
                await Console.Out.WriteLineAsync("Enter chore weighting:");
                var choreWeighting = await Console.In.ReadLineAsync();

                var chore = new Chore(choreName ?? string.Empty, float.Parse(choreWeighting ?? "0"));
                await choreRepository.AddChore(chore);
            };
            __options['2'] = async () =>
            {
                await Console.Out.WriteLineAsync();
                await Console.Out.WriteLineAsync("Enter persons name:");
                var personName = await Console.In.ReadLineAsync();

                await Console.Out.WriteLineAsync();
                await Console.Out.WriteLineAsync("Enter persons income:");
                var income = await Console.In.ReadLineAsync();

                var person = new Person(personName ?? string.Empty, string.IsNullOrEmpty(income) ? 0 : float.TryParse(income ?? "0", out var parsedIncome) ? parsedIncome : 0);
                await choreRepository.AddPerson(person);
            };
            __options['3'] = async () =>
            {
                var chores = await choreRepository.GetChores();
                var people = await choreRepository.GetPeople();

                await Console.Out.WriteLineAsync();
                await Console.Out.WriteLineAsync("What distribution method would you like to use?");
                await Console.Out.WriteLineAsync(__distributionOptionsMenu);

                var distributionOptionKey = Console.ReadKey();

                if (__distributionOptions.TryGetValue(distributionOptionKey.KeyChar, out var option))
                {
                    var choreDistribution = serviceProvider.GetRequiredKeyedService<IChoreDistribution>(option);

                    var distributedChores = choreDistribution.Distribute(people, chores);

                    await choreRepository.SaveDistributedChores(distributedChores.ToList());

                    await Console.Out.WriteLineAsync();
                    await Console.Out.WriteLineAsync("Chores distributed.");
                }
                else
                {
                    await Console.Out.WriteLineAsync();
                    await Console.Out.WriteLineAsync("Invalid selection.");
                }
            };
            __options['4'] = async () =>
            {
                await Console.Out.WriteLineAsync();
                foreach (var chore in await choreRepository.GetChores())
                {
                    await Console.Out.WriteLineAsync($"[Chore : '{chore.Name}' - Weight: '{chore.Weighting}']");
                }

            };
            __options['5'] = async () =>
            {
                await Console.Out.WriteLineAsync();
                foreach (var person in await choreRepository.GetPeople())
                {
                    await Console.Out.WriteLineAsync($"[Person : '{person.Name}' - Income: '{person.Income}']");
                }

            };
            __options['6'] = async () =>
            {
                await Console.Out.WriteLineAsync();
                foreach (var distributedChore in await choreRepository.GetDistributedChores())
                {
                    await Console.Out.WriteLineAsync($"Person : '{distributedChore.Key.Name}'");
                    foreach (var chore in distributedChore.Value)
                    {
                        await Console.Out.WriteLineAsync($"[Chore : '{chore.Name}' - Weight: '{chore.Weighting}']");
                    }
                }
            };
        }

        private static void InitialiseDistributionOptions()
        {
            __distributionOptions.Add('1', ChoreDistributors.Linear);
            __distributionOptions.Add('2', ChoreDistributors.Random);
            __distributionOptions.Add('3', ChoreDistributors.Equal);
            __distributionOptions.Add('4', ChoreDistributors.Income);
        }

        public static async Task PrintOptionsMenu()
        {
            await Console.Out.WriteLineAsync();
            await Console.Out.WriteLineAsync(__optionsMenu);
        }

        public static async Task Run(IServiceProvider serviceProvider)
        {
            InitialiseOptions(serviceProvider);
            InitialiseDistributionOptions();

            var isRunning = true;
            while (isRunning)
            {
                var optionKey = Console.ReadKey();
                if (__options.TryGetValue(optionKey.KeyChar, out var option))
                {
                    await option();

                    await Console.Out.WriteLineAsync();
                    await Console.Out.WriteLineAsync("Do you want to select another option? (y/n)");

                    var chooseAgain = Console.ReadKey();
                    if (chooseAgain.KeyChar == 'n')
                    {
                        isRunning = false;
                    }

                    await PrintOptionsMenu();
                }
                else
                {
                    await Console.Out.WriteLineAsync();
                    await Console.Out.WriteLineAsync("Invalid selection.");
                }
            }
        }
    }
}
