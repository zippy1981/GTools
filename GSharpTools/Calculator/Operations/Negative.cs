using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Numeric;

namespace GSharpTools.Calculator.Operations
{
    class Negative : Operation
    {
        private Operation A;

        public Negative(Operation a)
        {
            A = a;
        }

        override public Value Evaluate(Interpreter i)
        {
            Value CastedA = new Value(A, i);
            if (CastedA.Type == ValueType.Boolean)
                CastedA.CastAsInteger();

            switch (CastedA.Type)
            {
                case ValueType.Integer:
                    return new Value(-CastedA.Integer);

                case ValueType.Decimal:
                    return new Value(-CastedA.Decimal);
            }
            Debug.Assert( false );
            return CastedA;
        }
    }
}
