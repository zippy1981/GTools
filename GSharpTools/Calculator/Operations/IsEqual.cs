using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GSharpTools.Calculator.Operations
{
    class IsEqual : Operation
    {
        private Operation A;
        private Operation B;

        public IsEqual(Operation a, Operation b)
        {
            A = a;
            B = b;
        }

        override public Value Evaluate(Interpreter i)
        {
            Value CastedA = new Value(A, i);
            Value CastedB = new Value(B, i);
            CastedA.CastSameType(CastedB);
            
            switch (CastedA.Type)
            {
                case ValueType.Boolean:
                    return new Value(CastedA.Bool == CastedB.Bool);

                case ValueType.Integer:
                    return new Value(CastedA.Integer == CastedB.Integer);

                case ValueType.Decimal:
                    return new Value(CastedA.Decimal == CastedB.Decimal);
                    
            }
            return CastedA;
        } 
    }
}
