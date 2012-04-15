using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;

namespace GSharpTools.DBTools
{
    public class UpdateStatement : StatementBuilder
    {
        private readonly string WhereClause;

        public UpdateStatement(DBConnection database, string tableName, string whereClause)
            : base(database, tableName)
        {
            WhereClause = whereClause;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendFormat("update {0} set ", TableName);

            bool first = true;
            foreach (DbParameter np in Data)
            {
                if (first)
                    first = false;
                else
                    result.Append(",");

                result.AppendFormat("{0}={1}", np.SourceColumn, Database.BuildNamedParameter(np.ParameterName));
            }
            result.Append(" where ");
            result.Append(WhereClause);
            result.Append(";");
            return result.ToString();
        }
    }
}
