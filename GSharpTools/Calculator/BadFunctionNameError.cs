using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSharpTools.Calculator
{
    public class BadFunctionNameError : Exception
    {
        public BadFunctionNameError(string name)
            : base(string.Format("No function named '{0}' known.", name)) { }
    }
}
