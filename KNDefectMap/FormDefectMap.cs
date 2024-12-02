//todo show目檢xout
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using KNDefectMap.Properties;
using KNDefectMap.DataModel;
using System.Drawing.Drawing2D;
using KNDefectMap.CS;
//using Impersonator;
using System.Text.RegularExpressions;

namespace KNDefectMap
{
    public partial class FormDefectMap : Form
    {
        string InputCode = "";      //儲存KeyIn值        
        string EngNum = "";
        int PCSX = 0;   //SPNL中行數(cols)
        int PCSY = 0;   //SPNL中列數(rows)
        int SpnlAngle = 0;
        //List<Point> NGList = new List<Point>();
        DataTable NGPcsDT = new DataTable();
        DataTable NGPcsBySideDT;
        DataTable DefectCodeCount = new DataTable();
        DataTable LotData;
        WpnlMap WPNL;
        string DummyRows = "";//空白列
        string DummyCols = "";//空白行
        Color ColorAlwaysShow = Color.Black;
        string[] DefectCodesAlwayShow;
        Label MessageLabel = new Label();
        bool isCSVData = false;        
        string[] DummyRowsArray; //容納DummyRow切出的個別值
        string[] DummyColsArray; //容納DummyCol切出的個別值
        string XoutSideResult; //取得XoutSide

        public FormDefectMap()
        {
            InitializeComponent();

            MessageLabel.ForeColor = Color.Red;
            MessageLabel.Text = "未入庫";

            DefectCodeCount.Columns.Add("標記", typeof(bool));
            DefectCodeCount.Columns.Add("報廢代碼", typeof(string));
            DefectCodeCount.Columns.Add("PCS數", typeof(int));
            dataGridViewDefectCount.Columns.Clear();

            textBoxInput.Focus();
        }

        private void FWasteSearch_Load(object sender, EventArgs e)
        {
            //FLogin frm = new FLogin();
            //if (frm.ShowDialog(this) == DialogResult.OK)
            //{
            //    labelEmployee.Text = frm.EmployeeName;
            //    if (labelEmployee.Text == "")
            //        FWasteSearch_Load(sender, e);
            //    else
            //    {
            //        this.Show();
            //        textBoxInput.Focus();
            //    }
            //    Impersonator.Impersonator impersonator = new Impersonator.Impersonator("admin", "wus", "admin");

            //}
            //else
            //    this.Close();
            textBoxAlwaysShow.Text = Settings.Default.specifyDefectCodes;
            if (Settings.Default.ShowSPNL)
                radioButtonSPNL.Checked = true;
            else
                radioButtonWPNL.Checked = true;

            if (Settings.Default.ShowSideB)
                radioButtonSideB.Checked = true;
            else
                radioButtonSideA.Checked = true;
        }


