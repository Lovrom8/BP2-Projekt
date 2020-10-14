using System;
using System.Collections.Generic;
using System.Data;
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
    }
}
