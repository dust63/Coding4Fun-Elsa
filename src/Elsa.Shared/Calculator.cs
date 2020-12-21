using System;

namespace Elsa.Shared
{
    public class Calculator
    {

        public Calculator(int value1, int value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public int Value1 { get; }

        public int Value2 { get; }



        public int Calculate(Operator operatorCalcul)
        {
            switch (operatorCalcul)
            {
                case Operator.Add:
                    return Value1 + Value2;
                case Operator.Substract:
                    return Value1 - Value2;
                case Operator.Multiply:
                    return Value1 * Value2;
                case Operator.Divide:
                    return Value1 / Value2;
                default:
                    throw new NotImplementedException();
            }

        }
    }

    public enum Operator
    {

        Add = 1,
        Substract = 2,
        Multiply = 3,
        Divide = 4
    }

}