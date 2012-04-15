using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GSharpTools.Calculator.Operations
{
    class IsGreaterThanOrEqual : Operation
    {
        private Operation A;
        private Operation B;

        public IsGreaterThanOrEqual(Operation a, Operation b)
        {
            A = a;
            B = b;
        }

        override public Value Evaluate(Interpreter i)
        {
            Value CastedA = new Value(A, i);
            Value CastedB = new Value(B, i);
            CastedA.CastSameType(CastedB, ValueType.Integer);
            Debug.Assert(CastedA.Type != ValueType.Boolean);
            
            switch (CastedA.Type)
            {
                case ValueType.Integer:
                    return new Value(CastedA.Integer >= CastedB.Integer);

                case ValueType.Decimal:
                    return new Value(CastedA.Decimal >= CastedB.Decimal);
                    
            }
            return CastedA;
        } 
    }
}
