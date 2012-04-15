using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.IO;

namespace GPrint
{
    class XmlDocStruct
    {
        public readonly XmlStyleSheet StyleSheet = new XmlStyleSheet();
        //public readonly List<XmlDocElement> Elements = new XmlDocElement();

        public XmlDocStruct(string xmlData)
        {
            /*XmlTextReader reader = new XmlTextReader(new StringReader(xmlData));
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        {
                            Trace.TraceInformation("Begin {0}.", reader.Name);
                            Trace.Indent();

                            XmlParagraphStyle p = StyleSheet.FindParagraphStyle(reader.Name);
                            if (p != null)
                            {
                                Elements.Add(new XmlDocTextBlock(p));
                            }
                        }
                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        Trace.TraceInformation("Text '{0}'.", reader.Value);
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        Trace.Unindent();
                        Trace.TraceInformation("End {0}.", reader.Name);                        
                        break;
                    default:
                        Trace.TraceInformation("{0} ????", reader.NodeType);
                        break;
                }
            }*/
        }
    }
}
