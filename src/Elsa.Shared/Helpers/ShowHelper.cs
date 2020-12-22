using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elsa.Shared
{

    public class ShowHelper
    {

        private readonly Dictionary<string, Show> ConsoleAction = new Dictionary<string, Show>(StringComparer.OrdinalIgnoreCase);

        public ShowHelper Add(string key, Func<Task> taskToShow, string description)
        {
            ConsoleAction.Add(key, new Show(taskToShow, description));
            return this;
        }

        /// <summary>
        /// Use to run desire action that user input
        /// </summary>
        /// <returns></returns>
        public async Task Run()
        {

            System.Console.WriteLine("Type the desire action to run or q to quit:");
            System.Console.WriteLine("--------------------------------------");
            foreach (var item in ConsoleAction)
            {
                System.Console.WriteLine($"\t- {item.Key}: {item.Value.Description}");
            }
            var input = System.Console.ReadLine();
            if (string.Equals(input, "q", StringComparison.OrdinalIgnoreCase))
                return;
            if (ConsoleAction.ContainsKey(input))
            {
                System.Console.Clear();
                System.Console.WriteLine("Starting the show:");
                System.Console.WriteLine("--------------------------------------");
                System.Console.WriteLine();
                await ConsoleAction[input].Task?.Invoke();
                System.Console.WriteLine();
                System.Console.WriteLine();
            }
            await Run();
        }
    }
}