using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GSharpTools
{
    public class SafeAccess
    {
        public object PublicProperty(object o, string property)
        {
            try
            {
                if (o == null)
                    return null;

                Type t = o.GetType();
                PropertyInfo pi = t.GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
                return pi.GetValue(o, null);
            }
            catch (Exception e)
            {
                Tools.DumpException(e, "SafeAccess.PublicProperty({0}.{1})", o, property);
                return null;
            }
        }

        public object PublicField(object o, string property)
        {
            try
            {
                if (o == null)
                    return null;

                Type t = o.GetType();
                FieldInfo pi = t.GetField(property, BindingFlags.Public | BindingFlags.Instance);
                return pi.GetValue(o);
            }
            catch (Exception e)
            {
                Tools.DumpException(e, "SafeAccess.PublicField({0}.{1})", o, property);
                return null;
            }
        }
    }
}