        //簡碼轉報廢代碼
        private string SimpleCodeToWasteCode(string simpleCode)
        {
            string wasteSimpleCode = simpleCode.Trim('-').Trim();
            object wasteCodeObj = DBAccessor.SearchWasteCode(wasteSimpleCode);
            if (wasteCodeObj == null)
            {
                throw new Exception("「" + simpleCode + "」此報廢代碼未定義!");
                //  MessageBox.Show(wasteSimpleCode + "此報廢代碼未定義!");              
                //  return "";
            }
            else
            {
                if (simpleCode.Contains('-'))
                    return wasteCodeObj.ToString() + '-';
                else
                    return wasteCodeObj.ToString();
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if ((e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar >= 97 && e.KeyChar <= 122) || (e.KeyChar >= 65 && e.KeyChar <= 90) || e.KeyChar=='-') // 0~9 & a~z & -
            //{
            //    InputCode += ((char)e.KeyChar).ToString();
            //}
            //else if (e.KeyChar == 8)//backspace
            //{
            //    InputCode = "";
            //}


            //textBoxBarcodeID.Text = InputCode;

            //if (e.KeyChar == '-')
            //{
            //    // 轉成完整報廢代碼
            //    InputCode = SimpleCodeToWasteCode(InputCode);               
            //}

            //if (e.KeyChar == 13 ) //Enter = 13
            //{
            //    if (InputCode.Trim() == "")
            //        return;

            //    if (InputCode.Length < 10)
            //    {

            //        if (InputCode.Contains("-"))
            //        {
            //            DataRow newRow = DefectCodeCount.NewRow();
            //            string[] inputs = InputCode.Split('-');
            //            newRow["報廢代碼"] = inputs[0];
            //            newRow["PCS數"] = int.Parse(inputs[1]);
            //            DefectCodeCount.Rows.Add(newRow);
            //            dataGridView1.DataSource = DefectCodeCount;
            //        }
            //        else
            //        {
            //            textBoxBarcodeID.Text = "";
            //            //轉成完整報廢代碼
            //            InputCode = SimpleCodeToWasteCode(InputCode);
            //            if (labelSpnlID.Text != "")
            //            {
            //                if (DBAccessor.InsertAllWasteCode(labelSpnlID.Text, InputCode, labelEmployee.Text) == 1)
            //                    labelAllWasteFinish.Text = "全報廢完成";
            //                else
            //                    labelAllWasteFinish.Text = "全報廢失敗";
            //            }
            //            else
            //            {
            //                MessageBox.Show("請先讀入SPNL Barcode!");
            //            }
            //        }
            //    }
            //    else
            //    {
            //        this.Cursor = Cursors.WaitCursor;

            //        labelAlert.Text = "";
            //        CancelHightlight(NGList);
            //      //  if (textBoxBarcodeID.Text.Length == 10)
            //     //   {
            //            labelAllWasteFinish.Text = "";
            //          //  labelID.Text = textBoxBarcodeID.Text.Trim();
            //            labelSpnlID.Text = InputCode;
            //            //載入SPNL目檢輸入的各報廢代碼的PCS數
            //            DefectCodeCount = DBAccessor.GetDefectCodeCount(labelSpnlID.Text);
            //            dataGridView1.DataSource = DefectCodeCount;

            //            object result = DBAccessor.CheckAllWaste(labelSpnlID.Text); //擷取電測全報的報廢代碼
            //            if (result == null)
            //            {
            //                NGList = DBAccessor.GetNGPcsListMySql(labelSpnlID.Text); //取得報廢總清單(OS + VI)
            //                if (NGList.Contains(new Point(0, 0)))
            //                {
            //                    labelXoutNum.Text = "該SPNL GVR已全報廢";
            //                    labelAlert.Text = "注意！" + Environment.NewLine + "該SPNL GVR已全報廢";
            //                    labelEXoutCount.Text = "全報廢";
            //                    labelRemainXoutNum.Text = "0";
            //                }
            //                else
            //                {
            //                    result = DBAccessor.CheckAllWasteByDoubleCheck(labelSpnlID.Text);
            //                    if (result != null)
            //                    {
            //                        labelXoutNum.Text = "該SPNL目檢已全報廢";
            //                        labelAlert.Text = "注意！" + Environment.NewLine + "該SPNL 目檢已全報廢";
            //                        labelEXoutCount.Text = "全報廢";
            //                        labelRemainXoutNum.Text = "0";
            //                    }
            //                    else
            //                    {
            //                        int xoutNum = NGList.Count;
            //                        labelEXoutCount.Text = DBAccessor.GetENGPcsListMySql(labelSpnlID.Text).Count.ToString();   //取得報廢清單(OS)
            //                        labelXoutNum.Text = xoutNum.ToString();

            //                        if (xoutNum > 0)
            //                        {
            //                            //檢查是否有上AIM     //不重撈不含OS手動標記的NG PCS數，以確保OS手動標記的板子也有上AIM重畫，以避免人員併碼時手動標記錯誤，造成板子上畫的與給客戶的XOut電子檔不符。
            //                            if (!DBAccessor.ExistsAIM(labelSpnlID.Text))
            //                            {
            //                                labelAlert.Text = "注意！" + Environment.NewLine + "此SPNL未上AIM";
            //                                labelAlert.BringToFront();
            //                            }


            //                            //int numOfNgPcs = DBAccessor.GetNumOfNGPcs(labelID.Text); //重撈不含手動標記的NG PCS數
            //                            //if (numOfNgPcs > 0)
            //                            //{
            //                            //    if (!DBAccessor.ExistsAIM(labelID.Text))
            //                            //    {
            //                            //        labelAlert.Text = "注意！"+ Environment.NewLine +"此SPNL未上AIM";  
            //                            //        labelAlert.BringToFront();
            //                            //    }
            //                            //}
            //                        }

            //                        string engNumTemp = EngNum;

            //                        DBAccessor.GetSpnlBaseData(labelSpnlID.Text, out EngNum, out PCSX, out PCSY);
            //                        if (engNumTemp != EngNum)
            //                        {
            //                            MakePanel();
            //                            int totalXoutNum = 0;
            //                            totalXoutNum = DBAccessor.GetTotalXoutNum(EngNum);
            //                            if (totalXoutNum != 0)
            //                            {
            //                                labelTotalXoutNum.Text = totalXoutNum.ToString();

            //                            }
            //                            else
            //                            {
            //                                labelTotalXoutNum.Text = "NA";
            //                                labelRemainXoutNum.Text = "NA";
            //                            }
            //                        }
            //                        else
            //                        {
            //                            HightlightPanel(NGList);
            //                        }

            //                        labelRemainXoutNum.Text = (int.Parse(labelTotalXoutNum.Text) - xoutNum).ToString();
            //                        labelEngNum.Text = EngNum;
            //                        //   MakePanel();
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                labelXoutNum.Text = "該SPNL電測已全報廢";
            //                labelAlert.Text = "注意！" + Environment.NewLine + "該SPNL電測已全報廢";
            //                labelEXoutCount.Text = "電測全報廢";
            //                labelRemainXoutNum.Text = "0";
            //            }
            //      //  }
            //        textBoxBarcodeID.Text = "";
            //        InputCode = "";
            //    }
            //      InputCode = "";
            //}
            //this.Cursor = Cursors.Default;  
        }


        private void textBoxBarcodeID_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                isCSVData = false;
                InputCode = textBoxInput.Text.Trim().ToUpper();
                textBoxInput.SelectAll();
                if (InputCode == "")
                    return;

                LotData = DBAccessor.GetWpnlData(InputCode);
                if (LotData.Rows.Count == 0)
                {
                    MessageBox.Show(InputCode + "查無資料！");
                    return;
                }
                labelEngNo.Text = LotData.Rows[0]["cp_rev"].ToString();
                labelLot.Text = LotData.Rows[0]["lot"].ToString().Substring(0, 6);
                if (labelEngNo.Text != EngNum)
                {
                    EngNum = labelEngNo.Text;
                    DBAccessor.GetSpnlBaseData(EngNum, out PCSX, out PCSY);
                    var spnlSize_mm = DBAccessor.GetSPNLSize_mm(EngNum);
                    if (spnlSize_mm[0] > spnlSize_mm[1]) //寬>高
                        SpnlAngle = 0;
                    else
                        SpnlAngle = 90;

                    string outPCSNumFileName = @"\\twnas1\AccessDB\aw\pcs_coordinate\" + labelEngNo.Text + ".txt";
                    if (!File.Exists(outPCSNumFileName))
                    {
                        MessageBox.Show(labelEngNo.Text + "AW未輸出座標檔！");
                        // return resultWpnl;
                    }

                    StreamReader sr = new StreamReader(outPCSNumFileName);
                    string myText = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                    string[] dataArray = myText.Split('\n');

                    DummyRows = dataArray[3].Replace("\r", "");
                    
                    if (DummyRows.Contains("NA"))
                        DummyRows = "";

                    DummyCols = dataArray[4].Replace("\r", "");

                    if (DummyCols.Contains("NA"))
                        DummyCols = "";

                }

                listBoxWPNL.DisplayMember = "wpnl_id";
                listBoxWPNL.ValueMember = "wpnl_id";
                listBoxWPNL.DataSource = LotData.DefaultView.ToTable(true, "wpnl_id");

                XoutSideResult = DBAccessor.GetXoutSide(InputCode);
                
                if (radioButtonSPNL.Checked)
                    MakeSPanel(panelMap);

                if (InputCode.Length == 6)
                {//lot
                 //do nothing
                }
                else if (InputCode.StartsWith("W"))
                {//spnl     
                    listBoxWPNL.SelectedValue = LotData.Select(string.Format(" finish_id='{0}'", InputCode))[0]["wpnl_id"].ToString();
                    listBoxSPNL.SelectedValue = InputCode;
                }
                else
                { //wpnl               
                    listBoxWPNL.SelectedValue = InputCode;
                }

                textBoxInput.Text = "";
                InputCode = "";

            }
        }

