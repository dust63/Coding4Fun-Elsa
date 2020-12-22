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

        static async Task Main(string[] args)
        {
            Configure();
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
            .AddConsoleActivities();

            definitions.AddActivity<CalculatorActivity>();

            services = definitions.BuildServiceProvider();
        }





    }
}
