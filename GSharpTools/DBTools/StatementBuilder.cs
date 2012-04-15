using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Data;
using System.Data.Common;

namespace GSharpTools.DBTools
{
    public class StatementBuilder : IDisposable
    {
        public readonly string TableName;
        private readonly string SkipTableHeader;
        public readonly ParameterList Data;
        public readonly ParameterList Parameters;
        public bool Quiet = false;
        protected readonly DBConnection Database;

        public StatementBuilder(DBConnection c, string tableName)
        {
            Database = c;
            Data = new ParameterList(c);
            Parameters = new ParameterList(c);
            TableName = tableName;
            SkipTableHeader = string.Format("{0}.", TableName);
        }

        public void Dispose()
        {
        }

        public bool FilterField(ref string fieldName)
        {
            int k = fieldName.IndexOf('.');
            if (k < 0)
                return false;

            if (fieldName.StartsWith(SkipTableHeader))
            {
                fieldName = fieldName.Substring(k + 1);
                return false;
            }
            return true;
        }
        
        public bool Execute()
        {
            ParameterList JoinedList = ParameterList.Merge(Data, Parameters);
            if (JoinedList.Count == 0)
            {
                Trace.TraceInformation("ERROR, no parameters in sql: {0}", ToString());
                return false;
            }
            return Database.ExecuteNonQuery(ToString(), JoinedList, Quiet);
        }
    }

}
