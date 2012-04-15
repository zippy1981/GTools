using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace pserv3
{
    public class StandardColumn_Long : StandardColumn
    {
        public StandardColumn_Long(string name, int id)
            : base(name, id, HorizontalAlignment.Right)
        {
        }

        #region IServiceColumn Members

        public override int Compare(IServiceObject a, IServiceObject b)
        {
            // default implementation: string compare
            object oA = (a == null) ? 0L : a.GetObject(ID);
            object oB = (b == null) ? 0L : b.GetObject(ID);

            if (oA == null)
                oA = 0L;
            if (oB == null)
                oB = 0L;
            long delta = (long)oA - (long)oB;
            if (delta > 0)
                return 1;
            if (delta < 0)
                return -1;
            return 0;
        }

        #endregion
    }
}
