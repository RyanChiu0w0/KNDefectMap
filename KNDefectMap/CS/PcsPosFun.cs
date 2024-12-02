using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace KNDefectMap
{
    class PcsPosFun
    {
        /// <summary>
        /// 計算 Xout 面 PcsIndex
        /// </summary>
        /// <param name="spnlPcsIndex">起始為零的流水號</param>
        /// <param name="spnlNumOfCol">PCS行數</param>
        /// <param name="side">Xout面</param>
        /// <returns></returns>
        public static int calPcsIndexOnXoutSide(int spnlPcsIndex, int spnlNumOfCol, string side)
        {
            if (spnlNumOfCol < 1) return -1;
            if (side.Contains("A"))
                return spnlNumOfCol - (spnlPcsIndex % spnlNumOfCol) - 1 + (spnlPcsIndex / spnlNumOfCol) * spnlNumOfCol; // int除法(/)只取整數
            else
                return spnlPcsIndex;

        }
        /// <summary>
        /// 依輸入的pcs index 計算另一面相對位置pcs之index。
        /// </summary>
        /// <param name="SpnlPcsIndex">起始為零的流水號</param>
        /// <returns>另一面相對位置pcs之index</returns>
        public static int getOtherSidePcsIndex(int spnlPcsIndex, int spnlNumOfCol)
        {
            if (spnlNumOfCol == 0) return -1;
            return spnlNumOfCol - (spnlPcsIndex % spnlNumOfCol) - 1 + (spnlPcsIndex / spnlNumOfCol) * spnlNumOfCol; // 除法(/)只取商
        }        

        /// <summary>
        /// 依PCS索引輸出在SPNL上的座標
        /// </summary>
        /// <param name="pcsIndex">從0開始的PCS索引</param>
        /// <param name="centerRowID">SPNL中間空白列編號(EX: LM 或 14,15)</param>
        /// <param name="isNumAtLongEdge">數字在長邊=True, 數字在短邊=False</param>
        /// <returns></returns>
        public static string GetCoordinateOnSPNL(int spnlPcsIndex, string centerRowID, bool isNumAtLongEdge, int spnlNumOfCol)
        {
            int x;
            int y;
            if (isNumAtLongEdge)
            {//數字在長邊
                y = (spnlPcsIndex / spnlNumOfCol) + 65;//商數+65等於列座標
                x = spnlPcsIndex % spnlNumOfCol + 1;//餘數+1等於行座標
                //跳過空白列ID
                int firstSpaceRow;//第一個空白列ascii碼
                centerRowID = centerRowID.Trim().ToUpper();
                if (centerRowID.Length > 0)
                {
                    firstSpaceRow = centerRowID[0];
                    if (y >= Convert.ToInt16(firstSpaceRow))
                    {
                        y += centerRowID.Length;
                    }
                }

                //大於Z 則依序為AA, BB, CC ...
                string yy;
                if (y > 90)
                {
                    y = y - 90 + 64;
                    if (y > 90)
                    {
                        throw new Exception("座標大於兩個字母！");
                    }
                    yy = Convert.ToChar(y).ToString() + Convert.ToChar(y);
                }
                else
                {
                    yy = Convert.ToChar(y).ToString();
                }
                return x.ToString() + "-" + yy;
            }
            else
            { //數字在短邊
                y = (spnlPcsIndex / spnlNumOfCol) + 1;//商數+1等於列座標
                x = spnlPcsIndex % spnlNumOfCol + 65;//餘數+65等於行座標
                string[] emptyRow = centerRowID.Split(',');
                int firstEmptyRow;//第一個空白列編號
                if (emptyRow.Length > 0)
                {
                    if (!int.TryParse(emptyRow[0], out firstEmptyRow))
                    {
                        throw new Exception("centerRowID 不是一個數字！");
                    }
                    if (y >= firstEmptyRow)
                    {
                        y += emptyRow.Length;
                    }
                }

                string xx;
                if (x > 90)
                {
                    x = x - 90 + 64;
                    if (x > 90)
                    {
                        throw new Exception("座標大於兩個字母！");
                    }
                    xx = Convert.ToChar(x).ToString() + Convert.ToChar(x);
                }
                else
                {
                    xx = Convert.ToChar(x).ToString();
                }
                return xx + "-" + y.ToString();
            }
        }

        /// <summary>
        /// 依PCS索引計算SPNL原點在左上角的PCS座標。
        /// </summary>
        /// <param name="spnlPcsIndex">從0開始的PCS索引</param>
        /// <param name="spnlNumOfCol">PCS行數</param>
        /// <returns></returns>
        public static Point CalPcsXY(int spnlPcsIndex, int spnlNumOfCol)
        {//SpnlPcsIndex由0開始
            int x = -1;
            int y = -1;
            try
            {
                y = (spnlPcsIndex / spnlNumOfCol) + 1;//商數+1等於列座標
                x = spnlPcsIndex % spnlNumOfCol + 1;//餘數+1等於行座標

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                //  writeLog(ex.Message + "\n" + ex.StackTrace);
            }
            return new Point(x, y);
        }

        /// <summary>
        /// 轉ASCII
        /// </summary>
        /// <param name="str">要轉型的字元</param>
        /// <returns></returns>
        public static int StringToInt(string str)
        {
            int result = 0;
            if (str == "")
                return result;
            if (!int.TryParse(str, out result))
            {
                //將字元為ASCII(A為65 順序為1)若為AA or AAA 乘上字串長度 * 26
                str = str.Trim().ToUpper();
                //for (int i = 0; i < str.Length; i++)
                //    result = Convert.ToInt32(str[i]) - 64 + i * 26;
                result = Convert.ToInt32(str.Last()) - 64 + (str.Length - 1) * 26;
            }
            return result;
        }

        /// <summary>
        /// 將第N行或第N列轉成字母。若大於Z則依序為AA, BB, CC ...
        /// </summary>
        /// <param name="number">N</param>
        /// <returns></returns>
        public static string IntToString(int number)
        {
            char result ;
            number -=1;
            //大於Z 則依序為AA, BB, CC ...
            int char_int = (number % 26)+65;
            int stringLength = (number / 26)+1;
            result = Convert.ToChar(char_int);
            return result.ToString().PadLeft(stringLength, result);           

        }

        /// <summary>
        /// 將板邊座標換算成PCS Index.
        /// </summary>
        /// <param name="x">PCS行座標</param>
        /// <param name="y">PCS列座標</param>
        /// <param name="lastX">X最大值</param>
        /// <param name="lastY">Y最大值</param>
        /// <param name="dummyRows">空白行</param>
        /// <param name="chamfer">缺角位置</param>
        /// <returns></returns>
        public static int CalPcsIndex(string x, string y, int lastX, int lastY, string dummyRows, string chamfer, string side)
        {
            int result = 0;
            if (x == "" || y == "")
                return result;

            int tempX = StringToInt(x);
            int tempY = StringToInt(y);
            if (dummyRows != "")
            {
                if (tempY > Convert.ToInt32(dummyRows.First()) - 64)
                    tempY = tempY - dummyRows.Length;
            }

            if (tempX > lastX || tempY > lastY)
            {
                throw new Exception("超出索引範圍!");

            }
            if (!(chamfer == "缺角在右下" ^ side == "A"))//反互斥或
                result = (lastY - tempY) * lastX + tempX - 1;
            else
                result = (lastY - tempY) * lastX + lastX - tempX;

            return result;
        }
    }
}