        private void ClearSpnlData()
        {
            labelEngNo.Text = "";
            labelLot.Text = "";
            listBoxWPNL.DataSource = null;
            listBoxSPNL.DataSource = null;
            dataGridViewDefectCount.DataSource = null;
            CancelHightlight(NGPcsDT);
        }
        private void UpdateDataGridViewDefectCount(DataTable dt, out DataTable dtBySide)
        {
            string side = radioButtonSideA.Checked ? "A" : "B";
            // DataRow[] rows = dt.Select(string.Format(" side='{0}'", side));
                        
            DataRow[] rows = dt.DefaultView.ToTable(true, "cp_rev", "finish_id", "pcs_index", "x", "y", "side", "defectcode")
                                .Select(string.Format(" side like '{0}%' and defectCode is not null", side));

            if (rows.Length > 0)
            {
                dtBySide = rows.CopyToDataTable();
                DefectCodeCount = rows.AsEnumerable()
                           .GroupBy(r => r["DefectCode"])
                           .Select(g =>
                           {
                               var row = DefectCodeCount.NewRow();
                               row["標記"] = false;
                               row["報廢代碼"] = g.Key;
                               row["PCS數"] = g.Count();
                               return row;
                           })
                           .OrderBy(r => r["報廢代碼"])
                           .CopyToDataTable();
            }
            else
            {
                dtBySide = dt.Clone();
                DefectCodeCount.Clear();
            }


            BindingSource bs = new BindingSource();
            bs.DataSource = DefectCodeCount;
            dataGridViewDefectCount.DataSource = bs;
            textBoxAlwaysShow.Text = textBoxAlwaysShow.Text.Trim().ToUpper();
            DefectCodesAlwayShow = textBoxAlwaysShow.Text.Split(',');
            for (int i = 0; i < dataGridViewDefectCount.Rows.Count; i++)
            {
                if (DefectCodesAlwayShow[0].Length == 0)
                {
                    dataGridViewDefectCount.Rows[i].DefaultCellStyle.BackColor = CS.StaticFunction.RandomColor(i);
                    dataGridViewDefectCount.Rows[i].Cells["標記"].Value = true;
                    //dataGridView資料有修改需加下列兩行，才會將修改內容更新到DataSource.
                    dataGridViewDefectCount.EndEdit();
                    bs.EndEdit();
                }
                else if (DefectCodesAlwayShow.Contains(dataGridViewDefectCount.Rows[i].Cells["報廢代碼"].Value.ToString()))
                {
                    dataGridViewDefectCount.Rows[i].DefaultCellStyle.BackColor = ColorAlwaysShow;
                    dataGridViewDefectCount.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                    dataGridViewDefectCount.Rows[i].Cells["標記"].Value = true;
                    //dataGridView資料有修改需加下列兩行，才會將修改內容更新到DataSource.
                    dataGridViewDefectCount.EndEdit();
                    bs.EndEdit();
                }
                else
                {
                    dataGridViewDefectCount.Rows[i].DefaultCellStyle.BackColor = CS.StaticFunction.RandomColor(i);
                }

            }

        }

        private void LoadData(string input)
        {
            
            if (NGPcsBySideDT != null) NGPcsBySideDT.Clear();
            CancelHightlight(NGPcsDT);

            if (!isCSVData)
            {
                NGPcsDT = DBAccessor.GetXOut_AVI(input);
                DataTable dtOS = DBAccessor.GetXOut_OS(input);
                DataTable dtHuman = DBAccessor.GetXOut_human(input);

                if (NGPcsDT.Rows.Count != 0 || dtOS.Rows.Count != 0 || dtHuman.Rows.Count != 0)
                {
                    NGPcsDT = NGPcsDT.AsEnumerable()
                             .Union(dtOS.AsEnumerable())
                             .Union(dtHuman.AsEnumerable())
                             .CopyToDataTable()
                             ;
                }
                   
            }

            if (NGPcsDT.Rows.Count == 0)
            {
                dataGridViewDefectCount.DataSource = null;

                this.Cursor = Cursors.Default;
                if (radioButtonSPNL.Checked && input == textBoxInput.Text)
                    MessageBox.Show("目前選擇的SPNL查無資料！");
                return;
            }
            labelEngNo.Text = NGPcsDT.Rows[0]["cp_rev"].ToString();
            labelLot.Text = NGPcsDT.Rows[0]["lot"].ToString().Trim().Substring(0, 6);

            //列出檢測機台
            string detector = "";
            if (NGPcsDT.Columns.Contains("detector"))
            {
                DataTable dt = NGPcsDT.DefaultView.ToTable(true, "detector");

                foreach (DataRow r in dt.Rows)
                {
                    detector += string.Format("{0},", r["detector"]);
                }
                detector = detector.Trim(',', ' ');
                labelMachine.Text = detector;
            }

            UpdateDataGridViewDefectCount(NGPcsDT, out NGPcsBySideDT);

        }

