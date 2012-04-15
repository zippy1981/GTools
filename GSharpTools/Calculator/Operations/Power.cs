using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GSharpTools.Calculator.Operations
{
    class Power : Operation
    {
        private Operation A;
        private Operation B;

        public Power(Operation a, Operation b)
        {
            A = a;
            B = b;
        }

        override public Value Evaluate(Interpreter i)
        {
            Value CastedA = new Value(A, i);
            Value CastedB = new Value(B, i);
            if( CastedA.Type != ValueType.Decimal )
                CastedA.CastAsDecimal();
            if( CastedB.Type != ValueType.Decimal )
                CastedB.CastAsDecimal();

            return new Value((decimal)Math.Pow((double)CastedA.Decimal, (double)CastedB.Decimal));
        }
    }

}
