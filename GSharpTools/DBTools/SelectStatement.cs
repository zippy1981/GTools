using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace GSharpTools.DBTools
{
    public class SelectStatement : IDisposable
    {
        public DbDataReader DataReader;
        public readonly DBConnection Database;
        private int RowNumber;
        public static int DefaultInt = 0;
        public static long DefaultLong = 0;
        public static double DefaultDouble = 0;

        public SelectStatement(DBConnection c, string statement, IEnumerable<DbParameter> parameters)
        {
            Database = c;
            RowNumber = 0;

            DbCommand command = c.CreateCommand(statement);
            if (parameters != null)
            {
                foreach (DbParameter p in parameters)
                {
                    command.Parameters.Add(p);
                }
            }
            DataReader = command.ExecuteReader();
        }

        public bool Next()
        {
            if (DataReader != null)
            {
                if (DataReader.Read())
                {
                    ++RowNumber;
                    return true;
                }
                DataReader.Close();
                DataReader = null;
            }
            return false;
        }

        public void Dispose()
        {
            if (DataReader != null)
            {
                DataReader.Close();
                DataReader = null;
            }
        }

        public object[] AllItemsAsObjects()
        {
            object[] result = new object[DataReader.FieldCount];
            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = GetValue(i);
            }
            return result;
        }


        public string[] AllItemsAsText()
        {
            string[] result = new string[DataReader.FieldCount];
            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = AsText(i);
            }
            return result;
        }

        public string[] AllItemsAsText(string[] AdditionalNames)
        {
            int n = DataReader.FieldCount, i;
            string[] result = new string[n + AdditionalNames.Length];
            for (i = 0; i < n; ++i)
            {
                result[i] = AsText(i);
            }
            while (i < result.Length)
            {
                result[i++] = "";
            }

            return result;
        }

        public string[] AllColumnNamesAsOOT(string prefix, string[] AdditionalNames)
        {
            StringBuilder temp = new StringBuilder();
            int n = DataReader.FieldCount;
            string[] result = new string[n + AdditionalNames.Length];            
            for (int i = 0; i < n; ++i)
            {
                result[i] = "(" + prefix + DataReader.GetName(i) + ")";
                temp.Append(result[i]);
                temp.Append(",");
            }
            for (int j = 0; j < AdditionalNames.Length; ++j)
            {
                result[n + j] = "(" + AdditionalNames[j] + ")";
                temp.Append(result[n + j]);
                temp.Append(",");
            }
            Trace.TraceInformation(temp.ToString());
            return result;
        }

        public string[] AllColumnNamesAsOOT(string prefix)
        {
            StringBuilder temp = new StringBuilder();
            string[] result = new string[DataReader.FieldCount];
            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = "(" + prefix + DataReader.GetName(i) + ")";
                temp.Append(result[i]);
                temp.Append(",");
            }
            Trace.TraceInformation(temp.ToString());
            return result;
        }

        public object GetValue(int index)
        {
            object result = DataReader[index];
            if (result is DBNull)
                result = null;
            return result;
        }

        public static string ValueAsText(object value, DbType TypeHint)
        {
            try
            {
                if (value == null)
                    return "";

                if (value is DBNull)
                    return "";

                if (value is string)
                    return value as string;

                if (value is double)
                    return string.Format("{0:#,##0.00}", (double)value);

                if (value is int || value is long)
                    return string.Format("{0}", value);

                if (value is DateTime)
                {
                    DateTime v = (DateTime)value;

                    if (TypeHint == DbType.Time)
                        return v.ToLongTimeString();

                    return v.ToShortDateString();
                }

                return "";
            }
            catch (Exception e)
            {
                Tools.DumpException(e, "ValueAsText: {0} [{1}]", value, value.GetType());
                return "";
            }
        }

        public string AsText(int index)
        {
            return ValueAsText(DataReader[index], DbType.Object);
        }

        public string AsText(int index, DbType TypeHint)
        {
            return ValueAsText(DataReader[index], TypeHint);
        }

        public int AsInt(int index)
        {
            try
            {
                object value = DataReader[index];
                if ((value == null) || (value is DBNull))
                    return DefaultInt;
                if (value is long)
                    return (int)(long)value;
                return (int)value;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public long AsLong(int index)
        {
            try
            {
                object value = DataReader[index];
                if ((value == null) || (value is DBNull))
                    return DefaultLong;
                if (value is int)
                    return (long)(int)value;
                return (long)value;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public double AsDouble(int index)
        {
            try
            {
                object value = DataReader[index];
                if ((value == null) || (value is DBNull))
                    return DefaultDouble;
                if (value is int)
                    return (double)(int)value;
                if (value is long)
                    return (double)(long)value;
                return (double)value;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        public DateTime AsDateTime(int index)
        {
            try
            {
                object value = DataReader[index];
                if (!(value == null) && (value is DateTime))
                    return (DateTime)value;
            }
            catch (Exception)
            {
            }
            return DateTime.Now;
        }

        public bool AsBool(int index, bool defaultValue)
        {
            try
            {
                object value = DataReader[index];
                if (!(value == null) && (value is bool))
                    return (bool)value;
            }
            catch (Exception)
            {
            }
            return defaultValue;
        }

        public bool IsNull(int index)
        {
            try
            {
                object value = DataReader[index];
                return (value is DBNull) || (value == null);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public T Get<T>(int index, T defaultValue)
        {
            try
            {
                object value = DataReader[index];
                if (value is DBNull)
                    return defaultValue;
                return (T)value;
            }
            catch (Exception e)
            {
                Tools.DumpException(e, "Get: {0}, {1}", index, defaultValue.GetType());
                return defaultValue;
            }
        }

        public DbDataReader Row
        {
            get
            {
                return DataReader;
            }
        }
    }
}
