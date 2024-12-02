using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using KNDefectMap.CS;

namespace KNDefectMap.DataModel
{
    public class WpnlMap
    {
        internal SpnlMap[] SPNLs;
        internal int[,] SeqMap;
        internal int SpacingX =5;
        internal int SpacingY =5;

        /// <summary>
        /// Wpnl內Spnl排列方式
        /// </summary>
        /// <param name="numbOfSpnl">Spnl數</param>
        public WpnlMap(int numbOfSpnl)
        {
            SPNLs = new SpnlMap[numbOfSpnl+1];//index 0不使用

            if(numbOfSpnl>12)
            {
                SeqMap = new int[,] { { 20,19,18,17}
                                     ,{ 16,15,14,13 }
                                     ,{ 12,11,10,9}
                                     ,{ 8, 7, 6, 5}
                                     ,{ 4, 3, 2, 1 }
                                     };
            }
            else
            {
                SeqMap = new int[,] { { 12,11,10 }
                                     ,{ 9, 8, 7}
                                     ,{ 6, 5, 4}
                                     ,{ 3, 2, 1 }
                                     };
            }
          
        }

        /// <summary>
        /// Wpnl繪圖
        /// </summary>
        /// <param name="container">容納繪圖之控件</param>
        /// <param name="itemColor">DefectCode顏色(DataGridView)</param>
        /// <param name="specifyDefectCodes">報廢代碼</param>
        /// <param name="selectedSpnl">Spnl</param>
        /// <param name="spnlAngle">角度</param>
        public void Draw(Control container ,DataGridView itemColor,string[] specifyDefectCodes,string  selectedSpnl,int spnlAngle)
        {
            //container.SuspendLayout();
            container.Controls.Clear();
            TableLayoutPanel tableLayout = new TableLayoutPanel();
            tableLayout.ColumnCount = SeqMap.GetLength(1);// 3;
            tableLayout.RowCount = SeqMap.GetLength(0);// 4;
           
            container.Controls.Add(tableLayout);
            tableLayout.Dock = DockStyle.Fill;
            int pcsSize = SPNLs[1].PcsListByDefectCode.First().Value[0].Width+1;// 8;
            float zoom = 1f;
            if (SeqMap.Length > 12)
                zoom = 0.9f;

            int spnlWidth= (SPNLs[1].ColumnCount + 2) * pcsSize;
            int spnlHeight = (SPNLs[1].RowCount+ SPNLs[1].DummyRowCount+ 2) * pcsSize;           

            for ( int x=0;x<tableLayout.ColumnCount;x++)
            {
                for(int y=0;y<tableLayout.RowCount;y++)
                {
                    int spnlSeq = SeqMap[y, x];//SeqMap陣列座標X,Y相反
                    SpnlMap spnl = SPNLs[spnlSeq];
                    if (spnl == null) continue;
                                                      
                    PictureBox sPanel = new PictureBox();
                    sPanel.SizeMode = PictureBoxSizeMode.Zoom;
                    if (spnl.SpnlID==selectedSpnl)
                        sPanel.BorderStyle = BorderStyle.FixedSingle;
                    else
                        sPanel.BorderStyle = BorderStyle.None;

                    switch (spnlAngle)
                    {
                        case 0:
                            sPanel.Size = new Size((int)Math.Ceiling(spnlWidth*zoom)
                                         ,(int)Math.Ceiling( spnlHeight*zoom));
                            break;
                        case 90:
                            sPanel.Size = new Size((int)Math.Ceiling(spnlHeight*zoom)
                                         ,(int)Math.Ceiling( spnlWidth*zoom));
                            break;
                        default:
                            throw new Exception("SPNL Angle: 「" + spnlAngle + "」未處置！");
                    }

                   
                    tableLayout.Controls.Add(sPanel, x, y);
                    tableLayout.AutoScroll = true;

                    Bitmap bitmap = new Bitmap(spnlWidth, spnlHeight);
                    Graphics g = Graphics.FromImage(bitmap);
                   
                    foreach (KeyValuePair<string, List<Rectangle>> defectPcsList in spnl.PcsListByDefectCode)
                    {
                        if ( specifyDefectCodes.Contains(defectPcsList.Key)) continue;//指定的defect最後畫
                        if (defectPcsList.Value.Count == 0) continue;

                        Color color = StaticFunction.GetDefectCodeColor(defectPcsList.Key, itemColor);
                        SolidBrush blueBrush = new SolidBrush(color);
                        g.FillRectangles(blueBrush, defectPcsList.Value.ToArray());

                    }
                    // 指定的defect最後畫,避免同PCS有多項defect畫了又被覆蓋。
                    foreach(string defectCode in specifyDefectCodes)
                    {
                        if (!spnl.PcsListByDefectCode.ContainsKey(defectCode)) continue;

                        List<Rectangle> list = spnl.PcsListByDefectCode[defectCode];
                        if (list != null)
                        {
                            Color color = StaticFunction.GetDefectCodeColor(defectCode, itemColor);
                            SolidBrush blueBrush = new SolidBrush(color);
                            g.FillRectangles(blueBrush, list.ToArray());
                        }
                    }

                    switch (spnlAngle)
                    {
                        case 0:                           
                            break;
                        case 90:
                            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                        default:
                            throw new Exception("SPNL Angle: 「" + spnlAngle + "」未處置！");
                    }
                    if (spnl.Message != "")
                        g.DrawString(spnl.Message, container.Font, new SolidBrush(Color.Red), new PointF(0, 0));

                    sPanel.Image = bitmap;
                }
            }

           // container.ResumeLayout(true);

        }
    }
}
