using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Numeric;

namespace GSharpTools.Calculator.Operations
{
    class BitwiseNegative : Operation
    {
        private Operation A;

        public BitwiseNegative(Operation a)
        {
            A = a;
        }

        override public Value Evaluate(Interpreter i)
        {
            Value CastedA = new Value(A, i);
            if (CastedA.Type != ValueType.Integer)
                CastedA.CastAsInteger();

            return new Value(~CastedA.Integer);
        }
    }
}
