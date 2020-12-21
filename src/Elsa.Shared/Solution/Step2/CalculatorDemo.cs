using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elsa.Models;
using Elsa.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Shared
{

    public static class CalculatorDemo
    {
        public static async Task Run(IServiceProvider services)
        {
            // Get a workflow invoker,
            var invoker = services.GetService<IWorkflowInvoker>();

            // Start the workflow.
            var executionContext = await invoker.StartAsync<CalculatorWorkflow>();

            while (executionContext.Workflow.Status != WorkflowStatus.Finished)
            {
                // Print current execution log + blocking activities to visualize current workflow state.
                //DisplayWorkflowState(executionContext.Workflow);

                var textInput = System.Console.ReadLine();
                var input = new Variables(new Dictionary<string, object>() { ["ReadLineInput"] = int.Parse(textInput) });

                executionContext.Workflow.Input = input;
                executionContext = await invoker.ResumeAsync(executionContext.Workflow, executionContext.Workflow.BlockingActivities);
            }
        }
    }

}