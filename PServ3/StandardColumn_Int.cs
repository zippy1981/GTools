using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace pserv3
{
    public class StandardColumn_Int : StandardColumn
    {
        public StandardColumn_Int(string name, int id)
            : base(name, id, HorizontalAlignment.Right)
        {
        }

        #region IServiceColumn Members

        public override int Compare(IServiceObject a, IServiceObject b)
        {
            // default implementation: string compare
            object oA = (a == null) ? 0 : a.GetObject(ID);
            object oB = (b == null) ? 0 : b.GetObject(ID);

            if (oA == null)
                oA = 0;
            if (oB == null)
                oB = 0;
            return ((int)oA) - ((int)oB);
        }

        #endregion
    }
}
