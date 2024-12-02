namespace KNDefectMap
{
    partial class FormDefectMap
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxInput = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.labelLot = new System.Windows.Forms.Label();
            this.labelEngNo = new System.Windows.Forms.Label();
            this.dataGridViewDefectCount = new System.Windows.Forms.DataGridView();
            this.ColumnVisible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column報廢代碼 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPCS數 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxAlwaysShow = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButtonSPNL = new System.Windows.Forms.RadioButton();
            this.radioButtonWPNL = new System.Windows.Forms.RadioButton();
            this.listBoxWPNL = new System.Windows.Forms.ListBox();
            this.listBoxSPNL = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButtonSideB = new System.Windows.Forms.RadioButton();
            this.radioButtonSideA = new System.Windows.Forms.RadioButton();
            this.checkBoxScrapByHuman = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelMap = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.labelMachine = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.buttonLoadCSV = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDefectCount)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "工號";
            // 
            // textBoxInput
            // 
            this.textBoxInput.Location = new System.Drawing.Point(49, 12);
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.Size = new System.Drawing.Size(125, 22);
            this.textBoxInput.TabIndex = 1;
            this.toolTip1.SetToolTip(this.textBoxInput, "請輸入6碼批號或SPNL 2D Code");
            this.textBoxInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxBarcodeID_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "批號";
            // 
            // labelLot
            // 
            this.labelLot.AutoSize = true;
            this.labelLot.Location = new System.Drawing.Point(58, 65);
            this.labelLot.Name = "labelLot";
            this.labelLot.Size = new System.Drawing.Size(41, 12);
            this.labelLot.TabIndex = 15;
            this.labelLot.Text = "000000";
            // 
            // labelEngNo
            // 
            this.labelEngNo.AutoSize = true;
            this.labelEngNo.Location = new System.Drawing.Point(57, 48);
            this.labelEngNo.Name = "labelEngNo";
            this.labelEngNo.Size = new System.Drawing.Size(36, 12);
            this.labelEngNo.TabIndex = 16;
            this.labelEngNo.Text = "Z0001";
            // 
            // dataGridViewDefectCount
            // 
            this.dataGridViewDefectCount.AllowUserToAddRows = false;
            this.dataGridViewDefectCount.AllowUserToDeleteRows = false;
            this.dataGridViewDefectCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridViewDefectCount.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridViewDefectCount.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDefectCount.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnVisible,
            this.Column報廢代碼,
            this.ColumnPCS數});
            this.dataGridViewDefectCount.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewDefectCount.Location = new System.Drawing.Point(6, 43);
            this.dataGridViewDefectCount.MultiSelect = false;
            this.dataGridViewDefectCount.Name = "dataGridViewDefectCount";
            this.dataGridViewDefectCount.ReadOnly = true;
            this.dataGridViewDefectCount.RowHeadersVisible = false;
            this.dataGridViewDefectCount.RowTemplate.Height = 24;
            this.dataGridViewDefectCount.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewDefectCount.Size = new System.Drawing.Size(188, 257);
            this.dataGridViewDefectCount.TabIndex = 0;
            this.dataGridViewDefectCount.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDefectCount_CellClick);
            this.dataGridViewDefectCount.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseUp);
            // 
            // ColumnVisible
            // 
            this.ColumnVisible.HeaderText = "標記";
            this.ColumnVisible.Name = "ColumnVisible";
            this.ColumnVisible.ReadOnly = true;
            this.ColumnVisible.Width = 43;
            // 
            // Column報廢代碼
            // 
            this.Column報廢代碼.HeaderText = "報廢代碼";
            this.Column報廢代碼.Name = "Column報廢代碼";
            this.Column報廢代碼.ReadOnly = true;
            this.Column報廢代碼.Width = 92;
            // 
            // ColumnPCS數
            // 
            this.ColumnPCS數.HeaderText = "PCS數";
            this.ColumnPCS數.Name = "ColumnPCS數";
            this.ColumnPCS數.ReadOnly = true;
            this.ColumnPCS數.Width = 72;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.dataGridViewDefectCount);
            this.groupBox2.Controls.Add(this.textBoxAlwaysShow);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox2.ForeColor = System.Drawing.Color.Blue;
            this.groupBox2.Location = new System.Drawing.Point(0, 413);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 294);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "報廢數(未排除重複PCS)";
            // 
            // textBoxAlwaysShow
            // 
            this.textBoxAlwaysShow.Location = new System.Drawing.Point(75, 17);
            this.textBoxAlwaysShow.Name = "textBoxAlwaysShow";
            this.textBoxAlwaysShow.Size = new System.Drawing.Size(117, 25);
            this.textBoxAlwaysShow.TabIndex = 1;
            this.textBoxAlwaysShow.Text = "B0011,B0029";
            this.toolTip1.SetToolTip(this.textBoxAlwaysShow, "EX: B0011,B0029");
            this.textBoxAlwaysShow.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxBarcodeID_KeyDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(4, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 15);
            this.label7.TabIndex = 14;
            this.label7.Text = "固定顯示";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 716);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1157, 22);
            this.statusStrip1.TabIndex = 20;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Red;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(172, 17);
            this.toolStripStatusLabel1.Text = "目檢與電測X-Out顯示在Side B";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "Input";
            // 
            // radioButtonSPNL
            // 
            this.radioButtonSPNL.AutoSize = true;
            this.radioButtonSPNL.Checked = true;
            this.radioButtonSPNL.Location = new System.Drawing.Point(2, 3);
            this.radioButtonSPNL.Name = "radioButtonSPNL";
            this.radioButtonSPNL.Size = new System.Drawing.Size(50, 16);
            this.radioButtonSPNL.TabIndex = 0;
            this.radioButtonSPNL.TabStop = true;
            this.radioButtonSPNL.Text = "SPNL";
            this.toolTip1.SetToolTip(this.radioButtonSPNL, "固定XOutSide朝上，缺角在下");
            this.radioButtonSPNL.UseVisualStyleBackColor = true;
            this.radioButtonSPNL.CheckedChanged += new System.EventHandler(this.radioButtonSPNL_CheckedChanged);
            // 
            // radioButtonWPNL
            // 
            this.radioButtonWPNL.AutoSize = true;
            this.radioButtonWPNL.Location = new System.Drawing.Point(68, 3);
            this.radioButtonWPNL.Name = "radioButtonWPNL";
            this.radioButtonWPNL.Size = new System.Drawing.Size(55, 16);
            this.radioButtonWPNL.TabIndex = 0;
            this.radioButtonWPNL.Text = "WPNL";
            this.toolTip1.SetToolTip(this.radioButtonWPNL, "L角固定在右上");
            this.radioButtonWPNL.UseVisualStyleBackColor = true;
            // 
            // listBoxWPNL
            // 
            this.listBoxWPNL.FormattingEnabled = true;
            this.listBoxWPNL.ItemHeight = 12;
            this.listBoxWPNL.Location = new System.Drawing.Point(14, 115);
            this.listBoxWPNL.Name = "listBoxWPNL";
            this.listBoxWPNL.ScrollAlwaysVisible = true;
            this.listBoxWPNL.Size = new System.Drawing.Size(177, 136);
            this.listBoxWPNL.TabIndex = 21;
            this.listBoxWPNL.SelectedIndexChanged += new System.EventHandler(this.listBoxWPNL_SelectedIndexChanged);
            // 
            // listBoxSPNL
            // 
            this.listBoxSPNL.FormattingEnabled = true;
            this.listBoxSPNL.ItemHeight = 12;
            this.listBoxSPNL.Location = new System.Drawing.Point(14, 271);
            this.listBoxSPNL.Name = "listBoxSPNL";
            this.listBoxSPNL.ScrollAlwaysVisible = true;
            this.listBoxSPNL.Size = new System.Drawing.Size(177, 136);
            this.listBoxSPNL.TabIndex = 21;
            this.listBoxSPNL.SelectedIndexChanged += new System.EventHandler(this.listBoxSPNL_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 12);
            this.label2.TabIndex = 14;
            this.label2.Text = "WPNL";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 256);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 12);
            this.label5.TabIndex = 14;
            this.label5.Text = "SPNL";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.radioButtonSideB);
            this.panel1.Controls.Add(this.radioButtonSideA);
            this.panel1.Location = new System.Drawing.Point(425, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(137, 25);
            this.panel1.TabIndex = 22;
            // 
            // radioButtonSideB
            // 
            this.radioButtonSideB.AutoSize = true;
            this.radioButtonSideB.Location = new System.Drawing.Point(41, 3);
            this.radioButtonSideB.Name = "radioButtonSideB";
            this.radioButtonSideB.Size = new System.Drawing.Size(87, 16);
            this.radioButtonSideB.TabIndex = 0;
            this.radioButtonSideB.Text = "B (XOutSide)";
            this.radioButtonSideB.UseVisualStyleBackColor = true;
            // 
            // radioButtonSideA
            // 
            this.radioButtonSideA.AutoSize = true;
            this.radioButtonSideA.Checked = true;
            this.radioButtonSideA.Location = new System.Drawing.Point(7, 3);
            this.radioButtonSideA.Name = "radioButtonSideA";
            this.radioButtonSideA.Size = new System.Drawing.Size(31, 16);
            this.radioButtonSideA.TabIndex = 0;
            this.radioButtonSideA.TabStop = true;
            this.radioButtonSideA.Text = "A";
            this.radioButtonSideA.UseVisualStyleBackColor = true;
            this.radioButtonSideA.CheckedChanged += new System.EventHandler(this.radioButtonSideA_CheckedChanged);
            // 
            // checkBoxScrapByHuman
            // 
            this.checkBoxScrapByHuman.AutoSize = true;
            this.checkBoxScrapByHuman.Location = new System.Drawing.Point(589, 10);
            this.checkBoxScrapByHuman.Name = "checkBoxScrapByHuman";
            this.checkBoxScrapByHuman.Size = new System.Drawing.Size(72, 16);
            this.checkBoxScrapByHuman.TabIndex = 23;
            this.checkBoxScrapByHuman.Text = "目檢報廢";
            this.checkBoxScrapByHuman.UseVisualStyleBackColor = true;
            this.checkBoxScrapByHuman.Visible = false;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.radioButtonWPNL);
            this.panel2.Controls.Add(this.radioButtonSPNL);
            this.panel2.Location = new System.Drawing.Point(273, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(137, 25);
            this.panel2.TabIndex = 22;
            // 
            // panelMap
            // 
            this.panelMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMap.AutoScroll = true;
            this.panelMap.Location = new System.Drawing.Point(198, 36);
            this.panelMap.Name = "panelMap";
            this.panelMap.Size = new System.Drawing.Size(959, 677);
            this.panelMap.TabIndex = 24;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(685, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(353, 16);
            this.label6.TabIndex = 0;
            this.label6.Text = "WPNL L角在右上(另一面座標需以透視方式看)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 82);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 12);
            this.label8.TabIndex = 14;
            this.label8.Text = "Detector";
            // 
            // labelMachine
            // 
            this.labelMachine.AutoEllipsis = true;
            this.labelMachine.Location = new System.Drawing.Point(58, 82);
            this.labelMachine.Name = "labelMachine";
            this.labelMachine.Size = new System.Drawing.Size(116, 12);
            this.labelMachine.TabIndex = 15;
            this.labelMachine.Text = "NA";
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 100;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.ShowAlways = true;
            // 
            // buttonLoadCSV
            // 
            this.buttonLoadCSV.Location = new System.Drawing.Point(1044, 9);
            this.buttonLoadCSV.Name = "buttonLoadCSV";
            this.buttonLoadCSV.Size = new System.Drawing.Size(101, 23);
            this.buttonLoadCSV.TabIndex = 25;
            this.buttonLoadCSV.Text = "載入PCS座標";
            this.buttonLoadCSV.UseVisualStyleBackColor = true;
            this.buttonLoadCSV.Click += new System.EventHandler(this.buttonLoadCSV_Click);
            // 
            // FormDefectMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1157, 738);
            this.Controls.Add(this.buttonLoadCSV);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.panelMap);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.checkBoxScrapByHuman);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.listBoxSPNL);
            this.Controls.Add(this.listBoxWPNL);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.labelEngNo);
            this.Controls.Add(this.labelMachine);
            this.Controls.Add(this.labelLot);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxInput);
            this.Controls.Add(this.label1);
            this.KeyPreview = true;
            this.Name = "FormDefectMap";
            this.Text = "KN Defect Map";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormDefectMap_FormClosed);
            this.Load += new System.EventHandler(this.FWasteSearch_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDefectCount)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxInput;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelLot;
        private System.Windows.Forms.Label labelEngNo;
        private System.Windows.Forms.DataGridView dataGridViewDefectCount;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnVisible;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column報廢代碼;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPCS數;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButtonSPNL;
        private System.Windows.Forms.RadioButton radioButtonWPNL;
        private System.Windows.Forms.ListBox listBoxWPNL;
        private System.Windows.Forms.ListBox listBoxSPNL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButtonSideB;
        private System.Windows.Forms.RadioButton radioButtonSideA;
        private System.Windows.Forms.CheckBox checkBoxScrapByHuman;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panelMap;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxAlwaysShow;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelMachine;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button buttonLoadCSV;
    }
}

