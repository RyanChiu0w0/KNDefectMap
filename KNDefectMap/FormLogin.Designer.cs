﻿namespace KNDefectMap
{
    partial class FLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxEmployeeNum = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxEmployeeNum
            // 
            this.textBoxEmployeeNum.Location = new System.Drawing.Point(96, 12);
            this.textBoxEmployeeNum.Name = "textBoxEmployeeNum";
            this.textBoxEmployeeNum.Size = new System.Drawing.Size(100, 22);
            this.textBoxEmployeeNum.TabIndex = 0;
            this.textBoxEmployeeNum.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxEmployeeNum_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "請輸入薪號";
            // 
            // FLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 50);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxEmployeeNum);
            this.Name = "FLogin";
            this.Text = "登入";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxEmployeeNum;
        private System.Windows.Forms.Label label1;
    }
}