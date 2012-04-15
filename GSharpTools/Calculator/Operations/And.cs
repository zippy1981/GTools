using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GSharpTools.Calculator.Operations
{
    class And : Operation
    {
        private Operation A;
        private Operation B;

        public And(Operation a, Operation b)
        {
            A = a;
            B = b;
        }

        override public Value Evaluate(Interpreter i)
        {
            Value CastedA = new Value(A, i);
            Value CastedB = new Value(B, i);
            if( CastedA.Type != ValueType.Boolean )
                CastedA.CastAsBoolean();
            if (CastedB.Type != ValueType.Boolean)
                CastedB.CastAsBoolean();

            return new Value(CastedA.Bool & CastedB.Bool);
        }
    }

}
