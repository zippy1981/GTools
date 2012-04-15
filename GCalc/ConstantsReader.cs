using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using GSharpTools.Calculator;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace GCalc
{
    class ConstantsReader
    {
        public bool Read(string filename)
        {
            if (File.Exists(filename))
            {
                List<SerializedNumber> Numbers = null;

                // Open the file containing the data that you want to deserialize.
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    Numbers = (List<SerializedNumber>)formatter.Deserialize(fs);
                    fs.Close();
                }
                /*
                foreach (SerializedNumber sn in Numbers)
                {
                    Parser.LookupByName[sn.Name] = sn.Value;
                    if (Parser.LookupByValue.ContainsKey(sn.Value))
                    {
                        Parser.LookupByValue[sn.Value] = Parser.LookupByValue[sn.Value] + "," + sn.Name;
                    }
                    else
                    {
                        Parser.LookupByValue[sn.Value] = sn.Name;
                    }
                }

                Trace.TraceInformation("Total number of names: {0}", Parser.LookupByName.Keys.Count);
                Trace.TraceInformation("Total number of values: {0}", Parser.LookupByValue.Keys.Count);
                */
            }
            return true;
        }
    }
}
