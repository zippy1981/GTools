using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSharpTools.Calculator
{
    public abstract class Operation
    {
        public abstract Value Evaluate(Interpreter i);
    }
}
