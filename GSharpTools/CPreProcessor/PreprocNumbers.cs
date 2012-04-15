using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using GSharpTools.Calculator;
using System.Runtime.Serialization.Formatters.Binary;

namespace GSharpTools.CPreProcessor
{
    public class PreprocNumbers
    {
        public int Compile(string filename, CPreProcessor processor)
        {
            Numbers = new List<SerializedNumber>();
            BuildSerializedNumbers(processor);
            return CompileNumbers(filename);
        }

        private int CompileNumbers(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                // Serialize the object to the file
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, Numbers);
                fs.Close();
            }
            return Numbers.Count;
        }

        private void BuildSerializedNumbers(CPreProcessor processor)
        {
            Interpreter runtime = new Interpreter();
            foreach (string key in processor.PreProcMacros.Keys)
            {
                PreprocMacro m = processor.PreProcMacros[key];
                if (m.Definition != null)
                {
                    // hack.
                    string definition = StripEverything(m.Definition);
                    if (definition != "")
                    {
                        Parser p = new Parser();
                        Operation node = p.Parse(definition);
                        if (node != null)
                        {
                            runtime.Reset();
                            Value v = node.Evaluate(runtime);
                            if (v != null)
                            {
                                SerializedNumber n = v.Serialized;
                                if (n != null)
                                {
                                    n.Name = key;
                                    Numbers.Add(n);
                                    Trace.TraceInformation("Define {0} as {1}.", key, n);
                                }
                            }
                            else
                            {
                                Trace.TraceError("Unable to evaluate: {0} [{1}]", definition, m.Definition);
                            }
                        }
                        else
                        {
                            Trace.TraceError("Unable to parse: {0} [{1}]", definition, m.Definition);
                        }
                    }
                }
            }
        }

        private string StripEverything(string text)
        {
            StringBuilder result = new StringBuilder();
            foreach (char c in text)
            {
                if ("0123456789xX()+-*/<>~!|&^".IndexOf(c) >= 0)
                {
                    result.Append(c);
                }
            }
            String s = result.ToString();
            s = s.Replace("()", "");
            s = s.ToLower();
            if (s == "x" || s == "xx" || s == "(x)")
                return "";
            return s;
        }

        private List<SerializedNumber> Numbers;
    }
}
