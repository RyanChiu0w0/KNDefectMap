using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace KNDefectMap.CS
{
    public class CSVAccessor
    {

        /// <summary>
        /// 產生csv
        /// </summary>
        /// <param name="path">檔案路徑</param>
        /// <param name="dt">datatable</param>
        public static void ExportCSV(string path, DataTable dt)
        {
            try
            {

                if (dt != null && dt.Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();

                    string[] columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();

                    sb.AppendLine(string.Join(",", columnNames));

                    foreach (DataRow row in dt.Rows)
                    {
                        IEnumerable<string> fields = row.ItemArray.Select(field => string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                        sb.AppendLine(string.Join(",", fields.ToArray()));
                    }
                    File.WriteAllText(path, sb.ToString());
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 從csv取資料
        /// </summary>
        /// <param name="path">檔案路徑</param>
        /// <param name="isFirstRowHeader">判斷header是否為第一列</param>
        /// <returns></returns>
        public static DataTable GetDataTableFromCsv(string path, bool isFirstRowHeader)
        {
            string header = isFirstRowHeader ? "Yes" : "No";

            string pathOnly = Path.GetDirectoryName(path);
            string fileName = Path.GetFileName(path);

            string sql = @"SELECT * FROM [" + fileName + "]";

            using (OleDbConnection connection = new OleDbConnection(
                      @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathOnly +
                      ";Extended Properties=\"Text;HDR=" + header + "\""))
            using (OleDbCommand command = new OleDbCommand(sql, connection))
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
            {
                DataTable dataTable = new DataTable();
                dataTable.Locale = CultureInfo.CurrentCulture;
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        /// <summary>
        /// 從csv取資料
        /// </summary>
        /// <param name="path">檔案路徑</param>
        /// <param name="dataTable">datatable</param>
        /// <returns></returns>
        public static DataTable GetDataTableFromCsv(string path, DataTable dataTable)
        {
            //string header = "No";
            //if(dataTable.Rows.Count==0)
            //    header = "Yes";
            //else
            //    header = "No";
            string header = "Yes";

            path = path.Trim('\r', '\n');
            string pathOnly = Path.GetDirectoryName(path);
            string fileName = Path.GetFileName(path);

            string sql = @"SELECT * FROM [" + fileName + "]";
            //Microsoft Jet只能用在32位元
            using (OleDbConnection connection = new OleDbConnection(
                      @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathOnly +
                      ";Extended Properties=\"Text;HDR=" + header + "\""))
            using (OleDbCommand command = new OleDbCommand(sql, connection))
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
            {
                // DataTable dataTable = new DataTable();
                dataTable.Locale = CultureInfo.CurrentCulture;
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

    }
}
