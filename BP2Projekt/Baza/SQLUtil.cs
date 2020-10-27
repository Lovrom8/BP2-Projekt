using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP2Projekt.Baza
{
    public static class SQLUtil
    {
        public static bool ColumnExists(IDataReader reader, string columnName)
        {
            return reader.GetSchemaTable()
                         .Rows
                         .OfType<DataRow>()
                         .Any(row => row["ColumnName"].ToString() == columnName);
        }

        public static int GetNthOrdinal(this SQLiteDataReader reader, string columnName, int nthOccurrence = 1)
        {
            DataTable schema = reader.GetSchemaTable();

            var occurrences = schema.Rows.Cast<DataRow>().Where(r => string.Equals((string)r["ColumnName"], columnName, StringComparison.Ordinal));
            var occurrence = occurrences.Skip(nthOccurrence - 1).First();

            object idx = occurrence["ColumnOrdinal"];

            return Convert.ToInt32(idx);
        }
    }
}
