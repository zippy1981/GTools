using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace pserv3
{
    public class StandardColumn : IServiceColumn
    {
        private readonly string Name;
        protected readonly int ID;
        protected readonly HorizontalAlignment TextAlignment;

        public StandardColumn(string name, int id, HorizontalAlignment textAlignment = HorizontalAlignment.Left)
        {
            Name = name;
            ID = id;
            TextAlignment = textAlignment;
        }

        #region IServiceColumn Members

        public HorizontalAlignment GetTextAlign()
        {
            return TextAlignment;
        }

        public string GetName()
        {
            return Name;
        }

        public int GetID()
        {
            return ID;
        }
        
        public virtual int Compare(IServiceObject a, IServiceObject b)
        {
            // default implementation: string compare
            string textA = (a == null) ? "" : a.GetText(ID);
            string textB = (b == null) ? "" : b.GetText(ID);

            if (textA == null)
                textA = "";
            if (textB == null)
                textB = "";
            return textA.CompareTo(textB);
        }

        #endregion
    }
}
