using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Numeric;
using GSharpTools.Calculator.Functions;

namespace GSharpTools.Calculator.Operations
{
    class Assign : Operation
    {
        private Operation A;
        private Operation B;

        public Assign(Operation a, Operation b)
        {
            A = a;
            B = b;
        }

        override public Value Evaluate(Interpreter i)
        {
            Variable v = A as Variable;
            if (v == null)
            {
                FunctionCall f = A as FunctionCall;
                if (f == null)
                {
                    throw new SyntaxError(0, null);
                }
                i.Functions[f.Name] = new InterpretedFunction(f, B);
                return new Value(true);
            }

            Value result = new Value(B, i);
            i.Variables[v.Name] = result;
            return result;
        }
    }
    }
