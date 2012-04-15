using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GSharpTools.Calculator.Operations;

namespace GSharpTools.Calculator.Functions
{
    class ApplyOperation : Function
    {
        public delegate Operation Constructor(Operation a, Operation b);
        private readonly Constructor C;

        public ApplyOperation(Constructor c)
        {
            C = c;
        }

        public override Value Evaluate(List<Operation> args, Interpreter runtime)
        {
            if( args.Count == 0 )
                return new Value(0);

            if( args.Count == 1 )
                return args[0].Evaluate(runtime);

            Value result = C(args[0], args[1]).Evaluate(runtime);
            for (int i = 2; i < args.Count; ++i)
            {
                result = C(result, args[i]).Evaluate(runtime);
            }
            return result;
        }       
    }
}
