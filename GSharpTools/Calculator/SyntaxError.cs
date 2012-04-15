using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSharpTools.Calculator
{
    public class SyntaxError : Exception
    {
        public SyntaxError(int readpos, string context) : base(
            (context == null) ? string.Format("Invalid Expression at position {0}", readpos) :
                string.Format("Invalid Expression at '{0}'", context.Substring(readpos)))
        {
        }
    }
}
