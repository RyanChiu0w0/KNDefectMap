using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace KNDefectMap.DataModel
{
    public  class SpnlMap
    {
        internal int SEQ;
        internal string SpnlID;
        internal int ColumnCount;
        internal int RowCount;
        internal int DummyRowCount;
        //internal Panel SPNL ;
        internal Dictionary<string,List< Rectangle>> PcsListByDefectCode;
        internal string Message;

        /// <summary>
        /// 產生 SpnlMap
        /// </summary>
        /// <param name="seq">在Wpnl上的順序</param>
        /// <param name="spnlID">SpnlID</param>
        /// <param name="cols">行數</param>
        /// <param name="rows">列數</param>
        /// <param name="dummyRowCount">空白列數</param>
        public SpnlMap(int seq,string spnlID,int cols,int rows,int dummyRowCount)
        {
          
            PcsListByDefectCode = new Dictionary<string,List< Rectangle>>();
            PcsListByDefectCode.Add("NoDefect", new List<Rectangle>());
            //SPNL.Size= new Size()

            SEQ = seq;
            SpnlID = spnlID;
            ColumnCount = cols;
            RowCount = rows;
            DummyRowCount = dummyRowCount;
        }
    }
}
