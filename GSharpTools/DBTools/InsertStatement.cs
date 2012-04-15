using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Data;
using System.Data.Common;

namespace GSharpTools.DBTools
{
    public class InsertSequence
    {
        public readonly string Name;
        public readonly string Sequence;

        public InsertSequence(string name, string sequence)
        {
            Name = name;
            Sequence = sequence;
        }
    }

    public class InsertStatement : StatementBuilder
    {
        public InsertStatement(DBConnection database, string tableName)
            : base(database, tableName)
        {
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendFormat("insert into {0} (", TableName);

            bool first = true;
            foreach (DbParameter np in Data)
            {
                if (first)
                    first = false;
                else
                    result.Append(",");

                result.Append(np.SourceColumn);
            }
            first = true;
            result.Append(") values (");
            foreach (DbParameter np in Data)
            {
                if (first)
                    first = false;
                else
                    result.Append(",");

                result.Append(Database.BuildNamedParameter(np.ParameterName));
            }
            result.Append(");");
            return result.ToString();
        }
    }
}
