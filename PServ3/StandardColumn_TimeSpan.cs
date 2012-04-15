using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace pserv3
{
    public class StandardColumn_TimeSpan : StandardColumn
    {
        public StandardColumn_TimeSpan(string name, int id)
            :   base(name, id, HorizontalAlignment.Right)
        {
        }

        #region IServiceColumn Members
        
        public override int Compare(IServiceObject a, IServiceObject b)
        {
            // default implementation: string compare
            object oA = (a == null) ? new TimeSpan() : a.GetObject(ID);
            object oB = (b == null) ? new TimeSpan() : b.GetObject(ID);

            if (oA == null)
                oA = new TimeSpan();
            if (oB == null)
                oB = new TimeSpan();
            return ((TimeSpan)oA).CompareTo(oB);
        }

        #endregion
    }
}
