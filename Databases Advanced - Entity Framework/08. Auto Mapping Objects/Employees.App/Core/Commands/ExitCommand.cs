namespace Employees.App.Core.Commands
{
    using System;
    using System.Threading;

    using Contracts;

    public class ExitCommand : ICommand
    {
        public string Execute(string[] args)
        {
            for (int i = 3; i >= 1; i--)
            {
                Console.WriteLine($"Program will close in {i} seconds");
                Thread.Sleep(1000);
            }

            Environment.Exit(0);

            return null;
        }
    }
}
