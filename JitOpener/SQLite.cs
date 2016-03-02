using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Threading;


namespace SQLiteBrowser
{
    public class SQLiteException : Exception
    {
        public SQLiteException(string message) :
            base(message)
        {

        }
    }

    public class SQLite
    {
        enum Constants
        {
            SQLITE_OK = 0,
            SQLITE_ROW = 100,
            SQLITE_DONE = 101,
            SQLITE_INTEGER = 1,
            SQLITE_FLOAT = 2,
            SQLITE_TEXT = 3,
            SQLITE_BLOB = 4,
            SQLITE_NULL = 5,
        }


        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_open", CallingConvention = CallingConvention.Cdecl)]
        static extern SQLite.Constants sqlite3_open(
        [MarshalAs(UnmanagedType.LPStr)]  string filename, out IntPtr db);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_close", CallingConvention = CallingConvention.Cdecl)]
        static extern SQLite.Constants sqlite3_close(IntPtr db);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_prepare_v2", CallingConvention = CallingConvention.Cdecl)]
        static extern SQLite.Constants sqlite3_prepare_v2(IntPtr db, IntPtr zSql,
          int nByte, out IntPtr ppStmpt, IntPtr pzTail);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_step", CallingConvention = CallingConvention.Cdecl)]
        static extern SQLite.Constants sqlite3_step(IntPtr stmHandle);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_finalize", CallingConvention = CallingConvention.Cdecl)]
        static extern SQLite.Constants sqlite3_finalize(IntPtr stmHandle);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_errmsg", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr sqlite3_errmsg(IntPtr db);

        [DllImport("sqlite3.dll",
          EntryPoint = "sqlite3_column_count", CallingConvention = CallingConvention.Cdecl)]
        static extern int sqlite3_column_count(IntPtr stmHandle);

        [DllImport("sqlite3.dll",
          EntryPoint = "sqlite3_column_origin_name", CallingConvention = CallingConvention.Cdecl)]
        static extern  IntPtr sqlite3_column_origin_name(
          IntPtr stmHandle, int iCol);

        [DllImport("sqlite3.dll",
          EntryPoint = "sqlite3_column_type", CallingConvention = CallingConvention.Cdecl)]
        static extern SQLite.Constants sqlite3_column_type(
          IntPtr stmHandle, int iCol);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_column_int", CallingConvention = CallingConvention.Cdecl)]
        static extern int sqlite3_column_int(
          IntPtr stmHandle, int iCol);

