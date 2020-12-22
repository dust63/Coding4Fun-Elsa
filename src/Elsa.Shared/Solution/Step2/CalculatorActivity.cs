using System.Threading;
using System.Threading.Tasks;
using Elsa;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Results;
using Elsa.Services;
using Elsa.Services.Models;

namespace Elsa.Shared
{
    [ActivityDefinition(
    DisplayName = "Calcualtor",
    Description = "Make some arythmetic operation",
    Icon = "fas fa-calculator",
    Outcomes = new[] { OutcomeNames.Done })]
    public class CalculatorActivity : Activity
    {
        private IWorkflowExpressionEvaluator _expressionEvaluator;


        public CalculatorActivity(IWorkflowExpressionEvaluator expressionEvaluator)
        {
            _expressionEvaluator = expressionEvaluator;
        }

        [ActivityProperty(Hint = "First value")]
        public WorkflowExpression<int> Value1
        {
            get => GetState<WorkflowExpression<int>>();
            set => SetState(value);
        }

        [ActivityProperty(Hint = "Second value")]
        public WorkflowExpression<int> Value2
        {
            get => GetState<WorkflowExpression<int>>();
            set => SetState(value);
        }

        [ActivityProperty(Hint = "Operator value use int value. (1) Add | (2) Substract | (3) Multiply | (4) Divide")]
        public WorkflowExpression<int> Operator
        {
            get => GetState<WorkflowExpression<int>>();
            set => SetState(value);
        }



        protected override async Task<ActivityExecutionResult> OnExecuteAsync(
                 WorkflowExecutionContext context,
                 CancellationToken cancellationToken)
        {
            var value1 = await _expressionEvaluator.EvaluateAsync(Value1, typeof(int), context, cancellationToken);
            var value2 = await _expressionEvaluator.EvaluateAsync(Value2, typeof(int), context, cancellationToken);
            var operatorCalcul = await _expressionEvaluator.EvaluateAsync(Operator, typeof(int), context, cancellationToken);
            var calculator = new Calculator((int)value1, (int)value2);


            Output.SetVariable("Result", calculator.Calculate((Operator)operatorCalcul));

            return Done();
        }
    }
}