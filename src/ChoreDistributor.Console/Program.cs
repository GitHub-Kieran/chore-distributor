using ChoreDistributor.Console;

ConsoleMenu.PrintOptionsMenu();

var serviceProvider = Startup.RegisterServices();
ConsoleMenu.Run(serviceProvider);