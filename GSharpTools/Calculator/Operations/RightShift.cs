using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Numeric;

namespace GSharpTools.Calculator.Operations
{
    class RightShift : Operation
    {
        private Operation A;
        private Operation B;

        public RightShift(Operation a, Operation b)
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

            return new Value((long)((int) CastedA.Integer >> (int) CastedB.Integer));
        }
    }
    }
