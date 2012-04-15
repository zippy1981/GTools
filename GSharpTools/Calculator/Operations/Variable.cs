using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GSharpTools.Calculator.Operations
{
    class Variable : Operation
    {
        public readonly string Name;

        public Variable(string name)
        {
            Name = name;
        }

        override public Value Evaluate(Interpreter runtime)
        {
            // at this point, we must invoke the call context
            return runtime.LookupVariable(Name);
        } 
    }
}
