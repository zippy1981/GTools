using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Data.Common;
using System.Diagnostics;

namespace GSharpTools.DBTools
{
    public abstract class DBConnection : IDisposable
    {
        protected DbConnection Connection;
        public int RowsAffected;

        public void Cleanup()
        {
            if (Connection != null)
            {
                Connection.Close();
                Connection.Dispose();
                Connection = null;
            }
        }

        public DBConnection(DbConnection connection)
        {
            Connection = connection;
            connection.Open();
        }

        public abstract DbTransaction CreateTransaction();
        public abstract DbCommand CreateCommand(string statement);
        public abstract DbParameter CreateParameter();
        public abstract string BuildNamedParameter(string parameterName);

        public bool IsQueryEmpty(string statement)
        {
            return IsQueryEmpty(statement, null);
        }

        public bool IsQueryEmpty(string statement, IEnumerable<DbParameter> parameters)
        {
            if (statement == null || statement.Equals(""))
                return true;

            DbCommand command = CreateCommand(statement);
            if (parameters != null)
            {
                foreach (DbParameter p in parameters)
                {
                    command.Parameters.Add(p);
                }
            }
            try
            {
                Dump(command);
                DbDataReader reader = command.ExecuteReader();
                bool success = reader.Read();
                reader.Close();
                return !success;
            }
            catch (DbException ne)
            {
                Tools.DumpException(ne, "IsQueryEmpty: {0}", statement);
                throw ne;
            }
        }

        public bool ExecuteScript(string filename)
        {
            Trace.TraceInformation("SQL: BEGIN ExecuteScript(filename = {0})", filename);
            Trace.Indent();
            string input = File.ReadAllText(filename, Encoding.Default);
            bool result = ExecuteNonQuery(input);
            Trace.Unindent();
            Trace.TraceInformation("SQL: END ExecuteScript(filename = {0})", filename);
            return result;
        }

        public void Dump(DbCommand command)
        {
            StringBuilder output = new StringBuilder();
            output.AppendFormat(" SQL: {0}\r\n", command.CommandText);
            output.AppendFormat("Type: {0}\r\n", command.CommandType);
            foreach(DbParameter p in command.Parameters)
            {
                output.AppendFormat("- {0} = {1}\r\n",
                    p.SourceColumn,
                    p.Value);
            }
            Trace.TraceInformation(output.ToString());
        }

        public bool ExecuteNonQuery(string statement, IEnumerable<DbParameter> parameters = null, bool quiet = false)
        {
            if (statement == null || statement.Equals(""))
                return true;

            DbCommand command = CreateCommand(statement);
            command.CommandTimeout = 0;
            if (parameters != null)
            {
                foreach (DbParameter p in parameters)
                {
                    command.Parameters.Add(p);
                }
            }
            RowsAffected = -1;
            try
            {
                if( !quiet )
                    Dump(command);
                RowsAffected = command.ExecuteNonQuery();
            }
            catch (DbException ne)
            {
                Tools.DumpException(ne, "SQL: ExecuteNonQuery: {0}", statement);
                throw ne;
            }
            if (!quiet)
            {
                Trace.TraceInformation("SQL: nRowsAffected = {0}", RowsAffected);
            }
            return true;
        }


        public object ExecuteScalar(string stmt, ParameterList pl = null)
        {
            using (SelectStatement ss = new SelectStatement(this, stmt, pl))
            {
                if (ss.Next())
                {
                    return ss.DataReader[0];
                }
            }
            return null;
        }

        /// <summary>
        /// Create this table
        /// </summary>
        /// <param name="schema"></param>
        public virtual void CreateTableFromSchema(TableSchema schema)
        {
            try
            {
                string stmt = GetCreateTableStatementFromSchema(schema);
                if (stmt != null)
                {
                    ExecuteNonQuery(stmt);
                }
            }
            catch (DbException)
            {
                // do nothing if this fails - this is normally not a severe condition...
            }
        }

        public abstract string GetNativeSqlType(ColumnType type);
        /// <summary>
        /// Returns a list of column names, as they exist in the database
        /// </summary>
        /// <param name="name">name of table</param>
        /// <returns>column names</returns>
        public virtual List<string> GetTableColumns(string name)
        {
            List<string> existingColumns = new List<string>();
            DbCommand command = CreateCommand(string.Format("select * from {0} limit 1;", name));
            try
            {
                DbDataReader reader = command.ExecuteReader();
                for (int i = 0; i < reader.FieldCount; ++i)
                {
                    existingColumns.Add(reader.GetName(i).ToLower());
                }
                reader.Close();
            }
            catch (DbException ne)
            {
                Tools.DumpException(ne, "GetTableColumns: {0}", name);
                throw ne;
            }
            return existingColumns;
        }
        public virtual string GetCreateTableStatementFromSchema(TableSchema schema)
        {
            StringBuilder result = new StringBuilder();
            result.AppendFormat("create table {0} (\n", schema.Name);
            bool first = true;
            foreach (ColumnSchema column in schema.Columns)
            {
                if (first)
                    first = false;
                else
                    result.Append(",\n");
                result.AppendFormat("{0} {1}", column.Name, GetNativeSqlType(column.Type));
            }
            result.Append(");");
            return result.ToString();
        }

        public bool AlterTableForSchema(TableSchema schema)
        {
            List<string> actualColumns = GetTableColumns(schema.Name);

            if (actualColumns.Count == 0)
            {
                Trace.TraceError("ERROR, {0} has no columns, but it does exist???", schema.Name);
                return false;
            }

            // check which of the columns exist in the schema, but not in the database: these must be added now
            List<ColumnSchema> columnsToAdd = new List<ColumnSchema>();
            foreach (ColumnSchema c in schema.Columns)
            {
                if (!actualColumns.Contains(c.Name.ToLower()))
                {
                    ExecuteNonQuery(string.Format("alter table {0} add column {1} {2};", schema.Name, c.Name, GetNativeSqlType(c.Type)));
                }
            }

            // check which columns exist in the database, but no longer in the schema
            foreach (string cn in actualColumns)
            {
                bool found = false;
                foreach (ColumnSchema c in schema.Columns)
                {
                    if (c.Name.ToLower() == cn)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    ExecuteNonQuery(string.Format("alter table {0} drop column {1};", schema.Name, cn));
                }
            }

            return true;
        }
        public virtual bool DoesTableExist(string name)
        {
            return GetTableSize(name) >= 0;
        }

        public virtual int GetTableSize(string name)
        {
            object o = ExecuteScalar(string.Format("select count(*) from %s;", name));
            if (o is long)
            {
                return (int)(long)o;
            }
            return (int)o;
        }

        /// <summary>
        /// Most database providers support a bootstrapping process whereby the database creates all the necessary tables on the fly.
        /// Some providers / some configuration setups require that the tables are created by scripts rather than by code; in these environments 
        /// the framework will not call this function anyway.
        /// </summary>
        public virtual bool ExecuteBootstrapCode(IEnumerable<TableSchema> listOfSchemas)
        {
            foreach (TableSchema ts in listOfSchemas)
            {
                if (DoesTableExist(ts.Name))
                {
                    AlterTableForSchema(ts);
                }
                else
                {
                    CreateTableFromSchema(ts);
                }
            }
            return true;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Connection.Close();
        }

        #endregion
    }
}
