using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSharpTools.Calculator
{
    public abstract class Function
    {
        public abstract Value Evaluate(List<Operation> args, Interpreter i);
    }
}
