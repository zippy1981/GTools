using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSharpTools.Calculator
{
    public class BadArgumentsError : Exception
    {
        public BadArgumentsError(int argsExpected, int argsReceived) : base(
            string.Format("Expected {0} args, got {1} instead.", argsExpected, argsReceived)) { }
    }
}
