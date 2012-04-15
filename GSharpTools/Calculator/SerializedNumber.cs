using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace GSharpTools.Calculator
{
    [Serializable()]
    public class SerializedNumber 
    {
        public string Name;
        public object Data;
        public ValueType Type;
    }
}
