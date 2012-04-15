using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GSharpTools.Calculator.Operations
{
    class FunctionCall : Operation
    {
        public readonly List<Operation> Args = new List<Operation>();
        public readonly string Name;

        public FunctionCall(string name, List<Operation> args)
        {
            Args = args;
            Name = name;
        }

        override public Value Evaluate(Interpreter runtime)
        {
            // at this point, we must invoke the call context
            return runtime.LookupFunction(Name).Evaluate(Args, runtime);
        } 
    }
}
