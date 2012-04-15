using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace GSharpTools.DBTools
{
    /// <summary>
    /// A column in a database schema is described by instances of this class. A column has a name and a type. 
    /// </summary>
    public class ColumnSchema
    {
        /// <summary>
        /// Name of the column
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Type of the column
        /// </summary>
        public readonly ColumnType Type;

        /// <summary>
        /// Reference (if Type is ColumnType.ForeignKey);
        /// </summary>
        public readonly string Reference;

        /// <summary>
        /// Comment on this entry (used for documentation purposes only)
        /// </summary>
        public readonly string Comment;

        /// <summary>
        /// Explicit constructor
        /// </summary>
        /// <param name="name">Name of the column</param>
        /// <param name="type">Type of the column</param>
        public ColumnSchema(string name, ColumnType type, string comment)
        {
            Name = name;
            Type = type;
            Comment = comment;
        }

        /// <summary>
        /// Explicit constructor for a foreign key
        /// </summary>
        /// <param name="name">Name of the column</param>
        /// <param name="type">Type of the column</param>
        public ColumnSchema(string name, ColumnType type, string tableName, string columnName, string comment)
        {
            Name = name;
            Type = type;
            Reference = string.Format("{0}.{1}", tableName, columnName);
            Comment = comment;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="objectSrc"></param>
        public ColumnSchema(ColumnSchema objectSrc)
        {
            Name = objectSrc.Name;
            Type = objectSrc.Type;
            Reference = objectSrc.Reference;
            Comment = objectSrc.Comment;
        }
    }
}
