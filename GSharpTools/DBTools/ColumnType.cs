using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSharpTools.DBTools
{
    public enum ColumnType
    {
        /// <summary>
        /// Unique ID for this row (type is DB dependent, but should normally fit in a int64
        /// </summary>
        ID,         

        /// <summary>
        /// Foreign key, ColumnSchema.ReferenceName must have the format TABLE.KEY
        /// </summary>
        ForeignKey, 

        /// <summary>
        /// Maps a string object
        /// </summary>
        String,

        /// <summary>
        /// Maps an integer object
        /// </summary>
        Integer,
        
        /// <summary>
        /// Maps a truth value
        /// </summary>
        Bool,

        /// <summary>
        /// Maps a DateTime instance
        /// </summary>
        Timestamp,

        /// <summary>
        /// Maps a currency amount. Preferably 64-bit
        /// </summary>
        Amount,

        /// <summary>
        /// Maps a currency ISO code 
        /// </summary>
        Currency,

        /// <summary>
        /// A date (not a complete timestamp)
        /// </summary>
        Date,

        /// <summary>
        /// A floating point value
        /// </summary>
        Double,
    }
}
