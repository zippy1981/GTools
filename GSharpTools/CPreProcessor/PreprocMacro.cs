using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSharpTools.CPreProcessor
{
    internal class PreprocMacro
    {
        public readonly bool HasArguments;            
        public readonly List<string> Arguments;
        public readonly string Definition;
        public readonly string Name;

        public PreprocMacro(bool hasArguments, string name, string args, string definition)
        {
            HasArguments = hasArguments;
            Name = name;
            Definition = definition;
            if (HasArguments)
            {
                Arguments = new List<string>();
                foreach (string arg in new PreprocTokenizer(args))
                {
                    if (arg != ",")
                        Arguments.Add(arg);
                }
            }
        }
        
        public string GetExpression()
        {
            return StringList.Join(Arguments, " ");
        }

        public void Expand(List<string> tokens, int startIndex, Dictionary<string, string> argumentValues)
        {
            StringBuilder result = new StringBuilder();
            bool insertSpace = true;
            foreach(string token in  new PreprocTokenizer(Definition))
            {
                if( token == "##" )
                {
                    insertSpace = false;
                }
                else
                {
                    if (insertSpace)
                        result.Append(" ");

                    insertSpace = true;
                    if (argumentValues.ContainsKey(token))
                    {
                        result.Append(argumentValues[token]);
                    }
                    else 
                    {                 
                        result.Append(token);
                    }
                }
            }
            result.Append(" ");

            tokens.Insert(startIndex, result.ToString());
        }
    }
}