        private void SPNL_0(ref SpnlMap spnlMap)
        {
            int dummyRowCount = 0;
            if (DummyRows.Trim() != "")
                dummyRowCount = DummyRows.Split(';').Length;

            int dummyColCount = 0;
            if (DummyCols.Trim() != "")
                dummyColCount = DummyCols.Split(';').Length;

            //容納DummyRow切出的個別值
            DummyRowsArray = DummyRows.Split(';');
            //容納DummyCol切出的個別值
            DummyColsArray = DummyCols.Split(';');

            string str = DummyRowsArray[0].Trim();
            bool isEn = Regex.IsMatch(str, @"^[A-Z]+$"); //有值且為英文
            bool isInt = Regex.IsMatch(str, @"^\+?[1-9][0-9]*$"); //有值且為數字
            
            for (int j = 1; j <= PCSY + dummyRowCount; j++)
            {
                //if (DummyRows.Contains(Convert.ToChar(65 + j)))
                //原碼(英文)
                if (DummyRows.Contains(Convert.ToChar(65 + (PCSY + dummyRowCount - j))))   //SPNL缺角在下字母為倒序，需轉成倒算，Row不對稱時才不會算錯
                    continue;

                //數字
                else if (DummyRowsArray.Contains((PCSY + dummyRowCount - (j - 1)).ToString()))
                    continue;

                int y = j;
                
                if (isEn == true && DummyRows.Length > 0 && j + 65 > Convert.ToInt32(DummyRows[0]))
                    y -= dummyRowCount;

                else if (isInt == true && j > int.Parse(DummyRowsArray[0]))
                    y -= dummyRowCount;

                for (int k = 1; k <= PCSX + dummyColCount; k++)
                {
                    int x = k;

                    //wpnl產生X空白列
                    //英文
                    if (DummyCols.Contains(Convert.ToChar(65 + (PCSX + dummyColCount - k))))   //SPNL缺角在下字母為倒序，需轉成倒算，Row不對稱時才不會算錯
                        continue;
                    //數字
                    else if (DummyColsArray.Contains((PCSX + dummyColCount - (k - 1)).ToString()))
                        continue;

                    //X座標去掉dummyCol
                    //英文
                    if (isInt == true && DummyCols.Length > 0 && k + 65 <= Convert.ToInt32(DummyCols[0]))
                    {                        
                        x -= dummyColCount;
                    }
                    //數字
                    else if (isEn == true && DummyCols.Length > 0 && k <= int.Parse(DummyColsArray[0]))
                    {                        
                        x -= dummyColCount;
                    }

                    Rectangle rect = new Rectangle();
                    Point p = new Point(x, y);
                    rect.X = 8 * (k);
                    rect.Y = 8 * (j);//調整距離不需減掉空白行, 所以用j
                    rect.Width = 7;
                    rect.Height = 7;

                    DataRow[] rows = NGPcsBySideDT.Select(string.Format("finish_id='{0}' and X='{1}' and Y='{2}' ", spnlMap.SpnlID, p.X, p.Y));
                    foreach (DataRow row in rows)
                    {
                        string defectCode = row["defectcode"].ToString();
                        if (!spnlMap.PcsListByDefectCode.ContainsKey(defectCode))
                        {
                            List<Rectangle> list = new List<Rectangle>();
                            list.Add(rect);
                            
                            spnlMap.PcsListByDefectCode.Add(defectCode, list);
                        }
                        else
                        {
                            spnlMap.PcsListByDefectCode[defectCode].Add(rect);
                        }
                    }
                    if (rows.Length == 0)
                    {
                        spnlMap.PcsListByDefectCode["NoDefect"].Add(rect);
                    }

                }
            }
        }
        //private void SPNL_90(ref SpnlMap spnlMap)
        //{//todo
        //    for (int labelY = 0; labelY < PCSX; labelY++)
        //    {
        //        int y = labelY;
        //        for (int labelX = 0; labelX < PCSY + DummyRows.Split(';').Length; labelX++)
        //        {
        //            if (DummyRows.Contains(Convert.ToChar(65 + labelX)))
        //                continue;

        //            int x = labelX;
        //            if (DummyRows.Length > 0 && labelX + 65 > Convert.ToInt32(DummyRows[0]))
        //                x -= DummyRows.Split(';').Length;

        //            Rectangle rect = new Rectangle();
        //            Point p = new Point(x + 1, y + 1);
        //            rect.X = 8 * (x + 1);
        //            rect.Y = 8 * (j + 1);//調整距離不需減掉空白行, 所以用j
        //            rect.Width = 7;
        //            rect.Height = 7;

        //            DataRow[] rows = NGPcsBySideDT.Select(string.Format("finish_id='{0}' and X='{1}' and Y='{2}' ", spnlMap.SpnlID, p.X, p.Y));
        //            foreach (DataRow row in rows)
        //            {
        //                string defectCode = row["defectcode"].ToString();
        //                if (!spnlMap.PcsListByDefectCode.ContainsKey(defectCode))
        //                {
        //                    List<Rectangle> list = new List<Rectangle>();
        //                    list.Add(rect);
        //                    spnlMap.PcsListByDefectCode.Add(defectCode, list);
        //                }
        //                else
        //                {
        //                    spnlMap.PcsListByDefectCode[defectCode].Add(rect);
        //                }
        //            }
        //            if (rows.Length == 0)
        //            {
        //                spnlMap.PcsListByDefectCode["NoDefect"].Add(rect);
        //            }

        //        }
        //    }
        //}
        private WpnlMap MakeWPanel(string wpnlID)//,int spnlAngle)
        {
            WpnlMap resultWpnl;
            DataTable wpnlData = LotData.Select(string.Format("wpnl_id='{0}' ", wpnlID)).OrderBy(r => r["spnl_seq"]).CopyToDataTable();

            resultWpnl = new WpnlMap(wpnlData.Rows.Count);
            int dummyRowCount = 0;
            if (DummyRows.Trim() != "")
                dummyRowCount = DummyRows.Split(';').Length;

            foreach (DataRow r in wpnlData.Rows)
            {
                //spnl
                SpnlMap spnlMap = new SpnlMap(int.Parse(r["spnl_seq"].ToString()), r["finish_id"].ToString(), PCSX, PCSY, dummyRowCount);
                resultWpnl.SPNLs[spnlMap.SEQ] = spnlMap;
                if (NGPcsDT.Select("finish_id='" + r["finish_id"].ToString() + "' and x=0 and station='OS2'").Length > 0)
                    spnlMap.Message = "電測SPNL報廢";
                if (NGPcsDT.Select("finish_id='" + r["finish_id"].ToString() + "'").Length == 0)
                {
                    spnlMap.Message = "無XOut資料";
                }

                if (r["flag"] == DBNull.Value || r["flag"].ToString() == "0")
                {
                    spnlMap.Message += ", 未入庫";
                    spnlMap.Message = spnlMap.Message.TrimStart(',', ' ');
                }


                //// 依條件判斷是否將SPNL轉90度
                //switch(spnlAngle)
                //{
                //    case 0:
                SPNL_0(ref spnlMap);
                //        break;
                //    case 90:
                //        SPNL_90(ref spnlMap);
                //        break;
                //    default:
                //        throw new Exception("SPNL Angle: 「"+spnlAngle+"」未處置！");
                //}                

            }


            return resultWpnl;
        }

