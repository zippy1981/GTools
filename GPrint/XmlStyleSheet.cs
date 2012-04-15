using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPrint
{
    class XmlStyleSheet
    {
        public readonly List<XmlParagraphStyle> ParagraphStyles = new List<XmlParagraphStyle>();
        
        public XmlStyleSheet()
        {
            ParagraphStyles.Add(new XmlParagraphStyle("p"));
            ParagraphStyles.Add(new XmlParagraphStyle("h1"));
        }
    }
}
