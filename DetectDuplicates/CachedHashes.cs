using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using GSharpTools;
using GSharpTools.DBTools;
using GSharpSQLite;
using System.Diagnostics;
using System.Text;

namespace DetectDuplicates
{
    class CachedHashes
    {
        private readonly Dictionary<string, string> CacheValues = new Dictionary<string, string>();

        public string LookupHash(string filename)
        {
            filename = filename.ToLower();
            if( CacheValues.ContainsKey(filename) )
                return CacheValues[filename];
            return null;
        }

        /// <summary>
        /// If configured, this function reads from a SQLite database caching previously known hashes.
        /// The motivation for this is that the most expensive operation would be the MD5 hash calculation;
        /// That means that on a second run of DetectDuplicates, we may want to reuse existing known MD5 hashes.
        /// </summary>
        public bool Initialize(string database_filename, Dictionary<long, Dictionary<string, string>> cache)
        {
            if (string.IsNullOrEmpty(database_filename))
                return false;

            Filename = database_filename;

            try
            {
                // connect to database
                Trace.TraceInformation("About to read cache from \"{0}\"", Filename);
                Database = new Database("Data Source=" + Filename);

                // make sure lookup table exists
                Database.ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS hashes (hash TEXT, filename TEXT);");

                DateTime cacheStartTime = DateTime.Now;

                // read known hashes from lookup table
                int CachedHashesRead = 0;
                using (SelectStatement ss = new SelectStatement(Database, "SELECT hash, filename FROM hashes", null))
                {
                    while (ss.Next())
                    {
                        string hash = ss.AsText(0);
                        string filename = ss.AsText(1);
                        CacheValues[filename.ToLower()] = hash;
                        ++CachedHashesRead;
                    }
                }
                if (CachedHashesRead == 0)
                {
                    Console.WriteLine("Cache is empty as of yet...");
                }
                else
                {
                    TimeSpan elapsed = DateTime.Now - cacheStartTime;
                    Console.WriteLine("Read {0} hashes from the cache in {1}...", CachedHashesRead, elapsed);
                }
                return true;
            }
            catch (Exception e)
            {
                Tools.DumpException(e, "ReadCache() caught an exception while reading \"{0}\"", Filename);
                return false;
            }
        }

        public void Flush()
        {
            if (Database != null)
            {
                if (Transaction != null)
                {
                    Debug.Assert(Command != null);
                    Debug.Assert(SizeUsed > 0);

                    Transaction.Commit();
                    Command.Dispose();
                    Command = null;
                    Transaction.Dispose();
                    Transaction = null;
                    SizeUsed = 0;
                }
            }
        }

        public void Write(string hash, string filename)
        {
            if (Database != null)
            {
                // create transaction object if it doesn't exist yet
                if( Transaction == null )
                {
                    Debug.Assert(SizeUsed == 0);
                    Debug.Assert(Command == null);

                    Transaction = Database.CreateTransaction();
                    Command = Database.CreateCommand("INSERT INTO hashes (hash, filename) VALUES (?,?)");

                    Field_HashText = Command.CreateParameter();
                    Field_FileName = Command.CreateParameter();

                    Command.Parameters.Add(Field_HashText);
                    Command.Parameters.Add(Field_FileName);
                }

                Field_HashText.Value = hash;
                Field_FileName.Value = filename;
                Command.ExecuteNonQuery();

                ++SizeUsed;
                if (SizeUsed >= FLUSH_SIZE)
                {
                    Flush();
                }
            }
        }

        /// <summary>
        /// Filename for database
        /// </summary>
        private string Filename;

        private DbParameter Field_HashText;
        private DbParameter Field_FileName;

        /// <summary>
        /// Connection to SQLite database
        /// </summary>
        private DBConnection Database;

        /// <summary>
        /// Transaction used to speed up the processing
        /// </summary>
        private DbTransaction Transaction;

        /// <summary>
        /// Command used during an active transaction
        /// </summary>
        private DbCommand Command;

        /// <summary>
        /// flush transaction every 10000 elements
        /// </summary> 
        private const int FLUSH_SIZE = 1000;

        /// <summary>
        /// current size of elements in cache 
        /// </summary>
        private int SizeUsed = 0;
    }
}