        private void MakeSPanel(Control container)
        {
            //無效
            this.SuspendLayout();
            container.SuspendLayout();

            container.Controls.Clear();
            Label myLabel = new Label();
            ToolTip tipType = new ToolTip();
            string filter = string.Format("finish_id='{0}' and flag=1", listBoxSPNL.SelectedValue.ToString());
            if (LotData.Select(filter).Length == 0)
            {
                MessageLabel.Text = "未入庫";
            }
            else
            {
                MessageLabel.Text = "";
            }
            container.Controls.Add(MessageLabel);

            int dummyRowCount = 0;
            if (DummyRows.Trim() != "")
                dummyRowCount = DummyRows.Split(';').Length;
            
            int dummyColCount = 0;
            if (DummyCols.Trim() != "")
                dummyColCount = DummyCols.Split(';').Length;

            //容納DummyRow切出的個別值
            DummyRowsArray = DummyRows.Split(';');
            //容納DummyCol切出的個別值
            DummyColsArray = DummyCols.Split(';');

            string str = DummyRowsArray[0].Trim();
            bool isEn = Regex.IsMatch(str, @"^[A-Z]+$"); //有值且為英文
            bool isInt = Regex.IsMatch(str, @"^\+?[1-9][0-9]*$"); //有值且為數字

            //X板邊號
            List<string> xSideLengthEn = GetEnSideLength(65, PCSX + dummyColCount);
            List<int> xSideLengthInt = GetIntSideLength(1, PCSX + dummyColCount);

            if (XoutSideResult.Contains("TOP")) //需倒序
            {
                if (isEn) //Y側為英文
                {
                    xSideLengthInt.Reverse(); //X側則為數字
                }
                else if (isInt)
                {
                    xSideLengthEn.Reverse();
                }
                else if (/*dummyRowCount == 0 &&*/ dummyColCount == 0)
                {
                    xSideLengthInt.Reverse();
                }

            }
            
            for (int i = 0; i < PCSX + dummyColCount; i++)
            {
                myLabel = new Label();

                if (isEn) //若Y側為英文則X側為數字
                {
                    myLabel.Text = xSideLengthInt[i].ToString();
                }
                else if (isInt)
                {
                    myLabel.Text = xSideLengthEn[i].ToString();
                }
                else if (dummyColCount == 0)
                {
                    myLabel.Text = xSideLengthInt[i].ToString();
                }

                myLabel.Left = 10 + 19 * (i + 1);
                myLabel.Top = 20;
                myLabel.Width = 20;
                myLabel.Height = 15;
                myLabel.Font = new Font("新細明體", 8, FontStyle.Regular, GraphicsUnit.Point, 136);
                myLabel.TextAlign = ContentAlignment.MiddleCenter;
                container.Controls.Add(myLabel);
            }
           
            //Y板邊號
            List<string> ySideLengthEn = GetEnSideLength(65, PCSY + dummyRowCount); //英文
            List<int> ySideLengthInt = GetIntSideLength(1, PCSY + dummyRowCount); //數字

            //預設倒序
            if (isEn) //英文
            {
                ySideLengthEn.Reverse();
            }
            else if (isInt) //數字
            {
                ySideLengthInt.Reverse();
            }
            else if (dummyRowCount == 0)
            {
                ySideLengthInt.Reverse();
            }

            for (int i = 0; i < PCSY + dummyRowCount; i++)
            {
                myLabel = new Label();
                //myLabel.Text = PcsPosFun.IntToString(PCSY - i);

                if (isEn)
                {
                    myLabel.Text = ySideLengthEn[i].ToString(); //英文
                }
                else if (isInt)
                {
                    myLabel.Text = ySideLengthInt[i].ToString(); //數字
                }
                else if (dummyRowCount == 0)
                {
                    myLabel.Text = ySideLengthInt[i].ToString();
                }

                myLabel.Left = 10;
                myLabel.Top = 20 + 20 * (i + 1);
                myLabel.Width = 20;
                myLabel.Height = 15;
                myLabel.Font = new Font("新細明體", 8, FontStyle.Regular, GraphicsUnit.Point, 136);
                myLabel.TextAlign = ContentAlignment.MiddleCenter;
                container.Controls.Add(myLabel);
            }

            //設定提示效果
            tipType.Active = true;  //取得或設定值，指出工具提示目前是否在作用中。
            tipType.AutomaticDelay = 200;  //取得或設定工具提示的自動延遲。
            tipType.AutoPopDelay = 0;   //取得或設定當指標靜止於含指定工具提示文字的控制項上時，工具提示保持可見的時間。
            tipType.BackColor = Color.Black;   //取得或設定工具提示的背景色彩。
            tipType.ForeColor = Color.Lime;   //取得或設定工具提示的前景色彩。
            tipType.InitialDelay = 1;    //取得或設定在工具提示出現之前，所經過的時間。
            tipType.IsBalloon = true;   //取得或設定值，指出工具提示是否應該使用汽球樣式的視窗。
            tipType.OwnerDraw = true;   //取得或設定值，指出要由作業系統繪製工具提示或是由您提供的程式碼繪製。
            tipType.ReshowDelay = 1;     //(取得或設定當指標從某個控制項移動到另一個控制項時, 在後續工具提示視窗出現之前, 必須經)
            tipType.ShowAlways = true;  //取得或設定值，指出即使父控制項為非現用時，是否也會顯示工具提示視窗。
            tipType.StripAmpersands = true; //取得或設定值，以便判斷連字號 (&) 字元的處理方式。
            tipType.ToolTipTitle = "XY位置";  //取得或設定工具提示視窗的標題。
            tipType.UseAnimation = true;    //取得或設定值，以便判斷顯示工具提示時是否應該使用動畫效果。
            tipType.UseFading = true;   //取得或設定值，以便判斷顯示工具提示時是否應該使用淡出效果。
            tipType.IsBalloon = true; //取得或設定值，指出工具提示是否應該使用汽球樣式的視窗。

            // int totalRow = PCSY - DummyRows.Split(';').Length;

            for (int j = 1; j <= PCSY + dummyRowCount; j++)
            {
                //if (DummyRows.Contains(Convert.ToChar(65 + j)))
                //原碼(英文)
                if (DummyRows.Contains(Convert.ToChar(65 + (PCSY + dummyRowCount - j))))   //SPNL缺角在下字母為倒序，需轉成倒算，Row不對稱時才不會算錯
                    continue;

                //數字
                else if (DummyRowsArray.Contains((PCSY + dummyRowCount - (j - 1)).ToString()))
                    continue;

                for (int k = 1; k <= PCSX + dummyColCount; k++)
                {
                    int y = j;
                    int x = k;

                    //spnl產生X空白列
                    //英文
                    if (DummyCols.Contains(Convert.ToChar(65 + (PCSX + dummyColCount - k))))   //SPNL缺角在下字母為倒序，需轉成倒算，Row不對稱時才不會算錯
                        continue;
                    //數字
                    else if (DummyColsArray.Contains((PCSX + dummyColCount - (k - 1)).ToString()))                    
                        continue;

                    //使座標正確(避免dummyRow & Col數量列入座標值計算)
                    //英文
                    if (isEn == true && DummyRows.Length > 0 && j + 65 > Convert.ToInt32(DummyRows[0]))
                    {                        
                        y -= dummyRowCount;
                    }
                    //數字
                    else if (isInt == true && j > int.Parse(DummyRowsArray[0]))
                    {                        
                        y -= dummyRowCount;
                    }

                    //X座標去掉dummyCol
                    //英文
                    if (isInt == true && DummyCols.Length > 0 && k + 65 <= Convert.ToInt32(DummyCols[0]))
                    {                        
                        x -= dummyColCount;                                            
                    }
                    //數字
                    else if (isEn == true && DummyCols.Length > 0 && k <= int.Parse(DummyColsArray[0]))
                    {                        
                        x -= dummyColCount;
                    }

                    Button myButton = new Button();
                    myButton.SuspendLayout();
                    //Point p = new Point(x , y );
                    myButton.Name = (x).ToString() + "_" + (y).ToString();
                    myButton.Left = 19 * (k) + 12;
                    myButton.Top = 20 * (j) + 20;//調整距離不需減掉空白行
                    myButton.Width = 15;
                    myButton.Height = 15;

                    myButton.BackColor = Color.Silver;
                    myButton.FlatStyle = FlatStyle.Standard;
                    //myButton.MouseDoubleClick += PcsButton_MouseDoubleClick;
                    myButton.MouseClick += MyButton_MouseClick;

                    container.Controls.Add(myButton);
                    myButton.ResumeLayout(false);
                    string custXY = "; 客戶座標: " + (PCSX - x + 1).ToString() + "_" + (PCSY - y + 1).ToString();
                    tipType.SetToolTip(myButton, (x).ToString() + "_" + (y).ToString() + custXY);

                }
            }
            
            //無效 
            container.ResumeLayout(false);
            this.ResumeLayout(false);

            HightlightPanel(NGPcsBySideDT);

        }

