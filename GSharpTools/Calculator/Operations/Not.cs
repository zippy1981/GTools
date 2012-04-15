using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Numeric;

namespace GSharpTools.Calculator.Operations
{
    class Not : Operation
    {
        private Operation A;

        public Not(Operation a)
        {
            A = a;
        }

        override public Value Evaluate(Interpreter i)
        {
            Value CastedA = new Value(A, i);
            if (CastedA.Type != ValueType.Boolean)
                CastedA.CastAsBoolean();

            return new Value(!CastedA.Bool);
        }
    }
}
