using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elsa.Activities.Console.Activities;
using Elsa.Activities.Console.Extensions;
using Elsa.Expressions;
using Elsa.Extensions;
using Elsa.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Console.Demo
{
    class Program
    {
        static IServiceProvider services;
        static Dictionary<string, Show> ConsoleAction = new Dictionary<string, Show>(StringComparer.OrdinalIgnoreCase);
        static async Task Main(string[] args)
        {
            Configure();

            ConsoleAction.Add("hello", new Show(() => RunHelloWorld(), "Use elsa to display hello world"));
            //TO INSERT YOUR AWESOME WORFKLOW DEMO HERE 
            //1-CALL WORKFLOW FROM JSON

            //2-USE CUSTOM ACTIVITY


            ConsoleAction.Add("cls", new Show(() => Task.Run(() => System.Console.Clear()), "Clear your console"));

            await Run();
        }

        /// <summary>
        /// Demo to use elsa to invoke console activity that display an amazing text ;)
        /// </summary>
        /// <returns></returns>
        static async Task RunHelloWorld()
        {

            // Get a workflow builder.
            var workflowBuilder = services.GetRequiredService<IWorkflowBuilder>();

            // Define a workflow and add a single activity.
            var workflowDefinition = workflowBuilder
                .StartWith<WriteLine>(x => x.TextExpression = new LiteralExpression("Hello world!"))
                .Build();

            // Get a workflow invoker,
            var invoker = services.GetService<IWorkflowInvoker>();

            // Start the workflow.
            await invoker.StartAsync(workflowDefinition);
        }


        /// <summary>
        /// Use to run desire action that user input
        /// </summary>
        /// <returns></returns>
        static async Task Run()
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


        /// <summary>
        /// Dependency injection configuration
        /// </summary>
        static void Configure()
        {
            // Setup a service collection.
            services = new ServiceCollection()

            // Add essential workflow services.
            .AddElsa()

            // Add Console activities (ReadLine and WriteLine).
            .AddConsoleActivities()

            .BuildServiceProvider();
        }






        internal class Show
        {

            public Show(Func<Task> task, string description)
            {
                Task = task;
                Description = description;
            }

            public string Description { get; }

            public Func<Task> Task { get; }
        }

    }
}