        /// <summary>
        /// 取得英文邊號
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private List<string> GetEnSideLength(int start, int end)
        {
            List<string> enList = new List<string>();
            List<string> enListExceedZ = new List<string>();

            for (int i = 0; i < end; i++)
            {
                enList.Add((Convert.ToChar(start + i)).ToString());
            }

            //超出範圍
            if (end > 26)
            {
                for (int i = 0; i < end; i++)
                {
                    enListExceedZ.Add((Convert.ToChar(start + i)).ToString() + (Convert.ToChar(start + i)).ToString());
                }
                enList.InsertRange(26, enListExceedZ);
                enList.RemoveRange(end, enList.Count - end);
            }
            return enList;
        }

        /// <summary>
        /// 取得數字邊號
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private List<int> GetIntSideLength(int start, int end)
        {
            List<int> intList = new List<int>();

            for (int i = 0; i < end; i++)
            {
                intList.Add(start + i);
            }
            return intList;
        }

        private void MyButton_MouseClick(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            string spnlID = listBoxSPNL.SelectedValue.ToString();
            string[] pcsXY = button.Name.Split('_');
            string side = radioButtonSideA.Checked ? "A" : "B";
            //查詢
            string xoutPcsImagePath = DBAccessor.GetXOutImage(spnlID, pcsXY[0], pcsXY[1], side);
            //變更NAS ip
            if (xoutPcsImagePath.StartsWith(@"\\192"))
            {
                char[] splitChars = new char[] { '\\' };
                string[] pathSplit = xoutPcsImagePath.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
                pathSplit[0] = @"\\172.26.61.218";
                xoutPcsImagePath = Path.Combine(pathSplit);
            }
            string frmText = string.Join("_", new string[] { spnlID, side, pcsXY[0], pcsXY[1] });

            //載入影像
            Image pcsImage;
            using (Impersonator.Impersonator impersonator = new Impersonator.Impersonator("gvireader", "127.0.0.1", "fY#5DNR69hpP")) //"app", "127.0.0.1", "wuswus"
            {
                if (File.Exists(xoutPcsImagePath))
                {
                    pcsImage = Image.FromFile(xoutPcsImagePath);
                }
                else
                {
                    MessageBox.Show("X-Out PCS 影像不存在！", frmText);
                    return;
                }
            }
            //載入PCS 缺陷位置
            string defectType;
            DataTable defectLocationDT = DBAccessor.GetDefectLocation(spnlID, pcsXY[0], pcsXY[1], side);
            Dictionary<string, List<Rectangle>> defectCirclesByDefectCode = CreateDefectCircle(defectLocationDT, out defectType);
            DrawCircles(Graphics.FromImage(pcsImage), defectCirclesByDefectCode, dataGridViewDefectCount);

            //顯示影像
            Form frm = new Form
            {
                Text = frmText,
                Width = pcsImage.Width,
                Height = pcsImage.Height,
                BackgroundImageLayout = ImageLayout.Zoom,
                BackgroundImage = pcsImage
            };

            ToolTip toolTip2 = new ToolTip
            {
                InitialDelay = 100,
                ShowAlways = true
            };
            toolTip2.SetToolTip(frm, defectType);

            frm.Show(this);
        }

        private Dictionary<string, List<Rectangle>> CreateDefectCircle(DataTable defectLocationDT, out string defectType)
        {
            defectType = "";
            Dictionary<string, List<Rectangle>> defectCirclesByDefectCode = new Dictionary<string, List<Rectangle>>();

            foreach (DataRow row in defectLocationDT.Rows)
            {
                Rectangle rect = new Rectangle();
                rect.X = int.Parse(row["orgX"].ToString());
                rect.Y = int.Parse(row["orgY"].ToString());
                rect.Width = int.Parse(row["Width"].ToString());
                rect.Height = int.Parse(row["Height"].ToString());
                string defectCode = row["defectcode"].ToString();
                if (!defectCirclesByDefectCode.ContainsKey(defectCode))
                {
                    List<Rectangle> list = new List<Rectangle>();
                    list.Add(rect);
                    defectCirclesByDefectCode.Add(defectCode, list);
                }
                else
                {
                    defectCirclesByDefectCode[defectCode].Add(rect);
                }

                defectType += row["defectcode"].ToString() + row["item"].ToString() + Environment.NewLine;
            }
            defectType = defectType.Trim();
            return defectCirclesByDefectCode;
        }
        /// <summary>
        /// 在指定的Graphics中依指定的顏色畫圈圈。
        /// </summary>
        /// <param name="g"></param>
        /// <param name="defectLocations">依報廢代碼分類的缺陷位置</param>
        /// <param name="itemColor">含有報廢代碼與顏色的DataGridView</param>
        private void DrawCircles(Graphics g, Dictionary<string, List<Rectangle>> defectLocations, DataGridView itemColor)
        {

            foreach (KeyValuePair<string, List<Rectangle>> defects in defectLocations)
            {
                if (defects.Value.Count == 0) continue;

                Color color = StaticFunction.GetDefectCodeColor(defects.Key, itemColor);
                SolidBrush blueBrush = new SolidBrush(color);
                GraphicsPath defectCircles = new GraphicsPath();
                foreach (Rectangle defect in defects.Value)
                {
                    defectCircles.AddEllipse(defect);
                }
                if (color != Color.Silver)
                    g.DrawPath(new Pen(blueBrush, 3), defectCircles);

            }
        }

