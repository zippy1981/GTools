using System;
using System.Collections.Generic;
using GSharpTools.DBTools;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace GSharpSQLite
{
    public class Database : DBConnection
    {
        public Database(string connectionString)
            :   base(new SQLiteConnection(connectionString))
        {
        }

        public override DbTransaction CreateTransaction()
        {
            return Connection.BeginTransaction();
        }

        public override DbCommand CreateCommand(string statement)
        {
            return new SQLiteCommand(statement, this.Connection as SQLiteConnection);
        }

        public override DbParameter CreateParameter()
        {
            return new SQLiteParameter();
        }

        public override string BuildNamedParameter(string parameterName)
        {
            return "?";
        }

        public override bool DoesTableExist(string name)
        {
            return !IsQueryEmpty(string.Format("select name from sqlite_master where lower(name)='{0}'", name.ToLower()));
        }

        public override string GetNativeSqlType(ColumnType type)
        {
            switch (type)
            {
                case ColumnType.ID:
                    return "integer primary key autoincrement";

                case ColumnType.ForeignKey:
                case ColumnType.Integer:
                    return "integer";

                case ColumnType.String:
                    return "text";

                case ColumnType.Bool:
                    return "bool";

                case ColumnType.Timestamp:
                    return "timestamp";

                case ColumnType.Date:
                    return "date";

                case ColumnType.Double:
                    return "double";
                
                default:
                    return null;
            }
        }
    }
}
