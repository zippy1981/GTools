using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GSharpTools.Calculator.Operations
{
    class BitwiseAnd : Operation
    {
        private Operation A;
        private Operation B;

        public BitwiseAnd(Operation a, Operation b)
        {
            A = a;
            B = b;
        }

        override public Value Evaluate(Interpreter i)
        {
            Value CastedA = new Value(A, i);
            Value CastedB = new Value(B, i);
            if (CastedA.Type != ValueType.Integer)
                CastedA.CastAsInteger();
            if (CastedB.Type != ValueType.Integer)
                CastedB.CastAsInteger();

            return new Value(CastedA.Integer | CastedB.Integer);
        }
    }

}
