using Elsa.Activities.Console.Activities;
using Elsa.Expressions;
using Elsa.Scripting.JavaScript;
using Elsa.Services;
using Elsa.Services.Models;


namespace Elsa.Shared
{
    public class CalculatorWorkflow : IWorkflow
    {
        public void Build(IWorkflowBuilder builder)
        {
            builder
               .StartWith<WriteLine>(x => x.TextExpression = new LiteralExpression("Enter value1 (number only):"))
               .Then<ReadLine>().WithName("Value1")
               .Then<WriteLine>(x => x.TextExpression = new LiteralExpression("Enter value2 (number only):"))
               .Then<ReadLine>().WithName("Value2")
               .Then<WriteLine>(x => x.TextExpression = new LiteralExpression("Enter operator \r\n\t(1)Add\r\n\t(2)Substract\r\n\t(3)Multiply\r\n\t(4)Divide"))
               .Then<ReadLine>().WithName("Operator")
               .Then<CalculatorActivity>(
                   x =>
                   {
                       x.Value1 = new JavaScriptExpression<int>("Value1.Input");
                       x.Value2 = new JavaScriptExpression<int>("Value2.Input");
                       x.Operator = new JavaScriptExpression<int>("Operator.Input");
                   }).WithName("Calculator")
               .Then<WriteLine>(x => x.TextExpression = new JavaScriptExpression<string>("`Result is: \"${Calculator.Result}\"`")).WithName("WriteResult");
        }
    }
}