        [DllImport("sqlite3.dll",
          EntryPoint = "sqlite3_column_text", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr sqlite3_column_text(
          IntPtr stmHandle, int iCol);

        [DllImport("sqlite3.dll",
          EntryPoint = "sqlite3_column_double", CallingConvention = CallingConvention.Cdecl)]
        static extern double sqlite3_column_double(
          IntPtr stmHandle, int iCol);

        private IntPtr _db; //pointer to SQLite database
        private bool _open; //whether or not the database is open

        /// <summary>
        /// Opens or creates SQLite database with the specified path
        /// </summary>
        /// <param name="path">Path to SQLite database</param>
        public void OpenDatabase(string path)
        {
            if (sqlite3_open(path, out _db) != (int)SQLite.Constants.SQLITE_OK)
                throw new SQLiteException(
                  "Could not open database file: " + path);

            _open = true;
        }

        /// <summary>
        /// Closes the SQLite database
        /// </summary>
        public void CloseDatabase()
        {
            if (_open)
                sqlite3_close(_db);

            _open = false;
        }

        /// <summary>
        /// Executes a query that returns no results
        /// </summary>
        /// <param name="query">SQL query to execute</param>
        public void ExecuteNonQuery(string query)
        {
            if (!_open)
                throw new SQLiteException(
                  "SQLite database is not open.");

            //prepare the statement
            IntPtr stmHandle = Prepare(query);

            if (sqlite3_step(stmHandle) != SQLite.Constants.SQLITE_DONE)
                throw new SQLiteException(
                  "Could not execute SQL statement.");

            Finalize(stmHandle);
        }

        /// <summary>
        /// Executes a query and stores the results in 
        /// a DataTable
        /// </summary>
        /// <param name="query">SQL query to execute</param>
        /// <returns>DataTable of results</returns>
        public DataTable ExecuteQuery(string query)
        {
            if (!_open)
                throw new SQLiteException(
                  "SQLite database is not open.");

            //prepare the statement
            IntPtr stmHandle = Prepare(query);

            //get the number of returned columns
            int columnCount = sqlite3_column_count(stmHandle);

            //create datatable and columns
            DataTable dTable = new DataTable();
            for (int i = 0; i < columnCount; i++)
                dTable.Columns.Add(
                    Marshal.PtrToStringAnsi(
                  sqlite3_column_origin_name(stmHandle, i)));

            //populate datatable
            while (sqlite3_step(stmHandle) == SQLite.Constants.SQLITE_ROW)
            {
                object[] row = new object[columnCount];
                for (int i = 0; i < columnCount; i++)
                {
                    switch (sqlite3_column_type(stmHandle, i))
                    {
                        case SQLite.Constants.SQLITE_INTEGER:
                            row[i] = sqlite3_column_int(stmHandle, i);
                            break;
                        case SQLite.Constants.SQLITE_TEXT:
                            row[i] = Marshal.PtrToStringAnsi(sqlite3_column_text(stmHandle, i));
                            break;
                        case SQLite.Constants.SQLITE_FLOAT:
                            row[i] = sqlite3_column_double(stmHandle, i);
                            break;
                    }
                }

                dTable.Rows.Add(row);
            }

            Finalize(stmHandle);

            return dTable;
        }


        /// <summary>
        /// Executes a query on a different thread and provides the results one by one via a delegate
        /// </summary>
        /// <param name="query">SQL query to execute</param>
        /// <returns>DataTable of results</returns>
        public void AsyncExecuteQuery(string query, Predicate<DataRow> data, Action completed)
        {
            if (!_open)
                throw new SQLiteException(
                  "SQLite database is not open.");

            Thread t = new Thread(new ThreadStart(() =>
            {
                //prepare the statement
                IntPtr stmHandle = Prepare(query);

                //get the number of returned columns
                int columnCount = sqlite3_column_count(stmHandle);

                //create datatable and columns
                DataTable dTable = new DataTable();
                for (int i = 0; i < columnCount; i++)
                    dTable.Columns.Add(
                      Marshal.PtrToStringAnsi(
                      sqlite3_column_origin_name(stmHandle, i)));

                //populate datatable
                while (sqlite3_step(stmHandle) == SQLite.Constants.SQLITE_ROW)
                {
                    object[] row = new object[columnCount];
                    for (int i = 0; i < columnCount; i++)
                    {
                        switch (sqlite3_column_type(stmHandle, i))
                        {
                            case SQLite.Constants.SQLITE_INTEGER:
                                row[i] = sqlite3_column_int(stmHandle, i);
                                break;
                            case SQLite.Constants.SQLITE_TEXT:
                                row[i] = Marshal.PtrToStringAnsi(sqlite3_column_text(stmHandle, i));
                                break;
                            case SQLite.Constants.SQLITE_FLOAT:
                                row[i] = sqlite3_column_double(stmHandle, i);
                                break;
                        }
                    }

                    DataRow r = dTable.Rows.Add(row);
                    if (!data.Invoke(r))
                    {   
                        break;
                    }
                }

                Finalize(stmHandle);

                completed.Invoke();
            }));

            t.IsBackground = true;
            t.Priority = ThreadPriority.Lowest;
            t.Start();
        }

        /// <summary>
        /// Prepares a SQL statement for execution
        /// </summary>
        /// <param name="query">SQL query</param>
        /// <returns>Pointer to SQLite prepared statement</returns>
        private IntPtr Prepare(string query)
        {
            IntPtr stmHandle;
            IntPtr queryPtr = Marshal.StringToHGlobalAnsi(query);

            if (sqlite3_prepare_v2(_db, queryPtr, query.Length,
              out stmHandle, IntPtr.Zero) != SQLite.Constants.SQLITE_OK)
                throw new SQLiteException(Marshal.PtrToStringAnsi(sqlite3_errmsg(_db)));

            Marshal.FreeHGlobal(queryPtr);

            return stmHandle;
        }

        /// <summary>
        /// Finalizes a SQLite statement
        /// </summary>
        /// <param name="stmHandle">
        /// Pointer to SQLite prepared statement
        /// </param>
        private void Finalize(IntPtr stmHandle)
        {
            sqlite3_finalize(stmHandle);
        }
    }
}
