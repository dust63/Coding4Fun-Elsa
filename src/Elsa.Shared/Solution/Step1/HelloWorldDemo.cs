using System;
using System.Threading.Tasks;
using Elsa.Activities.Console.Activities;
using Elsa.Activities.Console.Extensions;
using Elsa.Expressions;
using Elsa.Extensions;
using Elsa.Services;
using Microsoft.Extensions.DependencyInjection;
using Elsa.Models;
using Elsa.Services.Models;


namespace Elsa.Shared
{
    public static class  HelloWorldDemo
    {
        
        /// <summary>
        /// Demo to use elsa to invoke console activity that display an amazing text ;)
        /// </summary>
        /// <returns></returns>
        public static async Task Run(IServiceProvider services)
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
    }
}