        private void HightlightPanel(List<Point> ngList)
        {
            if (ngList == null) return;

            foreach (Point p in ngList)
            {
                string buttonName = p.X.ToString() + "_" + p.Y.ToString();
                Button btn = (Button)panelMap.Controls[buttonName];
                if (btn != null)
                {
                    btn.BackColor = Color.Red;
                    btn.FlatStyle = FlatStyle.Popup;
                }
            }
        }
        private void HightlightPanel(DataTable ngList)
        {
            if (ngList == null || ngList.Rows.Count == 0) return;

            foreach (DataRow p in ngList.Rows)            
            {
                string buttonName = p["X"].ToString() + "_" + p["Y"].ToString();
                Button btn = (Button)panelMap.Controls[buttonName];
                if (btn != null)
                {
                    if (btn.BackColor != ColorAlwaysShow)
                    {
                        Color c = CS.StaticFunction.GetDefectCodeColor(p["defectcode"].ToString(), dataGridViewDefectCount);
                        if (c == Color.Silver && btn.BackColor != Color.Silver)
                            continue;
                        else
                            btn.BackColor = c;
                    }

                    btn.FlatStyle = FlatStyle.Popup;

                }
            }
        }

        //private Color GetDefectCodeColor(string defectCode)
        //{
        //    Color c = Color.Silver;
        //    object dataSource = dataGridViewDefectCount.DataSource;
        //    int rid;
        //    switch (dataSource)
        //    {
        //        case DataTable dt:                   
        //            DataRow[] rows = dt.Select("報廢代碼='" + defectCode + "' and 標記=True");

        //            if (rows.Length == 0) return c;

        //            rid= dt.Rows.IndexOf(rows[0]);
        //            c = dataGridViewDefectCount.Rows[rid].DefaultCellStyle.BackColor;
        //            break;
        //        case BindingSource bs:
        //            DataTable dataTable =(DataTable) bs.DataSource;
        //            DataRow[] r = dataTable.Select("報廢代碼='" + defectCode + "' and 標記=True");

        //            if (r.Length == 0) return c;

        //            rid = dataTable.Rows.IndexOf(r[0]);
        //            c = dataGridViewDefectCount.Rows[rid].DefaultCellStyle.BackColor;
        //            break;
        //        default:
        //            throw new Exception(dataSource.GetType().ToString()+"未處置！"); 
        //    }


