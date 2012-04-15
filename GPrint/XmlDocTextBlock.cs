using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPrint
{
    class XmlDocTextBlock : XmlDocElement
    {
        public readonly XmlParagraphStyle Style;

        public XmlDocTextBlock(XmlParagraphStyle style)
        {
            Style = style;
        }
    }
}
