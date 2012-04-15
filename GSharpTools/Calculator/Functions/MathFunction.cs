using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSharpTools.Calculator.Functions
{
    class MathFunction : Function
    {
        public delegate double FunctionCallback(double arg);

        private FunctionCallback F;

        public MathFunction(FunctionCallback f)
        {
            F = f;
        }

        public override Value Evaluate(List<Operation> args, Interpreter i)
        {
            if (args.Count != 1)
                throw new BadArgumentsError(1, args.Count);

            Value argument = new Value(args[0], i);
            switch (argument.Type)
            {
                case ValueType.Boolean:
                    argument.CastAsInteger();
                    return new Value((decimal)F((double)argument.Integer));                    

                case ValueType.Integer:
                    return new Value((decimal)F((double)argument.Integer));

                case ValueType.Decimal:
                    return new Value((decimal)F((double)argument.Decimal));

                default:
                    return null;
            }
        }       
    }
}
