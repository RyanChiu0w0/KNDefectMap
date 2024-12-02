using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Data;


namespace KNDefectMap.CS
{
    public static class StaticFunction
    {
        /// <summary>
        /// 取得 DefectCode 顏色類型
        /// </summary>
        /// <param name="defectCode">報廢代碼</param>
        /// <param name="dgv">DataGridView</param>
        /// <returns></returns>
        public static Color GetDefectCodeColor(string defectCode,DataGridView dgv)
        {
            Color c = Color.Silver;
            object dataSource = dgv.DataSource;
            int rid;
            switch (dataSource)
            {
                case DataTable dt:
                    DataRow[] rows = dt.Select("報廢代碼='" + defectCode + "' and 標記=True");
                  
                    if (rows.Length == 0) return c;

                    rid = dt.Rows.IndexOf(rows[0]);
                    c = dgv.Rows[rid].DefaultCellStyle.BackColor;
                    break;
                case BindingSource bs:
                    DataTable dataTable = (DataTable)bs.DataSource;
                    DataRow[] r = dataTable.Select("報廢代碼='" + defectCode + "' and 標記=True");

                    if (r.Length == 0) return c;

                    rid = dataTable.Rows.IndexOf(r[0]);
                    c = dgv.Rows[rid].DefaultCellStyle.BackColor;
                    break;
                case object obj:
                    throw new Exception(dataSource.GetType().ToString() + "未處置！");                   
                default:
                    //null; do nothing
                    break;
            }

            return c;
        }
        
        /// <summary>
        /// Function to get a random number 
        /// </summary>
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        private static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }
        //public static  Color RandomColor()
        //{
        //   //顏色會太接近
        //    int R = RandomNumber(128, 255);
        //    int G = RandomNumber(128, 255);
        //    int B = RandomNumber(128, 255);
        //    B = (R + G > 400) ? R + G - 400 : B;
        //     B = (B > 255) ? 255 : B;

        //    return Color.FromArgb(R, G, B);
        //}

        /// <summary>
        /// RGB顏色
        /// </summary>
        /// <param name="i">隨機數種子值</param>
        /// <returns></returns>
        public static Color RandomColor(int i)
        {
             Random ran = new Random(i);
            int R = ran.Next(100, 170);
            int G = ran.Next(50, 170);
            int B = ran.Next(50, 170);
           

            return Color.FromArgb(R, G, B);
        }
       
    }
}
