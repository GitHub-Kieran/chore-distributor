using ChoreDistributor.Console;

await ConsoleMenu.PrintOptionsMenu();

var serviceProvider = Startup.RegisterServices();
await ConsoleMenu.Run(serviceProvider);