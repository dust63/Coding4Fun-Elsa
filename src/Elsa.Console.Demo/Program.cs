using System;
using System.Threading.Tasks;
using Elsa.Activities.Console.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Elsa.Shared;

using Elsa.Activities.ControlFlow.Extensions;

namespace Elsa.Console.Demo
{
    class Program
    {
        static IServiceProvider services;

        static async Task Main(string[] args)
        {
            Configure();
            //To help us you can go https://github.com/elsa-workflows/elsa-core/tree/master/src/samples
            await new ShowHelper()
            //1 Simple hello world
            .Add("hello", () => HelloWorldDemo.Run(services), "Use elsa to display hello world. Awesome code ;)")

            //2-USE CUSTOM ACTIVITY
            .Add("calculator", () => CalculatorDemo.Run(services), "Run calculator with custom activity")

            .Add("cls", () => Task.Run(() => System.Console.Clear()), "Clear your console")
            .Run();
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
            .AddConsoleActivities()
            .AddControlFlowActivities();


            definitions.AddActivity<CalculatorActivity>();

            services = definitions.BuildServiceProvider();
        }





    }
}
