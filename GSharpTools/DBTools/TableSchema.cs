using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSharpTools.DBTools
{
    /// <summary>
    /// This class defines all the tables used in ProAKT
    /// </summary>
    public class TableSchema
    {
        /// <summary>
        /// Name of the table
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// (Sorted) List of columns in the table
        /// </summary>
        public readonly ColumnSchema[] Columns;

        /// <summary>
        /// Comment on this entry (used for documentation purposes only)
        /// </summary>
        public readonly string Comment;

        /// <summary>
        /// The explicit constructor creates a new table schema
        /// </summary>
        /// <param name="name"></param>
        /// <param name="columns"></param>
        public TableSchema(string name, string comment, params ColumnSchema[] columns)
        {
            Name = name;
            Comment = comment;
            Columns = columns;
        }
    }
}
