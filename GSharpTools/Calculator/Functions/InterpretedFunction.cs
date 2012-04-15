using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using GSharpTools.Calculator.Operations;

namespace GSharpTools.Calculator.Functions
{
    class InterpretedFunction : Function
    {
        private readonly FunctionCall Arguments;
        private readonly Operation Code;

        public InterpretedFunction(FunctionCall a, Operation b)
        {
            Arguments = a;
            Code = b;
        }

        public override Value Evaluate(List<Operation> args, Interpreter runtime)
        {
            if (args.Count != Arguments.Args.Count)
                throw new BadArgumentsError(Arguments.Args.Count, args.Count);

            // need to setup interpreter local variables.
            Dictionary<string, Value> CopyOfVariables = new Dictionary<string, Value>();
            int ArgumentIndex = 0;
            foreach (Operation o in Arguments.Args)
            {
                Variable v = o as Variable;
                if( v == null )
                    throw new SyntaxError(0, null);

                // since it is possible that the interpreter already has these, I need to make a backup copy of the used variables
                if (runtime.Variables.ContainsKey(v.Name))
                {
                    CopyOfVariables[v.Name] = new Value(runtime.Variables[v.Name]); 
                }
                
                // at this point, we must find the matching argument and use *that* 
                runtime.Variables[v.Name] = args[ArgumentIndex].Evaluate(runtime);

                ++ArgumentIndex;
            }

            // now, call the function. all its arguments should be setup as variables!
            Value result = Code.Evaluate(runtime);

            // clean stack
            foreach (Operation o in Arguments.Args)
            {
                Variable v = o as Variable;
                Debug.Assert(v != null);

                if (CopyOfVariables.ContainsKey(v.Name))
                {
                    runtime.Variables[v.Name] = CopyOfVariables[v.Name];
                }
                else
                {
                    runtime.Variables.Remove(v.Name);
                }
            }


            return result;
        }       
    }
}
