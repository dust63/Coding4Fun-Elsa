using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Elsa.Activities.Console.Activities;
using Elsa.Activities.Console.Extensions;
using Elsa.Expressions;
using Elsa.Extensions;
using Elsa.Services;
using Microsoft.Extensions.DependencyInjection;
using Elsa.Shared;
using Elsa.Models;
using Elsa.Services.Models;
using System.Data;
using System.Linq;

namespace Elsa.Console.Demo
{
    class Program
    {
        static IServiceProvider services;
        static Dictionary<string, Show> ConsoleAction = new Dictionary<string, Show>(StringComparer.OrdinalIgnoreCase);
        static async Task Main(string[] args)
        {
            Configure();

            ConsoleAction.Add("hello", new Show(() => RunHelloWorld(), "Use elsa to display hello world. Awesome code ;)"));
            ConsoleAction.Add("test-json", new Show(() => TestJson(), "Test to read json workflow"));
            ConsoleAction.Add("calculator", new Show(() => CalculatorDemo.Run(services), "Run calculator with custom activity"));
            //TODO INSERT YOUR AWESOME WORFKLOW DEMO HERE 
            //1-CALL WORKFLOW FROM JSON

            //2-USE CUSTOM ACTIVITY


            ConsoleAction.Add("cls", new Show(() => Task.Run(() => System.Console.Clear()), "Clear your console"));

            await Run();
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

        private static async Task TestJson()
        {
            var json = await ReadJsonWorkflowResource("calculator.json");
            System.Console.WriteLine(json);
        }
        private static async Task<string> ReadJsonWorkflowResource(string resourceName)
        {
            var assembly = typeof(Program).Assembly;
            var resourcePath = $"{typeof(Program).Assembly.GetName().Name}.workflows.{resourceName}";
            using var reader = new StreamReader(assembly.GetManifestResourceStream(resourcePath));
            return await reader.ReadToEndAsync();
        }

        /// <summary>
        /// Dependency injection configuration
        /// </summary>
        static void Configure()
        {
            // Setup a service collection.
            var definitions = new ServiceCollection();
            definitions
            .AddElsa()
            .AddConsoleActivities();

            definitions.AddActivity<CalculatorActivity>();

            services = definitions.BuildServiceProvider();
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