        //    return c;
        //}
        private void CancelHightlight(List<Point> ngList)
        {
            foreach (Point p in ngList)
            {
                string buttonName = p.X.ToString() + "_" + p.Y.ToString();
                Button btn = (Button)panelMap.Controls[buttonName];
                if (btn == null) continue;
                btn.BackColor = Color.Silver;
                btn.FlatStyle = FlatStyle.Standard;
            }
        }
        private void CancelHightlight(DataTable ngList)
        {
            if (ngList == null) return;

            foreach (DataRow p in ngList.Rows)
            {
                string buttonName = p["X"].ToString() + "_" + p["Y"].ToString();
                Button btn = (Button)panelMap.Controls[buttonName];
                if (btn == null) continue;
                btn.BackColor = Color.Silver;
                btn.FlatStyle = FlatStyle.Standard;
            }
        }

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                dataGridViewDefectCount.ClearSelection();
                dataGridViewDefectCount.Rows[e.RowIndex].Selected = true;
            }

        }



        private void dataGridViewDefectCount_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewDefectCount.Columns[e.ColumnIndex].GetType() == typeof(DataGridViewCheckBoxColumn))
            {
                bool chkValue = !(bool)dataGridViewDefectCount[e.ColumnIndex, e.RowIndex].Value;
                dataGridViewDefectCount[e.ColumnIndex, e.RowIndex].Value = chkValue;
                dataGridViewDefectCount.EndEdit();
                BindingSource bs = (BindingSource)dataGridViewDefectCount.DataSource;
                bs.EndEdit();

                DataTable dt = NGPcsBySideDT.Select(string.Format("defectcode='{0}' "
                                                            , dataGridViewDefectCount["報廢代碼", e.RowIndex].Value.ToString()
                                                            )
                                             ).CopyToDataTable();


                if (radioButtonSPNL.Checked)
                {
                    if (chkValue)
                        HightlightPanel(dt);
                    else
                    {
                        CancelHightlight(dt);
                        HightlightPanel(NGPcsBySideDT);
                    }


                }
                else
                {
                    WPNL = MakeWPanel(listBoxWPNL.SelectedValue.ToString());
                    WPNL.Draw(panelMap, dataGridViewDefectCount, DefectCodesAlwayShow, listBoxSPNL.SelectedValue.ToString(), SpnlAngle);
                }
            }

        }

        private void listBoxWPNL_SelectedIndexChanged(object sender, EventArgs e)
        {
            // string wpnlID = ((DataRowView)listBoxWPNL.SelectedValue)["wpnl_id"].ToString();
            string wpnlID = listBoxWPNL.SelectedValue.ToString();
            DataTable dt = LotData.Select("wpnl_id='" + wpnlID + "'")
                                               .CopyToDataTable();

            listBoxSPNL.DisplayMember = "finish_id";
            listBoxSPNL.ValueMember = "finish_id";
            listBoxSPNL.DataSource = dt;
            //   listBoxSPNL.SelectedIndex = 0;
            if (radioButtonWPNL.Checked)
            {
                LoadData(listBoxWPNL.SelectedValue.ToString());
                WPNL = MakeWPanel(listBoxWPNL.SelectedValue.ToString());
                WPNL.Draw(panelMap, dataGridViewDefectCount, DefectCodesAlwayShow, listBoxSPNL.SelectedValue.ToString(), SpnlAngle);
            }

        }

        private void listBoxSPNL_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxSPNL.SelectedValue == null) return;


            if (radioButtonSPNL.Checked)
            {
                CancelHightlight(NGPcsBySideDT);
                LoadData(listBoxSPNL.SelectedValue.ToString());
                HightlightPanel(NGPcsBySideDT);

                string filter = string.Format("finish_id='{0}' and flag=1", listBoxSPNL.SelectedValue.ToString());
                if (LotData.Select(filter).Length == 0)
                {
                    MessageLabel.Text = "未入庫";
                }
                else
                {
                    MessageLabel.Text = "";
                }
            }
            else
            {
                if (WPNL != null)
                    WPNL.Draw(panelMap, dataGridViewDefectCount, DefectCodesAlwayShow, listBoxSPNL.SelectedValue.ToString(), SpnlAngle);
            }

        }

        private void radioButtonSideA_CheckedChanged(object sender, EventArgs e)
        {
            if (NGPcsDT == null || NGPcsDT.Rows.Count == 0) return;

            if (radioButtonSPNL.Checked)
            {
                CancelHightlight(NGPcsDT);
                UpdateDataGridViewDefectCount(NGPcsDT, out NGPcsBySideDT);
                HightlightPanel(NGPcsBySideDT);
            }
            else
            {
                if (listBoxWPNL.SelectedValue == null) return;
                LoadData(listBoxWPNL.SelectedValue.ToString());
                WPNL = MakeWPanel(listBoxWPNL.SelectedValue.ToString());
                WPNL.Draw(panelMap, dataGridViewDefectCount, DefectCodesAlwayShow, listBoxSPNL.SelectedValue.ToString(), SpnlAngle);
            }
        }

        private void radioButtonSPNL_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSPNL.Checked)
            {
                if (listBoxSPNL.SelectedValue == null) return;
                LoadData(listBoxSPNL.SelectedValue.ToString());
                MakeSPanel(panelMap);
            }
            else
            {
                if (listBoxWPNL.SelectedValue == null) return;
                LoadData(listBoxWPNL.SelectedValue.ToString());
                WPNL = MakeWPanel(listBoxWPNL.SelectedValue.ToString());
                WPNL.Draw(panelMap, dataGridViewDefectCount, DefectCodesAlwayShow, listBoxSPNL.SelectedValue.ToString(), SpnlAngle);
            }
        }

        private void FormDefectMap_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default.specifyDefectCodes = textBoxAlwaysShow.Text.Trim().ToUpper();
            Settings.Default.ShowSPNL = radioButtonSPNL.Checked;
            Settings.Default.ShowSideB = radioButtonSideB.Checked;
            Settings.Default.Save();
        }

        private void buttonLoadCSV_Click(object sender, EventArgs e)
        {
            OpenFileDialog openImageFile = new OpenFileDialog();
            //openImageFile.Filter = "Image Files(*.BMP;*.JPG;*.TIF;*.PNG;*.jpeg)|*.BMP;*.JPG;*.TIF;*.PNG;*.jpeg";
            openImageFile.FileName = "";
            if (openImageFile.ShowDialog() == DialogResult.Cancel)
                return;

            isCSVData = true;
            string filePath = openImageFile.FileName;
            LoadCSVData(filePath);
            //LoadCSVData();

        }

        private void LoadCSVData(string path)
        {
            NGPcsDT = CS.CSVAccessor.GetDataTableFromCsv(path, true);
            string msg = "";
            string[] cols = new string[] { "cp_rev", "Lot", "WPNL_id", "finish_id", "spnl_seq", "pcs_index", "X", "Y", "side", "defectCode", "flag" };
            foreach (var c in cols)
            {
                if (!NGPcsDT.Columns.Contains(c))
                    msg = string.Format("{0},{1} ", msg, c);
            }
            msg = msg.Trim(',');
            if (msg != "")
            {
                MessageBox.Show("CSV檔少了下列欄位！" + Environment.NewLine + msg, "系統訊息");
                return;
            }

            LotData = NGPcsDT.DefaultView.ToTable(true, "cp_rev", "Lot", "WPNL_id", "finish_id", "spnl_seq", "flag");
            if (LotData.Rows.Count == 0)
            {
                MessageBox.Show(InputCode + "無資料！");
                return;
            }
            labelEngNo.Text = LotData.Rows[0]["cp_rev"].ToString().ToUpper();
            labelLot.Text = LotData.Rows[0]["lot"].ToString().Substring(0, 6);
            if (labelEngNo.Text != EngNum)
            {
                EngNum = labelEngNo.Text;
                DBAccessor.GetSpnlBaseData(EngNum, out PCSX, out PCSY);
                var spnlSize_mm = DBAccessor.GetSPNLSize_mm(EngNum);
                if (spnlSize_mm[0] > spnlSize_mm[1])
                    SpnlAngle = 0;
                else
                    SpnlAngle = 90;

                string outPCSNumFileName = @"\\twnas1\AccessDB\aw\pcs_coordinate\" + labelEngNo.Text + ".txt";
                if (!File.Exists(outPCSNumFileName))
                {
                    MessageBox.Show(labelEngNo.Text + "AW未輸出座標檔！");
                    // return resultWpnl;
                }

                StreamReader sr = new StreamReader(outPCSNumFileName);
                string myText = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
                string[] dataArray = myText.Split('\n');

                DummyRows = dataArray[3].Replace("\r", "");
                //if (!DummyRows.Contains("NA"))
                //    PCSY += DummyRows.Split(';').Length;
                //else
                //    DummyRows = "";
                if (DummyRows.Contains("NA"))
                    DummyRows = "";

            }

            listBoxWPNL.DisplayMember = "wpnl_id";
            listBoxWPNL.ValueMember = "wpnl_id";
            listBoxWPNL.DataSource = LotData.DefaultView.ToTable(true, "wpnl_id");


            if (radioButtonSPNL.Checked)
                MakeSPanel(panelMap);

            //if (InputCode.Length == 6)
            //{//lot
            // //do nothing
            //}
            //else if (InputCode.StartsWith("W"))
            //{//spnl     
            //    listBoxWPNL.SelectedValue = LotData.Select(string.Format(" finish_id='{0}'", InputCode))[0]["wpnl_id"].ToString();
            //    listBoxSPNL.SelectedValue = InputCode;
            //}
            //else
            //{ //wpnl               
            //    listBoxWPNL.SelectedValue = InputCode;
            //}

        }
    }
}
