using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace pserv3
{
    public class StandardColumn_UInt : StandardColumn
    {
        public StandardColumn_UInt(string name, int id)
            : base(name, id, HorizontalAlignment.Right)
        {
        }

        #region IServiceColumn Members

        public override int Compare(IServiceObject a, IServiceObject b)
        {
            // default implementation: string compare
            object oA = (a == null) ? (uint)0 : a.GetObject(ID);
            object oB = (b == null) ? (uint)0 : b.GetObject(ID);

            if (oA == null)
                oA = (uint)0;
            if (oB == null)
                oB = (uint)0;

            uint A = (uint)oA;
            uint B = (uint)oB;

            if (A == B)
                return 0;

            if( A < B )             
                return 1;
            
            return -1;
        }

        #endregion
    }
}
